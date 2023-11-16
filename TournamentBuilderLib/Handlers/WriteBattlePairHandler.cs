using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IWriteBattlePairHandler
{
    void Execute(BattlePair battlePair);
}

public class WriteBattlePairHandler : BaseHandler, IWriteBattlePairHandler
{
    public WriteBattlePairHandler(string sheetId) : base(sheetId)
    {
    }

    public void Execute(BattlePair battlePair)
    {
        var objectList = new List<object>()
        {
            battlePair.FighterRedName,
            battlePair.FighterRedScore,
            battlePair.FighterBlueScore,
            battlePair.FighterBlueName,
            battlePair.IsStarted ? 1 : 0,
            battlePair.DoublesCount,
            battlePair.TimeInSeconds
        };
        var rangeData = new List<IList<object>> { objectList };
        ExcelWriter.Write(_sheetId, battlePair.Range, rangeData);
    }
}
