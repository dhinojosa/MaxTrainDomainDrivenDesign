namespace Blackjack;

public class Game
{
    private readonly Deck _deck;

    private readonly List<Card> _dealerHand = new List<Card>();
    private readonly List<Card> _playerHand = new List<Card>();

    public static void Main(string[] args)
    {
        Game game = new Game();
        Console.BackgroundColor = ConsoleColor.White;
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Welcome to");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" Jitterted's");
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(" BlackJack");

        game.InitialDeal();
        game.Play();

        Console.ResetColor();
    }

    public Game()
    {
        _deck = new Deck();
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
        Console.SetCursorPosition(0, 0);
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
        Console.SetCursorPosition(0, 7);
        Console.Write("┌─────────┐");
        Console.SetCursorPosition(12, 8);
        Console.Write("│░░░░░░░░░│");
        Console.SetCursorPosition(12, 9);
        Console.Write("│░ J I T ░│");
        Console.SetCursorPosition(12, 10);
        Console.Write("│░ T E R ░│");
        Console.SetCursorPosition(12, 11);
        Console.Write("│░ T E D ░│");
        Console.SetCursorPosition(0, 12);
        Console.Write("└─────────┘");
    }

    private void DisplayHand(List<Card> hand)
    {
        Console.WriteLine(Environment.NewLine);
        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop - 6);
        Console.WriteLine(hand.Select(card => card.Display()));
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
