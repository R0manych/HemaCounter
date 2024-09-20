using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IGetBattlePairsHandler
{
    public IEnumerable<BattlePair> Execute(string sheetName, int maxFights);
}

public class GetBattlePairsHandler : GetBattlePairsBaseHandler, IGetBattlePairsHandler
{
    public GetBattlePairsHandler(string sheetId) : base(sheetId)
    {
    }

    public IEnumerable<BattlePair> Execute(string sheetName, int maxFights)
    {
        var range = $"{sheetName}!A1:G{maxFights}";
        var rangeTemplate = sheetName + @"!A{0}:G{1}";

        return base.Execute(range, rangeTemplate);
    }
}
