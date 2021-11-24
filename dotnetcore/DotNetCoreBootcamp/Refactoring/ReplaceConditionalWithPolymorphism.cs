using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Refactoring
{
    public class ReplaceConditionalWithPolymorphism
    {
        [Fact]
        public void ReplaceConditionalWithPolymorphismBefore()
        {

        }

    }

    public abstract class EmployeeType
    {
        public abstract int GetTypeCode();
    }

    public class Engineer : EmployeeType
    {
        public override int GetTypeCode()
        {
            throw new NotImplementedException();
        }
    }

    class Employee
    {
        private EmployeeType _type;
        private int _monthlySalary;
        private int _commission;
        private int _bonus;

        public Employee(EmployeeType type)
        {
            _type = type;
        }

        public int PayAmount()
        {
            return 0;
            //switch (_type.GetTypeCode())
            //{
            //    case EmployeeType.ENGINEER:
            //        return _monthlySalary;
            //    case EmployeeType.SALESMAN:
            //        return _monthlySalary + _commission;
            //    case EmployeeType.MANAGER:
            //        return _monthlySalary + _bonus;
            //    default:
            //        throw new ArgumentOutOfRangeException("Incorrect Employee");
            //}
        }
    }
}
