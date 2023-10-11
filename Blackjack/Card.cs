namespace Blackjack
{
    public class Card
    {
        private readonly string _suit;
        private readonly string _rank;

        public Card(string suit, string rank)
        {
            this._suit = suit;
            this._rank = rank;
        }

        public int RankValue()
        {
            if ("JQK".Contains(_rank))
            {
                return 10;
            }
            else if (_rank == "A")
            {
                return 1;
            }
            else
            {
                return int.Parse(_rank);
            }
        }

        public string Display()
        {
            string[] lines = new string[7];
            lines[0] = "┌─────────┐";
            lines[1] = $"│{_rank}{(_rank == "10" ? "" : " ")}       │";
            lines[2] = "│         │";
            lines[3] = $"│    {_suit}    │";
            lines[4] = "│         │";
            lines[5] = $"│       {_rank}{(_rank == "10" ? "" : " ")}│";
            lines[6] = "└─────────┘";

            ConsoleColor cardColor = "♥♦".Contains(_suit) ? ConsoleColor.Red : ConsoleColor.Black;
            Console.ForegroundColor = cardColor;
            Console.WriteLine($"Updated Foreground Color2: {Console.ForegroundColor}");
            return string.Join(Environment.NewLine, lines);
        }

        public override string ToString()
        {
            return $"Card {{ suit={_suit}, rank={_rank} }}";
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (GetType() != obj.GetType()) return false;

            Card card = (Card)obj;
            return _suit == card._suit && _rank == card._rank;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_suit, _rank);
        }
    }
}
