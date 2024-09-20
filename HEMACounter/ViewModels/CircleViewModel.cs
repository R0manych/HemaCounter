using HEMACounter.Models;
using HEMACounter.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using TournamentBuilderLib.Handlers;
using TournamentBuilderLib.Models;
using TournamentBuilderLib.Utils;

namespace HEMACounter.ViewModels
{
    public class CircleViewModel : AdvancedViewModel<ParticipantWithClub>
    {
        private ICommand generateStageNCommand;
        public ICommand GenerateStageNCommand => generateStageNCommand ??= new CommandHandler(GenerateStageN, () => true);

        public CircleViewModel() : base()
        {
            Initialize();
        }

        private void Initialize()
        {
            IsCovered = true;
            elapsedTime = new TimeSpan();
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 1000;
            Time = elapsedTime.ToString(@"mm\:ss");
            StartButtonText = timer.Enabled ? "Стоп" : "Старт";

            _getParticipantsHandler = new GetParticipantsHandler(Settings.SheetId);
            _getBattlePairsHandler = new GetBattlePairsHandler(Settings.SheetId);
            _writeBattlePairHandler = new WriteBattlePairHandler(Settings.SheetId);
            _battleResultBuilder = new BattleResultBuilder();
            _writeBattleResultHandler = new WriteBattleResultHandler(Settings.SheetId);
            participants = _getParticipantsHandler.Execute();

            GenerateStages();
            CurrentStage = Stages.First();
        }

        public override void ReloadStageN()
        {
            if (CurrentStage == null)
                return;

            var current = CurrentStage.Id;
            var currentPairs = _getBattlePairsHandler.Execute($"Группа {current}", participants.Count() / Settings.StagesCount.Value)
                .Where(x => !x.IsStarted || LoadAll).ToList();

            BattlePairs.Clear();
            currentPairs.ForEach(BattlePairs.Add);
        }

        public void GenerateStageN()
        {
            var groupGenerator = new OlympicGroupGenerator(Settings.SheetId);
            groupGenerator.GenerateGroups(participants);
        }
    }
}
