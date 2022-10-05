namespace DesignPatterns.ChainOfResponsability.Example2
{
    public enum ExpenseState
    {
        Initial,
        Approved,
        Rejected
    }

    /// <summary>
    /// <para>Client. This class hold reference to ultimately approver</para>
    /// </summary>
    public class ExpenseReport
    {
        public bool IsInternationalTravel { get; private set; }
        public int Amount { get; private set; }
        public ExpenseState State { get; private set; }
        public Approver Approver { get; set; }

        public ExpenseReport(int amount, bool isInternationalTravel)
        {
            Amount = amount;
            IsInternationalTravel = isInternationalTravel;
            State = ExpenseState.Initial;
        }

        public void Approve(Approver approver)
        {
            Approver = approver;
            State = ExpenseState.Approved;
        }

        public void Reject(Approver approver)
        {
            Approver = approver;
            State = ExpenseState.Rejected;
        }
    }
}