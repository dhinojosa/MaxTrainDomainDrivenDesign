namespace SampleApplication.Models;

public class BankAccount
{
    public string Number { get; }
    public int Balance { get; set; }

    public BankAccount(int balance, string number)
    {
        this.Balance = balance;
        this.Number = number;
    }

    public void Deposit(int amount)
    {
        this.Balance += amount;
    }

    public int Withdrawal(int amount)
    {
        this.Balance -= amount;
        return amount;
    }
}
