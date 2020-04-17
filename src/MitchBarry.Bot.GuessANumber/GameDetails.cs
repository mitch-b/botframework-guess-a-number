using System.Collections.Generic;

namespace MitchBarry.Bot.GuessANumber
{
    public class GameDetails
    {
        public int? MinimumNumber { get; set; }

        public int? MaximumNumber { get; set; }

        public int? ChosenNumber { get; set; }

        public List<int> Guesses { get; set; } = new List<int>();
    }
}
