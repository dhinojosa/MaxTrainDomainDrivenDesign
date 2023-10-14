using System.Text;
using Ansi;

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

            var modeColor = "♥♦".Contains(_suit) ? Mode.ForegroundRed : Mode.ForegroundBlack;
            var separator = new StringBuilder().Down(1).Left(11).ToString();
            return new StringBuilder().SetMode(modeColor) + string.Join(separator, lines);
        }

        public override string ToString()
        {
            return $"Card {{ suit={_suit}, rank={_rank} }}";
        }

        public override bool Equals(object? obj)
        {
            if (this == obj) return true;
            if (obj != null && GetType() != obj.GetType()) return false;

            var otherCard = obj as Card;
            return otherCard != null && _suit == otherCard._suit && _rank == otherCard._rank;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_suit, _rank);
        }
    }
}
