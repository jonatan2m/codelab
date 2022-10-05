namespace DesignPatterns.Builder.FluentBuilder
{
    public class EmployeeInfoBuilder<T> : EmployeeBuilder where T: EmployeeInfoBuilder<T>
    {
        public T SetName(string name)
        {
            employee.Name = name;
            return (T)this;
        }
    }
}
