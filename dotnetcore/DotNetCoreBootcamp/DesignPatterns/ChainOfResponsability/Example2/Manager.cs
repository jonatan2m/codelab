using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.ChainOfResponsability.Example2
{
    public class Manager : Approver
    {
        private static int limitToApprove = 5000;

        public Manager(string name, bool canApproveInternationalTravel)
            : base(name, canApproveInternationalTravel, limitToApprove)
        { }
    }
}
