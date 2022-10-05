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

        private IAccountState State { get; set; }

        public AccountWithoutBranching(Action onUnfreeze)
        {
            State = new NotVerified(onUnfreeze);
        }

        public void Deposit(decimal amount)
        {
            State = State.Deposit(() => Balance += amount);
        }

        public void Withdraw(decimal amount)
        {
            State = State.Withdraw(() => Balance -= amount);
        }

        public void HolderVerified()
        {
            State = State.HolderVerified();
        }

        public void Close()
        {
            State = State.Close();
        }

        public void Freeze()
        {
            State = State.Freeze();
        }
    }
}
