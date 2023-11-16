using HEMACounter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TournamentBuilderLib.Handlers;
using TournamentBuilderLib.Models;
using TournamentBuilderLib.Utils;

namespace HEMACounter.ViewModels.Base
{
    public class BaseSwissViewModel<T> : BaseViewModel<T> where T : IParticipant
    {
        protected readonly IWriteBattlePairHandler _writeBattlePairHandler = new WriteBattlePairHandler(Settings.SheetId);
        protected readonly IBattleResultBuilder _battleResultBuilder = new BattleResultBuilder();
        protected readonly IWriteBattleResultHandler _writeBattleResultHandler = new WriteBattleResultHandler(Settings.SheetId);
        protected readonly IGetBattlePairsHandler _getBattlePairsHandler = new GetBattlePairsHandler(Settings.SheetId);
        protected readonly IGetParticipantsScoreHandler _getParticipantsScoreHandler = new GetParticipantsScoreHandler(Settings.SheetId);
        protected readonly IGetParticipantsHandler _getParticipantsHandler = new GetParticipantsHandler(Settings.SheetId);

        public override void FinishFight()
        {
            SetupCurrentBattlePairScore();

            //Запись в файл текущего круга
            _writeBattlePairHandler.Execute(CurrentBattlePair);

            if (Doubles > CurrentStage.MaxDoubles && Settings.TechDefeatByDoublesEnabled) //техническое поражение обоим
            {
                var (resultRed, resultBlue) = _battleResultBuilder.BuildTechnicalDefeat(CurrentBattlePair, participants.Cast<IParticipant>(), CurrentStage.Id, Doubles > CurrentStage.MaxDoubles);
                _writeBattleResultHandler.Execute(resultRed);
                _writeBattleResultHandler.Execute(resultBlue);
            }
            else if (CurrentBattlePair.IsDraw)
            {
                var (resultRed, resultBlue) = _battleResultBuilder.BuildDraws(CurrentBattlePair, participants.Cast<IParticipant>(), CurrentStage.Id, Doubles > CurrentStage.MaxDoubles);
                _writeBattleResultHandler.Execute(resultRed);
                _writeBattleResultHandler.Execute(resultBlue);
            }
            else
            {
                var winnerResult = _battleResultBuilder.BuildWinner(CurrentBattlePair, participants.Cast<IParticipant>(), CurrentStage.Id, Doubles > CurrentStage.MaxDoubles);
                var loserResult = _battleResultBuilder.BuildLoser(CurrentBattlePair, participants.Cast<IParticipant>(), CurrentStage.Id, Doubles > CurrentStage.MaxDoubles);
                _writeBattleResultHandler.Execute(winnerResult);
                _writeBattleResultHandler.Execute(loserResult);
            }
        }

        private void SetupCurrentBattlePairScore()
        {
            CurrentBattlePair!.FighterRedScore = RedScore;
            CurrentBattlePair!.FighterBlueScore = BlueScore;
            CurrentBattlePair!.TimeInSeconds = (int)elapsedTime.TotalSeconds;
            CurrentBattlePair!.DoublesCount = Doubles;
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
            if (CurrentStage == null)
                return;

            var current = CurrentStage.Id;
            var currentPairs = _getBattlePairsHandler.Execute($"Круг {current}", participants.Count())
                .Where(x => !x.IsStarted || LoadAll).ToList();

            BattlePairs.Clear();
            currentPairs.ForEach(BattlePairs.Add);
        }

        protected override void GenerateStageN()
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
                pair.Range = $"Круг {current}!A{i}:E{i}";
                _writeBattlePairHandler.Execute(pair);
                i++;
            }

            BattlePairs.Clear();
            generatedPairs.ForEach(BattlePairs.Add);
        }


        public override void ReloadParticipants()
        {
            throw new NotImplementedException();
        }

        public override void OnStartTimer()
        {
            if (CurrentBattlePair is not null)
            {
                SetupCurrentBattlePairScore();
                _writeBattlePairHandler.Execute(CurrentBattlePair);
            }
        }
    }
}
