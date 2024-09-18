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
        void Execute(List<BattlePair> battlePairs);
    }

    public class WriteMultipleBattlePairsHandler : BaseHandler, IWriteMultipleBattlePairsHandler
    {
        public WriteMultipleBattlePairsHandler(string sheetId) : base(sheetId)
        {
        }

        public void Execute(List<BattlePair> battlePairs)
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
            ExcelWriter.Write(_sheetId, GetRange(battlePairs), rangeData);
        }

        private string GetRange(List<BattlePair> battlePairs) 
        {
            return $"A1:G{battlePairs.Count + 1}";
        }
    }
}
