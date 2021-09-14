using System;
using Xunit;

namespace Refactoring
{
    public class ReplaceMethodWithMethodObject
    {
        [Fact]
        public void Test1()
        {

        }
    }

    class AccountBefore
    {
        int gamma(int inputVal, int quantity, int yearToDate)
        {
            int importantValue1 = (inputVal * quantity) + delta();
            int importantValue2 = (inputVal * yearToDate) + 100;
            if ((yearToDate - importantValue1) > 100)
                importantValue2 -= 20;
            int importantValue3 = importantValue2 * 7;
            // and so on.
            return importantValue3 - 2 * importantValue1;
        }

        public int delta()
        {
            //whatever non-zero value
            return 1;
        }
    }

    class Account
    {
        int gamma(int inputVal, int quantity, int yearToDate)
        {
            return new Gamma(this, inputVal, quantity, yearToDate).Compute();            
        }

        public int delta()
        {
            //whatever non-zero value
            return 1;
        }
    }

    class Gamma
    {
        readonly Account _account;
        readonly int _inputVal;
        readonly int _quantity;
        readonly int _yearToDate;
        int _importantValue1;
        int _importantValue2;
        int _importantValue3;

        public Gamma(Account account, int inputVal, int quantity, int yearToDate)
        {
            _account = account;
            _inputVal = inputVal;
            _quantity = quantity;
            _yearToDate = yearToDate;
        }

        public int Compute()
        {
            _importantValue1 = (_inputVal * _quantity) + _account.delta();
            _importantValue2 = (_inputVal * _yearToDate) + 100;
            if ((_yearToDate - _importantValue1) > 100)
                _importantValue2 -= 20;
            _importantValue3 = _importantValue2 * 7;
            // and so on.
            return _importantValue3 - 2 * _importantValue1;
        }

    }
}
