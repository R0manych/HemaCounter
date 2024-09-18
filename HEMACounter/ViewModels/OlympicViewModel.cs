using HEMACounter.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using TournamentBuilderLib.Models;
using TournamentBuilderLib.Utils;

namespace HEMACounter.ViewModels
{
    public class OlympicViewModel : AdvancedViewModel<ParticipantWithClub>
    { 
        public OlympicViewModel() : base()
        { 
        }

        public override void GenerateStages()
        {
            var randomArr = new int[participants.Count()];
            randomArr.Shuffle();

            var leftParticipantsCount = participants.Count();

            for (var i = 0; i< Settings.StagesCount; i++)
            {
                var groupParticipnatsCount = leftParticipantsCount % Settings.StagesCount > 0
                    ? leftParticipantsCount / Settings.StagesCount + 1
                    : leftParticipantsCount / Settings.StagesCount;
                var groupParticipantsIds = randomArr.Skip(participants.Count() - leftParticipantsCount)
                    .Take(groupParticipnatsCount.Value);
                leftParticipantsCount -= groupParticipnatsCount.Value;
                GenerateGroupN(i, groupParticipantsIds);
            }
        }

        public override void ReloadStageN()
        {
            //Для олимпийки не нужно перезагружать раунд
        }

        private void GenerateGroupN(int groupId, IEnumerable<int> participantIds)
        {

        }
    }
}
