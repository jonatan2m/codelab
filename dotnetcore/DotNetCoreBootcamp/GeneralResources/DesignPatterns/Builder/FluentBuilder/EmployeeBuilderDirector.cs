namespace DesignPatterns.Builder.FluentBuilder
{
    public class EmployeeBuilderDirector: EmployeeSalaryBuilder<EmployeeBuilderDirector>
    {
        public static EmployeeBuilderDirector NewEmployee => new EmployeeBuilderDirector();
    }
}
