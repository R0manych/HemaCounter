using ExcelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers
{
    public interface IWriteBattlePairsHandlerEggsTemplate
    {
        void Execute(BattlePair battlePair);
    }

    public class WriteBattlePairsHandlerEggsTemplate : BaseHandler, IWriteBattlePairsHandlerEggsTemplate
    {
        public WriteBattlePairsHandlerEggsTemplate(string sheetId) : base(sheetId)
        {
        }

        public void Execute(BattlePair battlePair)
        {
            var objectList = new List<object>()
            {
                battlePair.FighterRedScore
            };
            var rangeData = new List<IList<object>> { objectList };
            ExcelWriter.Write(_sheetId, battlePair.FighterRedRange, rangeData);
            objectList = new List<object>()
            {
                battlePair.FighterBlueScore
            };
            rangeData = new List<IList<object>> { objectList };
            ExcelWriter.Write(_sheetId, battlePair.FighterBlueRange, rangeData);
        }
    }
}
