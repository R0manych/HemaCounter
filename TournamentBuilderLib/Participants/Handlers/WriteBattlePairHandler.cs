using ExcelLib;
using TournamentBuilderLib.Participants.Models;

namespace TournamentBuilderLib.Participants.Handlers;

public interface IWriteBattlePairHandler
{
    void Execute(BattlePair battlePair);
}

public class WriteBattlePairHandler : IWriteBattlePairHandler
{
    private const string SHEET_ID = "1Q7oySMjF3tiB-dlPkiIur9dyyqyGQz7qd3SKtarqS2Q";

    public void Execute(BattlePair battlePair)
    {
        var objectList = new List<object>()
        { 
            battlePair.FighterRedName, 
            battlePair.FighterRedScore,
            battlePair.FighterBlueScore,
            battlePair.FighterBlueName 
        };
        var rangeData = new List<IList<object>> { objectList };
        ExcelWriter.Write(SHEET_ID, battlePair.Range, rangeData);
    }
}
