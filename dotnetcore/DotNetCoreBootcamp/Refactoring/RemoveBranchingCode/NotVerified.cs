using System;

namespace Refactoring.RemoveBranchingCode
{
    public class NotVerified : IAccountState
    {
        private Action OnUnfreeze { get; }

        public NotVerified(Action onUnfreeze)
        {
            OnUnfreeze = onUnfreeze;
        }

        public IAccountState Deposit(Action addToBalance)
        {
            addToBalance();
            return this;
        }

        public IAccountState Withdraw(Action subtractFromBalance) => this;

        public IAccountState Freeze() => this;

        public IAccountState Close() => new Closed();

        public IAccountState HolderVerified() => new Active(OnUnfreeze);
    }
}