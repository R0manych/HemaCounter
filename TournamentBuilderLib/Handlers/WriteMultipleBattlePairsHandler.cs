using ExcelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers
{
    public interface IWriteMultipleBattlePairsHandler
    {
        void Execute(List<BattlePair> battlePairs, string range);
    }

    public class WriteMultipleBattlePairsHandler : BaseHandler, IWriteMultipleBattlePairsHandler
    {
        public WriteMultipleBattlePairsHandler(string sheetId) : base(sheetId)
        {
        }

        public void Execute(List<BattlePair> battlePairs, string range)
        {
            var rangeData = new List<IList<object>>();
            foreach (var battlePair in battlePairs)
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

                rangeData.Add(objectList);
            }
            ExcelWriter.Write(_sheetId, range, rangeData);
        }
    }
}
