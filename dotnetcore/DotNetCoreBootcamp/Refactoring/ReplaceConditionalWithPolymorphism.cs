using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Refactoring
{
    //Replace Type Code with State/Strategy
    class Employee
    {
        private int _monthlySalary;
        private int _commission;
        private int _bonus;

        private EmployeeType _type;

        Employee(int type)
        {
            setType(type);
        }

        //step one - encapsulate the type
        int getType()
        {
            return _type.getTypeCode();
        }

        void setType(int arg)
        {
            _type = EmployeeType.newType(arg);            
        }

        int payAmount()
        {
            switch (getType())
            {
                case EmployeeType.ENGINEER:
                    return _monthlySalary;
                case EmployeeType.SALESMAN:
                    return _monthlySalary + _commission;
                case EmployeeType.MANAGER:
                    return _monthlySalary + _bonus;
                default:
                    throw new ArgumentException("Incorrect Employee");
            }
        }
    }

    //step two - declare abstract class and a method for returning the type code
    abstract class EmployeeType
    {
        //step four - move all knowlegde about codes and subclasses over to the EmployeeType class
        public const int ENGINEER = 0;
        public const int SALESMAN = 1;
        public const int MANAGER = 2;

        public abstract int getTypeCode();

        //step five - Bring the Employee.setType method statement to this class and create a such of factory method.
        public static EmployeeType newType(int code)
        {
            switch (code)
            {
                case ENGINEER:
                    return new Engineer();
                case SALESMAN:
                    return new Salesman();
                case MANAGER:
                    return new Manager();
                default:
                    throw new ArgumentException("Incorrect Employee Code");
            }
        }

        //step three - create subclasses
        class Engineer : EmployeeType
        {
            public override int getTypeCode()
            {
                return Employee.ENGINEER;
            }
        }

        //step three - create subclasses
        class Manager : EmployeeType
        {
            public override int getTypeCode()
            {
                return Employee.MANAGER;
            }
        }

        //step three - create subclasses
        class Salesman : EmployeeType
        {
            public override int getTypeCode()
            {
                return Employee.SALESMAN;
            }
        }


        public class ReplaceConditionalWithPolymorphism
        {
            [Fact]
            public void ReplaceConditionalWithPolymorphismBefore()
            {

            }

        }

    }
