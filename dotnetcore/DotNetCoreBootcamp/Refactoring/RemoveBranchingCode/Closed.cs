using System;

namespace Refactoring.RemoveBranchingCode
{
    public class Closed : IAccountState
    {
        public IAccountState Deposit(Action addToBalance) => this;

        public IAccountState Withdraw(Action subtractFromBalance) => this;

        public IAccountState Freeze() => this;

        public IAccountState Close() => this;

        public IAccountState HolderVerified() => this;
    }
}