using System;

namespace Refactoring.RemoveBranchingCode
{
    public class Active : IAccountState
    {
        private Action OnUnfreeze { get; }

        public Active(Action onUnfreeze)
        {
            OnUnfreeze = onUnfreeze;
        }

        public IAccountState Deposit(Action addToBalance)
        {
            addToBalance();
            return this;
        }

        public IAccountState Withdraw(Action subtractFromBalance)
        {
            subtractFromBalance();
            return this;
        }
        public IAccountState Freeze() => new Frozen(OnUnfreeze);
        public IAccountState Close() => new Closed();
        public IAccountState HolderVerified() => this;
    }
}