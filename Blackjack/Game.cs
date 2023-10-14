using System.Text;
using Ansi;

namespace Blackjack;

public class Game
{
    private readonly Deck _deck = new();

    private readonly List<Card> _dealerHand = new List<Card>();
    private readonly List<Card> _playerHand = new List<Card>();

    public static void Main(string[] args)
    {
        Game game = new Game();
        Console.WriteLine(new StringBuilder()
            .SetMode(Mode.BackgroundWhite)
            .SetMode(Mode.ForegroundBlack)
            .Clear()
            .SetCursorPosition(0, 0)
            .SetMode(Mode.ForegroundGreen)
            .Append("Welcome to")
            .SetMode(Mode.ForegroundRed)
            .Append("Jitterted's")
            .SetMode(Mode.ForegroundBlack)
            .Append("Blackjack")
            .HideCursor()
            .SaveState()
        );

        game.InitialDeal();
        game.Play();

        Console.ResetColor();
    }

    public void InitialDeal()
    {
        // Deal the first round of cards, players first
        _playerHand.Add(_deck.Draw());
        _dealerHand.Add(_deck.Draw());

        // Deal the next round of cards
        _playerHand.Add(_deck.Draw());
        _dealerHand.Add(_deck.Draw());
    }

    public void Play()
    {
        // Get Player's decision: hit until they stand, then they're done (or they go bust)
        bool playerBusted = false;
        while (!playerBusted)
        {
            DisplayGameState();
            string playerChoice = InputFromPlayer().ToLower();
            if (playerChoice.StartsWith("s"))
            {
                break;
            }

            if (playerChoice.StartsWith("h"))
            {
                _playerHand.Add(_deck.Draw());
                if (HandValueOf(_playerHand) > 21)
                {
                    playerBusted = true;
                }
            }
            else
            {
                Console.WriteLine("You need to [H]it or [S]tand");
            }
        }

        // Dealer makes its choice automatically based on a simple heuristic (<=16, hit, 17>=stand)
        if (!playerBusted)
        {
            while (HandValueOf(_dealerHand) <= 16)
            {
                _dealerHand.Add(_deck.Draw());
            }
        }

        DisplayFinalGameState();

        if (playerBusted)
        {
            Console.WriteLine("You Busted, so you lose.  💸");
        }
        else if (HandValueOf(_dealerHand) > 21)
        {
            Console.WriteLine("Dealer went BUST, Player wins! Yay for you!! 💵");
        }
        else if (HandValueOf(_dealerHand) < HandValueOf(_playerHand))
        {
            Console.WriteLine("You beat the Dealer! 💵");
        }
        else if (HandValueOf(_dealerHand) == HandValueOf(_playerHand))
        {
            Console.WriteLine("Push: You tie with the Dealer. 💸");
        }
        else
        {
            Console.WriteLine("You lost to the Dealer. 💸");
        }
    }

    public int HandValueOf(List<Card> hand)
    {
        int handValue = hand
            .Sum(card => card.RankValue());

        // Does the hand contain at least 1 Ace?
        bool hasAce = hand
            .Any(card => card.RankValue() == 1);

        // If the total hand value <= 11, then count the Ace as 11 by adding 10
        if (hasAce && handValue < 11)
        {
            handValue += 10;
        }

        return handValue;
    }

    private string InputFromPlayer()
    {
        Console.WriteLine("[H]it or [S]tand?");
        return Console.ReadLine();
    }

    private void DisplayGameState()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 2);
        Console.WriteLine("Dealer has: ");
        Console.WriteLine(_dealerHand[0].Display()); // First card is face up

        // Second card is the hole card, which is hidden
        DisplayBackOfCard();

        Console.WriteLine();
        Console.WriteLine("Player has: ");
        DisplayHand(_playerHand);
        Console.WriteLine(" (" + HandValueOf(_playerHand) + ")");
    }

    private void DisplayBackOfCard()
    {
        Console.WriteLine(new StringBuilder().Up(7)
            .Right(12)
            .Append("┌─────────┐").Down(1).Left(11)
            .Append("│░░░░░░░░░│").Down(1).Left(11)
            .Append("│░ J I T ░│").Down(1).Left(11)
            .Append("│░ T E R ░│").Down(1).Left(11)
            .Append("│░ T E D ░│").Down(1).Left(11)
            .Append("│░░░░░░░░░│").Down(1).Left(11)
            .Append("└─────────┘"));
    }

    private void DisplayHand(List<Card> hand)
    {
        var strings = hand.Select(card => card.Display()).ToArray();
        Console.WriteLine(string.Join(new StringBuilder().Up(6).Right(1).ToString(), strings));
    }

    private void DisplayFinalGameState()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("Dealer has: ");
        DisplayHand(_dealerHand);
        Console.WriteLine(" (" + HandValueOf(_dealerHand) + ")");

        Console.WriteLine();
        Console.WriteLine("Player has: ");
        DisplayHand(_playerHand);
        Console.WriteLine(" (" + HandValueOf(_playerHand) + ")");
    }
}
