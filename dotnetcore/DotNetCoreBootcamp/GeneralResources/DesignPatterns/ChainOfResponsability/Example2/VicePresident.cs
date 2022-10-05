using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.ChainOfResponsability.Example2
{
    public class VicePresident : Approver
    {
        private static int limitToApprove = 100000;
        
        public VicePresident(string name)
            : base(name, true, limitToApprove)
        {
        }
    }
}
