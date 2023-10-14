namespace Blackjack;

public class Deck
{
    private readonly List<Card> _cards = new List<Card>();

    public Deck()
    {
        List<string> cardValues = new List<string> { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        List<string> suits = new List<string> { "♠", "♦", "♥", "♣" };
        foreach (var suit in suits)
        {
            foreach (var cardValue in cardValues)
            {
                _cards.Add(new Card(suit, cardValue));
            }
        }

        Shuffle(_cards);
    }

    public int Size()
    {
        return _cards.Count;
    }

    public Card Draw()
    {
        Card drawnCard = _cards[0];
        _cards.RemoveAt(0);
        return drawnCard;
    }

    // Custom shuffle method to replace Collections.shuffle
    private void Shuffle<T>(IList<T> list)
    {
        Random random = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}
