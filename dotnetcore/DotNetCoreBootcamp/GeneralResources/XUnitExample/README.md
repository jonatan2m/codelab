# Ferramentas complementares
Mocking: Moq, FakeitEasy and NSubstitute

Assertion Helpers: Fluent Assertions https://fluentassertions.com/introduction

Data driven: AutoFixture  (ajuda a gerar dados para não perdermos tempo nos teste)

# Write your first theory
xUnit.net includes support for two different major types of unit tests: facts and theories. 

> *Facts* are tests which are always true. They test invariant conditions.

> *Theories* are tests which are only true for a particular set of data.

A good example of this is testing numeric algorithms.
Let's say you want to test an algorithm which determines whether a number is odd or not.
If you're writing the positive-side tests (odd numbers), then feeding even numbers into the test would cause it fail, and not because the test or algorithm is wrong.

Let's add a theory to our existing facts (including a bit of bad data, so we can see it fail):

[Theory]
[InlineData(3)]
[InlineData(5)]
[InlineData(6)]
public void MyFirstTheory(int value)
{
    Assert.True(IsOdd(value));
}

bool IsOdd(int value)
{
    return value % 2 == 1;
}

# How many Assertions Per Unit Test?
If you are testing something is related, don't worry about the number of Assertions, otherwise, you should consider separating those scenarios

# Tests Without an Assert
It is possible to write tests without Assertions. In this situation, you desire to verify if an error count raise in any moment.
Use Record.Exception to handle it.

# How xUnit run tests
By xUnit 2, the tests run in parallel. Each class is a test collection and this is what run in parallel.
The tests that are inside those classes, run sequencially.
more info about configuration tests can be found here [https://xunit.net/docs/configuration-files]

# Sharing context between tests
Avoid wasting resources to create objects every time the tests run.
ClassFixture and Collection Fixtures will help us in this mission.
*ClassFixture*: Use it when you want to create a single test context and share it among all the tests in the class,
and have it cleaned up after all the tests in the class have finished.

*Collection Fixtures*: Use it when you want to create a single test context and share it among tests in several _test classes_,
and have it cleaned up after all the tests in the test classes have finished.

# Categorizing tests
We can use Traits to categorize tests

# Working with complex types
*InlineData*: Use it directly on the test

*ClassData*: Create a class to use in your test

*MemberData*: You can use any method (static, though) as you use with ClassData

*TheoryData*: It attributes has many advantages. (prefer use it instead of ClassData and MemberData)
First of all, it’s simple and less code to write.
Second it’s strongly-typed code which is going to give us compile time error if anything in wrong in our code.