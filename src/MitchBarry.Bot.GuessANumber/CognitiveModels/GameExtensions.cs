using System.Linq;

namespace MitchBarry.Bot.GuessANumber.CognitiveModels
{
    public partial class Game
    {
        public (int? minimumNumber, int? maximumNumber) NumberRangeEntities
        {
            get
            {
                var minimumValueString = Entities?.numberRange?.FirstOrDefault().minimumNumber?.FirstOrDefault();
                var maximumValueString = Entities?.numberRange?.FirstOrDefault().maximumNumber?.FirstOrDefault();
                int? minimumNumber = null;
                int? maximumNumber = null;
                if (int.TryParse(minimumValueString, out int min))
                {
                    minimumNumber = min;
                }
                if (int.TryParse(maximumValueString, out int max))
                {
                    maximumNumber = max;
                }
                return (minimumNumber, maximumNumber);
            }
        }
    }
}
