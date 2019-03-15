using System;
using DesignPatterns.ChainOfResponsability.Example1;
using DesignPatterns.ChainOfResponsability.Example2;
using Xunit;

namespace DesignPatternsTest
{
    public class ChainOfResponsabilityTest : IDisposable
    {
        Approver vp;
        Approver manager;        
        Approver ceo;

        public ChainOfResponsabilityTest()
        {
            manager = new Manager("John Manager", false);            
            vp = new VicePresident("VP Mark");
            ceo = new CEO();

            manager.SetNextApprover(vp);
            vp.SetNextApprover(ceo);
        }

        public void Dispose()
        {
            vp = manager = null;            
        }


        [Fact]
        public void ChainOfResponsability_Example1()
        {
            DivNumbers div = new DivNumbers();
            MultNumbers mult = new MultNumbers(div);
            SubtractNumbers sub = new SubtractNumbers(mult);
            AddNumbers rootOperator = new AddNumbers(sub);

            Numbers numbers = new Numbers(2, 4, CalcOperator.Mult);
            var result = rootOperator.Calculate(numbers);

            Assert.Equal(8, result);
        }

        [Fact]
        public void CoR_Ex2_should_manager_approves_expenses_amout_500()
        {
            ExpenseReport expense = new ExpenseReport(
                amount: 500,
                isInternationalTravel: false);

            manager.Handle(expense);

            assertApprovedBy(manager, expense);
        }

        [Fact]
        public void CoR_Ex2_should_VP_aprroves_expenses_amout_500_and_international()
        {
            ExpenseReport expense = new ExpenseReport(
                amount: 500,
                isInternationalTravel: true);

            manager.Handle(expense);

            assertApprovedBy(vp, expense);
        }

        [Fact]
        public void CoR_Ex2_should_AutoReject_rejects_expenses_amout_6000()
        {
            ExpenseReport expense = new ExpenseReport(
                amount: 600000,
                isInternationalTravel: false);
            ceo.IsOutOfOffice = true;
            manager.Handle(expense);

            Assert.Equal(ExpenseState.Rejected, expense.State);
        }

        [Fact]
        public void CoR_Ex2_should_vp_approves_expenses_amout_50000()
        {            
            ExpenseReport expense = new ExpenseReport(
                amount: 50000,
                isInternationalTravel: false);

            manager.Handle(expense);

            assertApprovedBy(vp, expense);
        }

        private void assertApprovedBy(Approver approver, ExpenseReport expense)
        {
            Assert.Equal(ExpenseState.Approved, expense.State);
            Assert.Equal(approver, expense.Approver);
        }

        private void assertRejectedBy(Approver approver, ExpenseReport expense)
        {
            Assert.Equal(ExpenseState.Rejected, expense.State);
            Assert.Equal(approver, expense.Approver);
        }
    }
}
