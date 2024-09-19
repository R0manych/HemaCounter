using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers
{
    public class GetBattlePairsBaseHandler : BaseHandler
    {
        public GetBattlePairsBaseHandler(string sheetId) : base(sheetId)
        {
        }

        public IEnumerable<BattlePair> Execute(string range, string rangeTemplate)
        {
            var values = ExcelReader.Read(_sheetId, range);
            if (values == null)
                return new List<BattlePair>();

            var battlePairs = new List<BattlePair>();
            int i = 1;
            foreach (var value in values)
            {
                var test = string.Format(rangeTemplate, i, i);
                BattlePair item = new()
                {
                    FighterRedName = value[0]?.ToString(),
                    FighterRedScore = string.IsNullOrEmpty(value[1].ToString()) ? 0 : Convert.ToInt32(value[1]),
                    FighterBlueScore = string.IsNullOrEmpty(value[2].ToString()) ? 0 : Convert.ToInt32(value[2]),
                    FighterBlueName = value[3]?.ToString(),
                    IsStarted = value[4]?.ToString() == "1",
                    DoublesCount = string.IsNullOrEmpty(value[5].ToString()) ? 0 : Convert.ToInt32(value[5]),
                    TimeInSeconds = string.IsNullOrEmpty(value[6].ToString()) ? 0 : Convert.ToInt32(value[6]),
                    Range = string.Format(rangeTemplate, i, i),
                };
                battlePairs.Add(item);
                i++;
            }
            return battlePairs;
        }
    }
}
