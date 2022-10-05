namespace DesignPatterns.Builder.FluentBuilder
{
    public class EmployeeSalaryBuilder<T>: EmployeePositionBuilder<EmployeeSalaryBuilder<T>>
        where T: EmployeeSalaryBuilder<T>
    {
        public T WithSalary(double salary)
        {
            employee.Salary = salary;
            return (T)this;
        }
    }
}
