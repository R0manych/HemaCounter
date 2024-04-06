using HEMACounter.Models;
using HEMACounter.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using TournamentBuilderLib.Handlers;
using TournamentBuilderLib.Models;

namespace HEMACounter.ViewModels
{
    internal class EggsViewModel : BaseViewModel<ParticipantWithClub>
    {
        private string _blueImage;

        public string BlueImage
        {
            get => _blueImage;
            set 
            {
                _blueImage = value;
                if (propertyChanged != null)
                {
                    propertyChanged(this, new PropertyChangedEventArgs("BlueImage"));
                }
            }
        }

        private string _redImage;

        public string RedImage
        {
            get => _redImage;
            set
            {
                _redImage = value;
                if (propertyChanged != null)
                {
                    propertyChanged(this, new PropertyChangedEventArgs("RedImage"));
                }
            }
        }

        private readonly GetBattlePairsForEggsTemplateService _getBattlePairsHandler =
            new GetBattlePairsForEggsTemplateService(Settings.SheetId);

        private readonly GetBattlePairsForEggsTemplate8Service _getBattlePairsForEggsTemplate8Service = 
            new GetBattlePairsForEggsTemplate8Service(Settings.SheetId);

        private readonly IWriteBattlePairsHandlerEggsTemplate _writeBattlePairsHandlerEggsTemplate =
            new WriteBattlePairsHandlerEggsTemplate(Settings.SheetId);

        private readonly IGetParticipantsHandler _getParticipantsHandler = new GetParticipantsHandler(Settings.SheetId);

        public EggsViewModel()
        {
            Initialize();
        }

        public void Initialize()
        {
            IsCovered = true;
            elapsedTime = new TimeSpan();
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 1000;
            Time = elapsedTime.ToString(@"mm\:ss");
            StartButtonText = timer.Enabled ? "Стоп" : "Старт";
            GenerateStages();
            CurrentStage = Stages.First();
            participants = _getParticipantsHandler.Execute();
        }

        /*public void SetRound(int roundIndex)
        {
            if (roundIndex >= Rounds.Count || roundIndex < 0)
                return;

            elapsedTime = new TimeSpan();
            timer.Stop();
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 1000;
            Time = elapsedTime.ToString(@"mm\:ss");

            foreach (var round in Rounds)
                round.IsCurrent = false;

            Rounds[roundIndex].IsCurrent = true;

            CurrentBlueFighter = Rounds[roundIndex].BlueFighter;
            CurrentRedFighter = Rounds[roundIndex].RedFighter;

            if (roundIndex + 1 < Rounds.Count)
            {
                NextBlueFighter = Rounds[roundIndex + 1].BlueFighter;
                NextRedFighter = Rounds[roundIndex + 1].RedFighter;
            }
            else
            {
                NextBlueFighter = "";
                NextRedFighter = "";
            }

            StartButtonText = timer.Enabled ? "Стоп" : "Старт";
        }*/

        public override void GetReady()
        {
            NextBattlePair = SelectedBattlePair;
            SelectedBattlePair = null;

            if (NextBattlePair is null)
                return;

            if (NextBattlePair == null)
                return;

            NextRedFighter = NextBattlePair.FighterRedName;
            NextBlueFighter = NextBattlePair.FighterBlueName;
        }

        public override void ReloadStageN()
        {
            if (CurrentStage == null)
                return;

            var current = CurrentStage.Id;
            var currentPairs = new List<BattlePair>();
            if (participants.Count() > 8)
            {
                switch (current)
                {
                    case 1:
                        currentPairs = _getBattlePairsHandler.GetBattlePairsForFightRound1($"Круг текущий", 28).ToList();
                        break;
                    case 2:
                        currentPairs = _getBattlePairsHandler.GetBattlePairsForFightRound2($"Круг текущий", 28).ToList();
                        break;
                    case 3:
                        currentPairs = _getBattlePairsHandler.GetBattlePairsForFightRound3($"Круг текущий", 28).ToList();
                        break;
                    case 4:
                        currentPairs = _getBattlePairsHandler.GetBattlePairsForFightRound4($"Круг текущий", 28).ToList();
                        break;
                    case 5:
                        currentPairs = _getBattlePairsHandler.GetBattlePairsForFightRound5($"Круг текущий", 28).ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (current)
                {
                    case 1:
                        currentPairs = _getBattlePairsForEggsTemplate8Service.GetBattlePairsForFightRound1($"Круг текущий", 8).ToList();
                        break;
                    case 2:
                        currentPairs = _getBattlePairsForEggsTemplate8Service.GetBattlePairsForFightRound2($"Круг текущий", 8).ToList();
                        break;
                    case 3:
                        currentPairs = _getBattlePairsForEggsTemplate8Service.GetBattlePairsForFightRound3($"Круг текущий", 8).ToList();
                        break;
                    case 4:
                        currentPairs = _getBattlePairsForEggsTemplate8Service.GetBattlePairsForFightRound4($"Круг текущий", 8).ToList();
                        break;
                    default:
                        break;
                }
            }

            BattlePairs.Clear();
            currentPairs.ForEach(BattlePairs.Add);
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

        public override void FinishFight()
        {
            if (CurrentBattlePair is not null)
            {
                CurrentBattlePair.FighterRedScore = RedScore;
                CurrentBattlePair.FighterBlueScore = BlueScore;
                //Запись в файл текущего круга
                _writeBattlePairsHandlerEggsTemplate.Execute(CurrentBattlePair);
            }
        }

        public override void ReloadParticipants()
        {
            //participants = _getParticipantsHandler.Execute();
            ReloadStageN();

            ResetNextFighters();
        }

        private void ResetNextFighters()
        {
            NextBattlePair = null;
            NextRedFighter = null;
            NextBlueFighter = null;
        }

        public override void OnBlueScoreUpdate()
        {
            BlueImage = $"/HEMACounter;component/Images/blue{BlueScore}.png";
        }

        public override void OnRedScoreUpdate()
        {
            RedImage = $"/HEMACounter;component/Images/red{RedScore}.png";
        }

        protected override void GenerateStageN()
        {
            return;//Пока не нужна логика генерации кругов.
        }

        public override void OnStartTimer()
        {
            return;//Пустое действие, здесь пока не нужна эта логика.
        }
    }
}
