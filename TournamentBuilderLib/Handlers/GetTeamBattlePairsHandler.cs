using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IGetTeamBattlePairsHandler
{
    public IEnumerable<BattlePair> Execute(string sheetName, int participantsCount);
}

[Obsolete]
public class GetTeamBattlePairsHandler : BaseHandler, IGetTeamBattlePairsHandler
{
    public GetTeamBattlePairsHandler(string sheetId) : base(sheetId)
    {
    }

    public IEnumerable<BattlePair> Execute(string sheetName, int participantsCount)
    {
        var range = $"{sheetName}!A1:E{participantsCount/2}";

        var values = ExcelReader.Read(_sheetId, range);
        if (values == null)
            return new List<BattlePair>();

        var battlePairs = new List<BattlePair>();
        int i = 1;
        foreach (var value in values)
        {
            BattlePair item = new()
            {
                FighterRedName = value[0]?.ToString(),
                FighterRedScore = string.IsNullOrEmpty(value[1].ToString()) ? 0 : Convert.ToInt32(value[1]),
                FighterBlueScore = string.IsNullOrEmpty(value[2].ToString()) ? 0 : Convert.ToInt32(value[2]),
                FighterBlueName = value[3]?.ToString(),
                IsStarted = value[4]?.ToString() == "1",
                Range = $"{sheetName}!A{i}:E{i}",
            };
            battlePairs.Add(item);
            i++;
        }
        return battlePairs;
    }
}
