using HEMACounter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using TournamentBuilderLib.Handlers;
using TournamentBuilderLib.Models;
using TournamentBuilderLib.Utils;

namespace HEMACounter.ViewModels.Base
{
    public class BaseSwissViewModel<T> : BaseViewModel<T> where T : IParticipant
    {
        protected readonly IWriteBattlePairHandler _writeBattlePairHandler = new WriteBattlePairHandler(Settings.CurrentSheetId);
        protected readonly IBattleResultBuilder _battleResultBuilder = new BattleResultBuilder();
        protected readonly IWriteBattleResultHandler _writeBattleResultHandler = new WriteBattleResultHandler(Settings.CurrentSheetId);
        protected readonly IGetBattlePairsHandler _getBattlePairsHandler = new GetBattlePairsHandler(Settings.CurrentSheetId);
        protected readonly IGetParticipantsScoreHandler _getParticipantsScoreHandler = new GetParticipantsScoreHandler(Settings.CurrentSheetId);
        protected readonly IGetParticipantsHandler _getParticipantsHandler = new GetParticipantsHandler(Settings.CurrentSheetId);

        public override void FinishFight()
        {
            CurrentBattlePair!.FighterRedScore = RedScore;
            CurrentBattlePair!.FighterBlueScore = BlueScore;

            //Запись в файл текущего круга
            _writeBattlePairHandler.Execute(CurrentBattlePair);

            if (Doubles > CurrentStage.MaxDoubles) //техническое поражение обоим
            {
                var (resultRed, resultBlue) = _battleResultBuilder.BuildTechnicalDefeat(CurrentBattlePair, participants.Cast<IParticipant>(), CurrentStage.Id);
                _writeBattleResultHandler.Execute(resultRed);
                _writeBattleResultHandler.Execute(resultBlue);
            }
            else if (CurrentBattlePair.IsDraw)
            {
                var (resultRed, resultBlue) = _battleResultBuilder.BuildDraws(CurrentBattlePair, participants.Cast<IParticipant>(), CurrentStage.Id);
                _writeBattleResultHandler.Execute(resultRed);
                _writeBattleResultHandler.Execute(resultBlue);
            }
            else
            {
                var winnerResult = _battleResultBuilder.BuildWinner(CurrentBattlePair, participants.Cast<IParticipant>(), CurrentStage.Id);
                var loserResult = _battleResultBuilder.BuildLoser(CurrentBattlePair, participants.Cast<IParticipant>(), CurrentStage.Id);
                _writeBattleResultHandler.Execute(winnerResult);
                _writeBattleResultHandler.Execute(loserResult);
            }
        }

        public override void GenerateStages()
        {
            Stages.Clear();
            //TODO: вынести это число в конфиг.
            Enumerable.Range(1, 5).Select(x => new Stage()
            {
                Id = x,
                MaxScore = 7 * x,
                MaxDoubles = x * 2,
                Duration = TimeSpan.FromSeconds(60)
            }).ToList().ForEach(Stages.Add);
        }

        public override void GetReady()
        {
            if (NextBattlePair is not null)
            {
                NextBattlePair.IsStarted = false;
                _writeBattlePairHandler.Execute(NextBattlePair);
            }

            NextBattlePair = SelectedBattlePair;
            SelectedBattlePair = null;

            if (NextBattlePair is null)
                return;

            NextBattlePair.IsStarted = true;
            _writeBattlePairHandler.Execute(NextBattlePair);
        }

        public override void ReloadStageN()
        {
            var current = CurrentStage.Id;
            var currentPairs = _getBattlePairsHandler.Execute($"Круг {current}", participants.Count())
                .Where(x => !x.IsStarted || LoadAll).ToList();

            BattlePairs.Clear();
            currentPairs.ForEach(BattlePairs.Add);
        }

        protected override void GenerateStageN()
        {
            var current = CurrentStage.Id;

            if (_getBattlePairsHandler.Execute($"Круг {current}", participants.Count())
                .Where(x => x.IsStarted).Any())
            {
                MessageBox.Show("Круг уже начался!");
                return;
            }

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
                pair.Range = $"Круг {current}!A{i}:E{i}";
                _writeBattlePairHandler.Execute(pair);
                i++;
            }

            BattlePairs.Clear();
            generatedPairs.ForEach(BattlePairs.Add);
        }
    }
}
