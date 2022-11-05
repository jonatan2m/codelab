using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.XUnitExample.CollectionTests
{
    public class Tests
    {
        [Fact]
        public void AllNumberIsEven()
        {
            var numbers = new List<int> { 2, 4, 6 };

            Action<int> allAreEvent = (a) =>
            {
                Assert.True(a % 2 == 0);
            };

            //Verifies that all items in the collection pass when executed against action.
            Assert.All(numbers, allAreEvent);

            Assert.All(numbers, item => Assert.True(item % 2 == 0));
        }

        [Fact]
        public void AllNumberIsEvenUsingFluentAssertions()
        {
            var numbers = new List<int> { 2, 4, 6 };

            numbers.Should().AllSatisfy(x => Assert.True(x % 2 == 0));
        }

        [Fact]
        public void AllNumberAreEvenAndNotZero()
        {
            var numbers = new List<int> { 2, 4, 6 };

            Assert.Collection(numbers, a => Assert.True(a == 2), a => Assert.True(a == 4), a => Assert.True(a == 6));
        }

        [Fact]
        public void ShouldClearWithEvents()
        {
            // arrange
            var target = new ObservableStack<string>();

            target.Push("1");

            // act
            Assert.PropertyChanged(target, "Count", target.Clear);            
        }
    }

    /// <summary>
    /// https://stackoverflow.com/questions/53889344/using-observablecollectiont-as-a-fifo-stack
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableStack<T> : Stack<T>, INotifyPropertyChanged
    {        
        public event PropertyChangedEventHandler? PropertyChanged;

        public new void Clear()
        {
            base.Clear();
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Count"));
        }
    }
}
