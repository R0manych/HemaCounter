using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers
{
    public class GetBattlePairsComedyHandler : GetBattlePairsBaseHandler, IGetBattlePairsHandler
    {
        public GetBattlePairsComedyHandler(string sheetId) : base(sheetId)
        {
        }

        public IEnumerable<BattlePair> Execute(string sheetName, int maxFights)
        {
            var range = $"{sheetName}!D1:J{maxFights}";
            var rangeTemplate = sheetName + @"!D{0}:J{1}";

            return base.Execute(range, rangeTemplate);
        }
    }
}
