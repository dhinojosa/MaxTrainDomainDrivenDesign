using NUnit.Framework;
using Blackjack;

namespace BlackjackTest;

[TestFixture]
public class HandValueAceTest
{
    [Test]
    public void HandWithOneAceTwoCardsIsValuedAt11()
    {
        Game game = new Game();
        var cards = new List<Card>
        {
            new Card("whocares", "A"),
            new Card("whocares", "5")
        };

        Assert.That(game.HandValueOf(cards), Is.EqualTo(11 + 5));
    }

    [Test]
    public void HandWithOneAceAndOtherCardsEqualTo11IsValuedAt1()
    {
        Game game = new Game();
        List<Card> cards = new List<Card>
        {
            new Card("whocares", "A"),
            new Card("whocares", "8"),
            new Card("whocares", "3")
        };

        Assert.That(game.HandValueOf(cards), Is.EqualTo(1 + 8 + 3));
    }
}

