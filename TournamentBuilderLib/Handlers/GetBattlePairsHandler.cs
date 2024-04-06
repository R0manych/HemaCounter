using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IGetBattlePairsHandler
{
    public IEnumerable<BattlePair> Execute(string sheetName, int participantsCount);
}

public class GetBattlePairsHandler : GetBattlePairsBaseHandler, IGetBattlePairsHandler
{
    public GetBattlePairsHandler(string sheetId) : base(sheetId)
    {
    }

    public IEnumerable<BattlePair> Execute(string sheetName, int participantsCount)
    {
        var range = $"{sheetName}!A1:G{participantsCount / 2}";
        var rangeTemplate = $"{sheetName}!A{0}:G{1}";

        return base.Execute(range, rangeTemplate);
    }
}
