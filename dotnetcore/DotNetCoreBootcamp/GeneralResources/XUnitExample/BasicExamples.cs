using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.XUnitExample
{
    /// <summary>
    /// Basic example of xUnit
    /// </summary>
    public class BasicExamples
    {
        [Fact]
        public async Task Example01()
        {
            Assert.Contains("text", "This is a text");
            Assert.DoesNotContain("abc", "This is a text");

            var names = new List<string> { "Picard", "Kirk" };
            Assert.Contains(names, x => x == "Kirk");

            Assert.True("e".GetType() == typeof(string));
            Assert.False("11".GetType() == typeof(int));

            var regEx = @"\A[A-Z0-9+_.-]+\Z";
            Assert.DoesNotMatch(regEx, "This is a text");
            Assert.Matches(regEx, "A1+_.-");

            var set1 = new HashSet<int> { 1, 2, 3, 4, 5, 6 };
            var set2 = new HashSet<int> { 3, 4 };
            var notProperSubset = new HashSet<int> { 1, 2, 3, 4, 5, 6 };

            Assert.Subset(set1, set2);
            Assert.ProperSubset(set1, set2);

            // fails, https://mathinsight.org/definition/proper_subset
            //Assert.ProperSubset(set1, notProperSubset);

            var set3 = new HashSet<int> { 1, 2, 3, 4, 5, 6 };
            var set4 = new HashSet<int> { 3, 4 };
            var notProperSuperSet = new HashSet<int> { 1, 2, 3, 4, 5, 6 };

            Assert.Superset(set4, set3);
            Assert.ProperSuperset(set4, notProperSuperSet);

            // fails
            //Assert.ProperSuperset(set3, notProperSuperSet);

            //fails, case sensitive
            //Assert.Equal("text", "Text");
            Assert.Equal("text", "Text", ignoreCase: true);
            Assert.NotEqual("text", "Text");

            Assert.Empty(new List<int> { });
            Assert.NotEmpty(new List<int> { 1 });

            Assert.InRange(3, 1, 6);
            Assert.NotInRange(3, 1, 2);

            var listWithSingle = new List<int> { 1 };
            Assert.Single(listWithSingle);

            Assert.IsType<string>("passes");
            Assert.IsNotType<string>(1);

            Assert.IsAssignableFrom<IEnumerable<int>>(new List<int>());
            //fails
            //Assert.IsAssignableFrom<IDictionary<int, string>>(new List<int>());

            Assert.Null(null);
            Assert.NotNull(new List<int>());

            var obj1 = new object();
            var obj2 = obj1;
            var obj3 = new object();

            Assert.Same(obj1, obj2);
            Assert.NotSame(obj1, obj3);

            /* 
            Strictly relies on the built-in behavior of val1.Equals(val2),
            since Assert.Equal attempts to do a lot of reconciliation to get
            the correct answer for collections, etc.            
             */
            Assert.StrictEqual(1, 1);
            Assert.NotStrictEqual(2, 1);

            // Tests Without an Assert
            Exception ex = Record.Exception(ThrowException);
            Assert.NotNull(ex);
            ex = Record.Exception(() => ThrowException());
            Assert.NotNull(ex);
            ex = Record.Exception(NotThrowException);
            Assert.Null(ex);
            ex = await Record.ExceptionAsync(ThrowExceptionAsync);
            Assert.NotNull(ex);
        }

        private void NotThrowException()
        {
            //some code    
        }

        private void ThrowException()
        {
            throw new Exception();
        }

        private async Task ThrowExceptionAsync()
        {
            await Task.Delay(1);
            throw new Exception();
        }
    }
}
