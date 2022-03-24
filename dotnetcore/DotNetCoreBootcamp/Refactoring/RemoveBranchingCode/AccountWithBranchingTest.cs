using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Refactoring.RemoveBranchingCode
{
    public class AccountWithBranchingTest
    {
        internal readonly AccountWithBranching _account;
        private bool UnFreezeWasCalled = false;

        public AccountWithBranchingTest()
        {
            _account = new AccountWithBranching(() => UnFreezeWasCalled = true);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(10.23)]
        public void Given_an_active_account_should_deposit_balance(decimal amount)
        {
            // Arrange & Act
            _account.Deposit(amount);
            _account.Deposit(amount);

            // Assert
            Assert.Equal(amount * 2, _account.Balance);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(10.23)]
        public void Given_a_closed_account_should_not_deposit_any_value(decimal amount)
        {
            // Arrange
            _account.Deposit(amount);
            _account.Close();

            // Act
            _account.Deposit(amount);

            // Assert 
            Assert.Equal(amount, _account.Balance);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(10.23)]
        public void Given_a_verified_account_should_withdraw_balance(decimal amount)
        {
            // Arrange
            _account.Deposit(amount);
            _account.HolderVerified();
            
            // Act
            _account.Withdraw(amount / 2);

            // Assert
            Assert.Equal(amount / 2, _account.Balance);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(10.23)]
        public void Given_a_non_verified_account_should_not_withdraw_any_value(decimal amount)
        {
            // Arrange
            _account.Deposit(amount);
            decimal previousBalance = _account.Balance;

            // Act
            _account.Withdraw(amount);

            // Assert 
            Assert.Equal(previousBalance, _account.Balance);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(10.23)]
        public void Given_a_frozen_account_should_make_it_active_when_deposit_any_value(decimal amount)
        {
            // Arrange
            _account.Deposit(amount);
            _account.HolderVerified();
            _account.Freeze();

            // Act
            _account.Withdraw(amount);

            // Assert
            Assert.Equal(decimal.Zero, _account.Balance);
            Assert.True(UnFreezeWasCalled);
        }
    }
}
