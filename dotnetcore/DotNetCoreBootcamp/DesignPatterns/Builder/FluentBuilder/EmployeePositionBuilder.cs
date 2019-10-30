namespace DesignPatterns.Builder.FluentBuilder
{
    public class EmployeePositionBuilder<T>: EmployeeInfoBuilder<EmployeePositionBuilder<T>> 
        where T: EmployeePositionBuilder<T>
    {
        public T AtPosition(string position)
        {
            employee.Position = position;
            return (T)this;
        }
    }
}
