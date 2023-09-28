using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IWriteBattlePairHandler
{
    void Execute(BattlePair battlePair);
}

public class WriteBattlePairHandler : IWriteBattlePairHandler
{
    private readonly string SHEET_ID = GoogleSheet.Default.SHEET_ID;

    public void Execute(BattlePair battlePair)
    {
        var objectList = new List<object>()
        {
            battlePair.FighterRedName,
            battlePair.FighterRedScore,
            battlePair.FighterBlueScore,
            battlePair.FighterBlueName,
            battlePair.IsStarted ? 1 : 0
        };
        var rangeData = new List<IList<object>> { objectList };
        ExcelWriter.Write(SHEET_ID, battlePair.Range, rangeData);
    }
}
