using HEMACounter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TournamentBuilderLib.Handlers;
using TournamentBuilderLib.Models;
using TournamentBuilderLib.Utils;

namespace HEMACounter.ViewModels.Base
{
    public class BaseSwissViewModel<T> : AdvancedViewModel<T> where T : IParticipant
    {
        private ICommand generateStageNCommand;
        public ICommand GenerateStageNCommand => generateStageNCommand ??= new CommandHandler(GenerateStageN, () => true);

        public BaseSwissViewModel() : base()
        {
            _writeBattlePairHandler = new WriteBattlePairHandler(Settings.SheetId);
            _battleResultBuilder = new BattleResultBuilder();
            _writeBattleResultHandler = new WriteBattleResultHandler(Settings.SheetId);
            _getBattlePairsHandler = new GetBattlePairsHandler(Settings.SheetId);
            _getParticipantsScoreHandler = new GetParticipantsScoreHandler(Settings.SheetId);
            _getParticipantsHandler = new GetParticipantsHandler(Settings.SheetId);
        }

        public override void GenerateStages()
        {
            Stages.Clear();

            Enumerable.Range(1, Settings.StagesCount ?? 6).Select(x => new Stage()
            {
                Id = x,
                MaxScore = Settings.ScoresPerRound ?? 10,
                MaxDoubles = Settings.MaxDoubles ?? -1,
                Duration = Settings.RoundTime ?? TimeSpan.FromSeconds(120)
            }).ToList().ForEach(Stages.Add);
        }

        public override void ReloadStageN()
        {
            if (CurrentStage == null)
                return;

            var current = CurrentStage.Id;
            var currentPairs = _getBattlePairsHandler.Execute($"Круг {current}", participants.Count())
                .Where(x => !x.IsStarted || LoadAll).ToList();

            BattlePairs.Clear();
            currentPairs.ForEach(BattlePairs.Add);
        }

        protected void GenerateStageN()
        {
            var current = CurrentStage.Id;

            try
            {
                if (_getBattlePairsHandler.Execute($"Круг {current}", participants.Count())
                    .Where(x => x.IsStarted).Any())
                {
                    MessageBox.Show("Круг уже начался!");
                    return;
                }
            }
            catch { }

            var restrictedPairs = new List<BattlePair>();

            for (int turn = 1; turn < current; turn++)
            {
                restrictedPairs.AddRange(_getBattlePairsHandler.Execute($"Круг {turn}", participants.Count()));
            }

            var participantScores = _getParticipantsScoreHandler.Execute();

            var generatedPairs = PairGenerator.GenerateBattlePairs(GenerationMode.Swiss,
                restrictedPairs.ToList(), participantScores.ToList());

            int i = 1;
            foreach (var pair in generatedPairs)
            {
                pair.Range = $"Круг {current}!A{i}:G{i}";
                _writeBattlePairHandler.Execute(pair);
                i++;
            }

            BattlePairs.Clear();
            generatedPairs.ForEach(BattlePairs.Add);
        }
    }
}
