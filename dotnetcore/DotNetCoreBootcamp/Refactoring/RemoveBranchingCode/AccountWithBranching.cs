using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring.RemoveBranchingCode
{
    class AccountWithBranching
    {
        public decimal Balance { get; private set; }
        private Action OnUnfreeze { get; }
        

        private bool IsVerified { get; set; }
        private bool IsClosed { get; set; }
        private bool IsFrozen { get; set; }

        
        public AccountWithBranching(Action onUnfreeze)
        {
            OnUnfreeze = onUnfreeze;
        }

        public void Deposit(decimal amount)
        {
            if (IsClosed) return;
            if (IsFrozen)
            {
                IsFrozen = false;
                OnUnfreeze();
            }
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (IsVerified == false) return;
            if (IsClosed) return;
            if (IsFrozen)
            {
                IsFrozen = false;
                OnUnfreeze();
            }

            Balance -= amount;
        }

        public void HolderVerified()
        {
            IsVerified = true;
        }

        public void Close()
        {
            IsClosed = true;
        }

        public void Freeze()
        {
            if (IsClosed) return;
            if (IsVerified == false) return;

            IsFrozen = true;
        }

    }
}
