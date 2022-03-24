using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace Refactoring.RemoveBranchingCode
{
    public interface IAccountState
    {
        IAccountState Deposit(Action addToBalance);
        IAccountState Withdraw(Action subtractFromBalance);
        IAccountState Freeze();
        IAccountState Close();
        IAccountState HolderVerified();
    }

    class AccountWithoutBranching
    {
        public decimal Balance { get; private set; }

        private IAccountState Freezable { get; set; }

        private bool IsVerified { get; set; }
        private bool IsClosed { get; set; }

        public AccountWithoutBranching(Action onUnfreeze)
        {
            Freezable = new NotVerified(onUnfreeze);
        }

        public void Deposit(decimal amount)
        {
            Freezable = Freezable.Deposit(() => Balance += amount);
        }

        public void Withdraw(decimal amount)
        {
            Freezable = Freezable.Withdraw(() => Balance -= amount);
        }

        public void HolderVerified()
        {
            Freezable = Freezable.HolderVerified();
        }

        public void Close()
        {
            Freezable = Freezable.Close();
        }

        public void Freeze()
        {
            Freezable = Freezable.Freeze();
        }
    }
}
