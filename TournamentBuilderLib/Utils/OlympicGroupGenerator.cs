using HEMACounter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentBuilderLib.Handlers;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Utils
{
    public class OlympicGroupGenerator
    {
        private readonly IWriteMultipleBattlePairsHandler _writeMultipleBattlePairsHandler;

        public OlympicGroupGenerator(string sheetId)
        {
            _writeMultipleBattlePairsHandler = new WriteMultipleBattlePairsHandler(sheetId);
        }

        public void GenerateGroups(IEnumerable<IParticipant> participants)
        {
            var randomArr = new int[participants.Count()];
            for (var i = 0; i < participants.Count(); i++)
            {
                randomArr[i] = i;
            }
            randomArr.Shuffle();

            var leftParticipantsCount = participants.Count();
            var leftGroupsCount = Settings.StagesCount;

            for (var i = 0; i < Settings.StagesCount; i++)
            {
                var groupParticipnatsCount = leftParticipantsCount % leftGroupsCount > 0
                    ? leftParticipantsCount / leftGroupsCount + 1
                    : leftParticipantsCount / leftGroupsCount;
                var groupParticipantsIds = randomArr.Skip(participants.Count() - leftParticipantsCount)
                    .Take(groupParticipnatsCount.Value);
                leftParticipantsCount -= groupParticipnatsCount.Value;
                leftGroupsCount--;
                var groupParticipants = participants.Where(x => groupParticipantsIds.Contains(x.Id));
                GenerateGroup(i, groupParticipants);
            }
        }

        public void GenerateGroup(int groupNum, IEnumerable<IParticipant> groupParticipants)
        {
            var pairStr = 1;
            var battlePairs = new List<BattlePair>();
            for (var i = 0; i < groupParticipants.Count(); i++)
            {
                for (var j = i + 1; j < groupParticipants.Count(); j++)
                {
                    var battlePair = new BattlePair()
                    {
                        DoublesCount = 0,
                        FighterBlueName = groupParticipants.ElementAt(i).Name,
                        FighterBlueScore = 0,
                        FighterRedName = groupParticipants.ElementAt(j).Name,
                        FighterBlueRange = $"Группа {groupNum+1}!B{pairStr}:B{pairStr}",
                        FighterRedRange = $"Группа {groupNum+1}!C{pairStr}:C{pairStr}",
                        FighterRedScore = 0,
                        IsStarted = false,
                        TimeInSeconds = 0,
                        Range = $"Группа {groupNum+1}!A{pairStr}:G{pairStr}",
                    };
                    battlePairs.Add(battlePair);
                    pairStr++;
                }
            }
            _writeMultipleBattlePairsHandler.Execute(battlePairs, $"Группа {groupNum + 1}!A{1}:G{pairStr}");
        }
    }
}
