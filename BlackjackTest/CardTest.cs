using Blackjack;
using NUnit.Framework;

namespace BlackjackTest;

[TestFixture]
public class CardTest
{
    [Test]
    public void WithNumberCardHasNumericValueOfTheNumber()
    {
        Card card = new Card("don't care", "7");
        Assert.That(card.RankValue(), Is.EqualTo(7));
    }

    [Test]
    public void WithValueOfQueenHasNumericValueOf10()
    {
        Card card = new Card("don't care", "Q");
        Assert.That(card.RankValue(), Is.EqualTo(10));
    }

    [Test]
    public void WithAceHasNumericValueOf1()
    {
        Card card = new Card("don't care", "A");
        Assert.That(card.RankValue(), Is.EqualTo(1));
    }
}
