using HEMACounter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TournamentBuilderLib.Builders;
using TournamentBuilderLib.Handlers;
using TournamentBuilderLib.Models;

namespace HEMACounter.ViewModels.Base
{
    public abstract class AdvancedViewModel<T> : BaseViewModel where T : IParticipant
    {
        protected IWriteBattlePairHandler _writeBattlePairHandler;
        protected IBattleResultBuilder _battleResultBuilder;
        protected IWriteBattleResultHandler _writeBattleResultHandler;
        protected IGetBattlePairsHandler _getBattlePairsHandler;
        protected IGetParticipantsScoreHandler _getParticipantsScoreHandler;
        protected IGetParticipantsHandler _getParticipantsHandler;

        protected IEnumerable<T> participants = new List<T>();

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

        public override void OnStartTimer()
        {
            if (CurrentBattlePair is not null)
            {
                SetupCurrentBattlePairScore();
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    try
                    {
                        _writeBattlePairHandler.Execute(CurrentBattlePair);
                    }
                    catch { }
                }).Start();
            }
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
    }
}

