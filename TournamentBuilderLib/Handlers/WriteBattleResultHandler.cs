using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IWriteBattleResultHandler
{
    public void Execute(BattleResult battleResult);
}

public class WriteBattleResultHandler : BaseHandler, IWriteBattleResultHandler
{
    private readonly string _sheetName = GoogleSheet.Default.RESULT_SHEET_NAME;

    public WriteBattleResultHandler(string sheetId) : base(sheetId)
    {
    }

    public void Execute(BattleResult battleResult)
    {
        var objectList = new List<object>()
        {
            battleResult.Result,
            battleResult.Score
        };
        var rangeData = new List<IList<object>> { objectList };
        ExcelWriter.Write(_sheetId, $"{_sheetName}!{battleResult.Range}", rangeData);
    }
}
