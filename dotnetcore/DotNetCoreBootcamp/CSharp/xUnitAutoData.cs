
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class AutoData : DataAttribute
{
    protected Func<Type, object> Factory;

    public static object New(Type type)
    {
        var random = new Random();
        return Create(new Dictionary<Type, object>(), type, null, random);
    }

    public static T New<T>(Action<T> f = null)
    {
        var random = new Random();
        var item = (T)Create(new Dictionary<Type, object>(), typeof(T), null, random);
        f?.Invoke(item);
        return item;
    }

    private readonly string[] Names;

    public AutoData(params string[] names)
    {
        Names = names ?? new string[] { };
    }

    protected virtual void GetObjects(MethodInfo method, Dictionary<Type, object> dic)
    {
        foreach (var item in Names)
        {
            var m = method.DeclaringType.GetMethod(item);
            var result = m.Invoke(null, null);
            dic.Add(m.ReturnType, result);
        }
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var random = new Random();

        var dic = new Dictionary<Type, object>();
        GetObjects(testMethod, dic);

        var list = new List<object>();
        foreach (var item in testMethod.GetParameters())
        {
            list.Add(Create(dic, item.ParameterType, item, random));
        }

        return new[] { list.ToArray() };
    }

    protected T Create<T>()
    {
        var random = new Random();
        return (T)Create(new Dictionary<Type, object>(), typeof(T), null, random);
    }

    private static object Create(Dictionary<Type, object> dic, Type parameterType, ParameterInfo parameter, Random random)
    {
        if (dic.TryGetValue(parameterType, out var svc))
        {
            if (svc is Func<object> f)
                return f();
            else
                return dic[parameterType];
        }

        if (parameterType == typeof(Guid) || parameterType == typeof(Guid?))
        {
            return Guid.NewGuid();
        }
        else if (parameterType == typeof(string))
        {
            return Guid.NewGuid().ToString();
        }
        else if (parameterType == typeof(double))
        {
            return random.NextDouble();
        }
        else if (parameterType == typeof(int))
        {
            return random.Next();
        }
        else if (parameterType == typeof(bool))
        {
            return false;
        }
        else if (parameterType == typeof(DateTime))
        {
            if (parameter != null)
            {
                var name = parameter.Name;
                if (name == "nowUtc") return DateTime.UtcNow;
                else if (name == "now") return DateTime.Now;

                if (name.StartsWith("local") && DateTime.TryParseExact(
                    s: name.Replace("at", ""),
                    formats: new[] {
                            "yyyyMMdd",
                            "yyyyMMddHHmmss"
                    },
                    provider: CultureInfo.CurrentCulture,
                    style: DateTimeStyles.AssumeLocal,
                    result: out DateTime r1))
                {
                    return r1;
                }
                else if (name.StartsWith("utc") && DateTime.TryParseExact(
                    s: name.Replace("utc", ""),
                    formats: new[] {
                            "yyyyMMdd",
                            "yyyyMMddHHmmss"
                    },
                    provider: CultureInfo.CurrentCulture,
                    style: DateTimeStyles.AssumeUniversal,
                    result: out DateTime r2))
                {
                    return r2;
                };
            };

            return DateTime.Now;
        }
        else if (parameterType.FullName.StartsWith("System.Func"))
        {
            var returnType = parameterType.GetGenericArguments().Last();
            if ((returnType == typeof(Task)) || (returnType.IsConstructedGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)))
            {
                return NSubstitute.Substitute.For(new[] { parameterType }, null);
            }
            var returnInstance = Create(dic, returnType, null, random);
            return Expression.Lambda(
                parameterType,
                Expression.Constant(returnInstance))
                .Compile();
        }
        else if (parameterType.GetTypeInfo().IsInterface)
        {
            return NSubstitute.Substitute.For(new[] { parameterType }, null);
        }
        else if (parameterType.GetTypeInfo().IsClass)
        {
            var constructor = parameterType.GetTypeInfo().GetConstructor(new Type[] { });

            // Constructor does not have dependency probably a DTO
            if (constructor != null)
            {
                return CreateDto(parameterType, constructor);
            }
            else
            {
                return CreateService(dic, parameterType, random);
            }
        }

        throw new NotImplementedException();
    }

    public Task<TR> Run<T1, T2, T3, TR>(Dictionary<Type, object> dic, Func<T1, T2, T3, Task<TR>> f)
    {
        var t1 = (T1)Create(dic, typeof(T1), null, null);
        var t2 = (T2)Create(dic, typeof(T2), null, null);
        var t3 = (T3)Create(dic, typeof(T3), null, null);
        return f(t1, t2, t3);
    }

    public Task<TR> Run<T1, T2, T3, T4, TR>(Dictionary<Type, object> dic, Func<T1, T2, T3, T4, Task<TR>> f)
    {
        var t1 = (T1)Create(dic, typeof(T1), null, null);
        var t2 = (T2)Create(dic, typeof(T2), null, null);
        var t3 = (T3)Create(dic, typeof(T3), null, null);
        var t4 = (T4)Create(dic, typeof(T4), null, null);
        return f(t1, t2, t3, t4);
    }

    private static object CreateDto(Type parameterType, ConstructorInfo constructor)
    {
        var instance = constructor.Invoke(null);

        foreach (var property in parameterType.GetProperties())
        {
            if (property.CanWrite == false) continue;

            if (property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?))
            {
                property.SetValue(instance, Guid.NewGuid());
            }
            else if (property.PropertyType == typeof(string))
            {
                property.SetValue(instance, $"{property.PropertyType.Name}-{Guid.NewGuid().ToString()}");
            }
            else if (property.PropertyType.IsArray)
            {
            }
            else if (property.PropertyType.IsClass)
            {
                constructor = property.PropertyType.GetConstructor(new Type[] { });

                // Constructor does not have dependency probably a DTO
                if (constructor != null)
                {
                    try
                    {
                        var value = CreateDto(property.PropertyType, constructor);
                        property.SetValue(instance, value);
                    }
                    catch
                    {

                    }
                }
            }
        }

        return instance;
    }

    private static object CreateService(Dictionary<Type, object> dic, Type parameterType, Random random)
    {
        var constructor = parameterType.GetConstructors()
            .OrderByDescending(x => x.GetParameters().Length)
            .First();

        var list = new List<object>();

        foreach (var item in constructor.GetParameters())
        {
            list.Add(Create(dic, item.ParameterType, null, random));
        }

        return NSubstitute.Substitute.For(new[] { parameterType }, list.ToArray());
    }
}