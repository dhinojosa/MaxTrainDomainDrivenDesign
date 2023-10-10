using System.Diagnostics;
using NUnit.Framework;
using SampleApplication.Models;

namespace SpecFlowBankAccount.Steps;

[Binding]
public class BankAccountBinding
{
    private BankAccount? _bankAccount;


    [Given(@"an empty bank account")]
    public void GivenAnEmptyBankAccount()
    {
        _bankAccount = new BankAccount(0, "302939232-1221");
    }

    [When(@"a deposit of (.*) dollars is made")]
    public void WhenADepositOfDollarsIsMade(int p0)
    {
        Debug.Assert(_bankAccount != null, nameof(_bankAccount) + " != null");
        _bankAccount.Deposit(p0);
    }

    [Then(@"then balance should be (.*)")]
    public void ThenThenBalanceShouldBe(int p0)
    {
        Debug.Assert(_bankAccount != null, nameof(_bankAccount) + " != null");
        Assert.That(_bankAccount.Balance, Is.EqualTo(100));
    }
}
