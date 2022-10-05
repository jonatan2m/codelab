using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.ChainOfResponsability.Example2
{
    public class AutoReject : Approver
    {        
        public AutoReject()
            : base("auto-reject", false, 0)
        { }

        public override void Handle(ExpenseReport expense)
        {
            expense.Reject(this);
        }
    }
}
