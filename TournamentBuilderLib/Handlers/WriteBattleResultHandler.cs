using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IWriteBattleResultHandler
{
    public void Execute(BattleResult battleResult);
}

public class WriteBattleResultHandler : IWriteBattleResultHandler
{
    private readonly string SHEET_ID = GoogleSheet.Default.SHEET_ID;
    private readonly string SHEET_NAME = GoogleSheet.Default.RESULT_SHEET_NAME;

    public void Execute(BattleResult battleResult)
    {
        var objectList = new List<object>()
        {
            battleResult.Result,
            battleResult.Score
        };
        var rangeData = new List<IList<object>> { objectList };
        ExcelWriter.Write(SHEET_ID, $"{SHEET_NAME}!{battleResult.Range}", rangeData);
    }
}
