using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.ChainOfResponsability.Example2
{
    public abstract class Approver
    {
        public string Name { get; set; }
        public int LimitToApprove { get; set; }
        public bool CanApproveInternationalTravel { get; set; }
        public bool IsOutOfOffice { get; set; }
        protected Approver NextApprover { get; set; }

        public Approver(string name, bool canApproveInternationalTravel, int limitToApprove)
        {
            Name = name;
            CanApproveInternationalTravel = canApproveInternationalTravel;
            LimitToApprove = limitToApprove;
        }
        
        public void SetNextApprover(Approver approver)
        {
            NextApprover = approver;
        }

        public virtual void Handle(ExpenseReport expense)
        {
            if (CanApprove(expense))
                expense.Approve(this);
            else if (NextApprover != null)
                NextApprover.Handle(expense);
            else
                expense.Reject(this);
        }

        private bool CanApprove(ExpenseReport expense)
        {
            return expense.Amount <= LimitToApprove && IsOutOfOffice == false &&
                (CanApproveInternationalTravel || expense.IsInternationalTravel == false);
        }

    }
}
