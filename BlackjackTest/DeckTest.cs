using Blackjack;
using NUnit.Framework;

namespace BlackjackTest;

[TestFixture]
public class DeckTest
{
    [Test]
    public void FullDeckHas52Cards()
    {
        var deck = new Deck();
        Assert.That(deck.Size(), Is.EqualTo(52));
    }

    [Test]
    public void DrawCardFromDeckReducesDeckSizeByOne()
    {
        Deck deck = new Deck();
        deck.Draw();
        Assert.That(deck.Size(), Is.EqualTo(51));
    }

    [Test]
    public void DrawCardFromDeckReturnsValidCard()
    {
        var deck = new Deck();
        var card = deck.Draw();
        Assert.That(card, Is.Not.Null);
        Assert.That(card.RankValue(), Is.GreaterThan(0));
    }

    [Test]
    public void DrawAllCardsResultsInSetOf52UniqueCards()
    {
        var deck = new Deck();
        var drawnCards = new HashSet<Card>();
        for (var i = 1; i <= 52; i++)
        {
            drawnCards.Add(deck.Draw());
        }

        Assert.That(drawnCards, Has.Count.EqualTo(52));
    }
}
