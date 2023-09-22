using ExcelLib;
using TournamentBuilderLib.Participants.Models;

namespace TournamentBuilderLib.Participants.Handlers;

public interface IGetBattlePairsHandler
{
    public IEnumerable<BattlePair> Execute(string sheetName);
}

public class GetBattlePairsHandler : IGetBattlePairsHandler
{
    private const string SHEET_ID = "1Q7oySMjF3tiB-dlPkiIur9dyyqyGQz7qd3SKtarqS2Q";

    public IEnumerable<BattlePair> Execute(string sheetName)
    {
        var range = $"{sheetName}!D1:G18";
        var values = ExcelReader.Read(SHEET_ID, range);
        var battlePairs = new List<BattlePair>();
        int i = 1;
        foreach (var value in values)
        {
            BattlePair item = new()
            {
                FighterRedName = value[0]?.ToString(),
                FighterRedNameAddress = $"D{i}",
                FighterRedScore = string.IsNullOrEmpty(value[1].ToString()) ? 0 : Convert.ToInt32(value[1]),
                FighterRedScoreAddress = $"E{i}",
                FighterBlueScore = string.IsNullOrEmpty(value[2].ToString()) ? 0 : Convert.ToInt32(value[2]),
                FighterBlueScoreAddress = $"F{i}",
                FighterBlueName = value[3]?.ToString(),
                FighterBlueNameAddress = $"G{i}",
                Range = $"{sheetName}!D{i}:G{i}",
            };
            battlePairs.Add(item);
            i++;
        }
        return battlePairs;
    }
}
