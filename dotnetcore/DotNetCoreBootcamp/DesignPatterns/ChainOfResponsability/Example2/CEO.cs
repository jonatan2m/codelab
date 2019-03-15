namespace DesignPatterns.ChainOfResponsability.Example2
{
    public class CEO : Approver
    {
        private static int limitToApprove = int.MaxValue;

        public CEO()
            : base("John CEO", true, limitToApprove)
        {
            SetNextApprover(new AutoReject());
        }
    }
}