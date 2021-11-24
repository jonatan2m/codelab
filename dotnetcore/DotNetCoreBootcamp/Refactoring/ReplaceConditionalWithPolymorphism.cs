using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Refactoring
{
   

    //Replace Type Code with State/Strategy
    public class Employee
    {        
        private EmployeeType _type;        
        private int v;
        private readonly int _monthlySalary;                       
        private readonly int _commission;
        private readonly int _bonus;

        public Employee(int type, int monthlySalary, int commission = 0, int bonus = 0)
        {
            setType(type);
            _monthlySalary = monthlySalary;            
            _commission = commission;
            _bonus = bonus;
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

        public int getMonthlySalary()
        {
            return _monthlySalary;
        }

        public int getComission()
        {
            return _commission;
        }

        public int getBonus()
        {
            return _bonus;
        }

        public int payAmount()
        {
            //step seven - remove switch case from here and put it on EmployeeType, passing 'this' as parameter
            return _type.payAmount(this);            
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

        //step six - move payAmount to EmployeeType and add acessible methods to get salary, commission and bonus
        public virtual int payAmount(Employee emp)
        {
            switch (getTypeCode())
            {
                case EmployeeType.ENGINEER:
                    throw new InvalidOperationException("Should be overrided");
                case EmployeeType.SALESMAN:
                    //TODO Remove it for Salesman class
                    return emp.getMonthlySalary() + emp.getComission();
                case EmployeeType.MANAGER:
                    //TODO Remove it for Manager class
                    return emp.getMonthlySalary() + emp.getBonus();
                default:
                    throw new ArgumentException("Incorrect Employee");
            }
        }

        //step three - create subclasses
        class Engineer : EmployeeType
        {
            public override int getTypeCode()
            {
                return ENGINEER;
            }

            public override int payAmount(Employee emp)
            {
                return emp.getMonthlySalary();
            }
        }

        //step three - create subclasses
        class Manager : EmployeeType
        {
            public override int getTypeCode()
            {
                return MANAGER;
            }
        }

        //step three - create subclasses
        class Salesman : EmployeeType
        {
            public override int getTypeCode()
            {
                return SALESMAN;
            }
        }      
    }

    public class ReplaceConditionalWithPolymorphism
    {
        [Fact]
        public void ReplaceConditionalWithPolymorphismBefore()
        {
            var engineer = new Employee(EmployeeType.ENGINEER, 1000);

            var result = engineer.payAmount();

            Assert.Equal(1000, result);
        }
    }
}
