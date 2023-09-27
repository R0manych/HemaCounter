using HEMACounter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using TournamentBuilderLib.Handlers;
using TournamentBuilderLib.Models;
using TournamentBuilderLib.Utils;

namespace HEMACounter.ViewModels
{
    internal class IndividualViewModel : INotifyPropertyChanged
    {
        #region Parameters

        private IEnumerable<BattlePair> battlePairs = new List<BattlePair>();
        private IEnumerable<IParticipant> Participants = new List<ParticipantWithClub>();

        private ObservableCollection<string> fighters = new ObservableCollection<string>();
        public ObservableCollection<string> Fighters
        {
            get => fighters;
            set
            {
                fighters = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("Fighters"));
            }
        }

        private string startButtonText;
        public string StartButtonText
        {
            get => startButtonText;
            set
            {
                startButtonText = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("StartButtonText"));
            }
        }

        //TODO: сделать селектор на интерфейсе
        private BattlePair selectedBattlePair;

        public BattlePair SelectedBattlePair
        {
            get => selectedBattlePair;
            set
            {
                selectedBattlePair = value;
            }
        }

        private BattlePair currentBattlePair;

        public BattlePair CurrentBattlePair
        {
            get => currentBattlePair;
            set
            {
                currentBattlePair = value;
                CurrentRedFighter = value.FighterRedName;
                CurrentBlueFighter = value.FighterBlueName;
            }
        }

        private string currentRedFighter;

        public string CurrentRedFighter
        {
            get { return currentRedFighter; }
            set
            {
                currentRedFighter = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs(nameof(CurrentRedFighter)));
            }
        }

        private string currentBlueFighter;

        public string CurrentBlueFighter
        {
            get { return currentBlueFighter; }
            set
            {
                currentBlueFighter = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs(nameof(CurrentBlueFighter)));
            }
        }

        private BattlePair nextBattlePair;

        public BattlePair NextBattlePair
        {
            get => nextBattlePair;
            set
            {
                nextBattlePair = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("NextBattlePair.FighterBlueName"));
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("NextBattlePair.FighterRedName"));
            }
        }

        private bool isCovered;
        public bool IsCovered
        {
            get => isCovered;
            set
            {
                isCovered = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("IsCovered"));
            }
        }

        private int blueScore;
        public int BlueScore
        {
            get => blueScore;
            set
            {
                blueScore = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("BlueScore"));
            }
        }

        private int redScore;
        public int RedScore
        {
            get => redScore;
            set
            {
                redScore = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("RedScore"));
            }
        }

        private string time;
        public string Time
        {
            get => time;
            set
            {
                time = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("Time"));
            }
        }

        private int doubles;
        public int Doubles
        {
            get => doubles;
            set
            {
                doubles = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("Doubles"));
            }
        }

        private Stage currentStage;
        public Stage CurrentStage
        {
            get => currentStage;
            set
            {
                currentStage = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("CurrentStage"));
            }
        }

        private ObservableCollection<Stage> stages = new ObservableCollection<Stage>();
        public ObservableCollection<Stage> Stages
        {
            get => stages;
            set
            {
                stages = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("Stages"));
            }
        }


        //TODO: заполнять где-то
        //При смене - загрузить пары с нового круга.
        private int _currentTurn;

        //TODO: Список из кругов: номер круга, время на круга, макс баллов.

        private PropertyChangedEventHandler? propertyChanged;
        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        #endregion

        #region Timer
        private static System.Timers.Timer timer;
        private TimeSpan elapsedTime;
        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
            Time = elapsedTime.ToString(@"mm\:ss");
        }

        #endregion

        #region Commands

        private ICommand startStopCommand;
        public ICommand StartStopCommand
        {
            get
            {
                return startStopCommand ?? (startStopCommand = new CommandHandler(() => StartStopTimer(), () => { return true; })); ;
            }
        }

        private ICommand newFightCommand;
        public ICommand NewFightCommand
        {
            get
            {
                return newFightCommand ?? (newFightCommand = new CommandHandler(() => NewFight(), () => { return true; })); ;
            }
        }

        private ICommand plusOneBlueCommand;
        public ICommand PlusOneBlueCommand
        {
            get
            {
                return plusOneBlueCommand ?? (plusOneBlueCommand = new CommandHandler(() => PlusOneBlue(), () => { return true; })); ;
            }
        }
        private ICommand minusOneBlueCommand;
        public ICommand MinusOneBlueCommand
        {
            get
            {
                return minusOneBlueCommand ?? (minusOneBlueCommand = new CommandHandler(() => MinusOneBlue(), () => { return true; })); ;
            }
        }


        private ICommand plusOneRedCommand;
        public ICommand PlusOneRedCommand
        {
            get
            {
                return plusOneRedCommand ?? (plusOneRedCommand = new CommandHandler(() => PlusOneRed(), () => { return true; })); ;
            }
        }
        private ICommand minusOneRedCommand;
        public ICommand MinusOneRedCommand
        {
            get
            {
                return minusOneRedCommand ?? (minusOneRedCommand = new CommandHandler(() => MinusOneRed(), () => { return true; })); ;
            }
        }

        private ICommand plusOneDoubleCommand;
        public ICommand PlusOneDoubleCommand
        {
            get
            {
                return plusOneDoubleCommand ?? (plusOneDoubleCommand = new CommandHandler(() => PlusOneDouble(), () => { return true; })); ;
            }
        }

        private ICommand minusOneDoubleCommand;
        public ICommand MinusOneDoubleCommand
        {
            get
            {
                return minusOneDoubleCommand ?? (minusOneDoubleCommand = new CommandHandler(() => MinusOneDouble(), () => { return true; })); ;
            }
        }

        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return cancelCommand ?? (cancelCommand = new CommandHandler(() => Cancel(), () => { return true; })); ;
            }
        }

        private ICommand coverCommand;
        public ICommand CoverCommand
        {
            get
            {
                return coverCommand ?? (coverCommand = new CommandHandler(() => Cover(), () => { return true; })); ;
            }
        }

        private ICommand getReadyCommand;
        public ICommand GetReadyCommand
        {
            get
            {
                return getReadyCommand ?? (getReadyCommand = new CommandHandler(() => GetReady(), () => { return true; })); ;
            }
        }

        private ICommand startStageNCommand;
        public ICommand StartStageNCommand
        {
            get
            {
                return startStageNCommand ?? (startStageNCommand = new CommandHandler(() => StartStageN(), () => { return true; })); ;
            }
        }

        #endregion

        private readonly IGetBattlePairsHandler _getBattlePairsHandler = new GetBattlePairsHandler();
        private readonly IGetParticipantsHandler _getParticipantsHandler = new GetParticipantsHandler();
        private readonly IWriteBattlePairHandler _writeBattlePairHandler = new WriteBattlePairHandler();
        private readonly IBattleResultBuilder _battleResultBuilder = new BattleResultBuilder();
        private readonly IWriteBattleResultHandler _writeBattleResultHandler = new WriteBattleResultHandler();
        private readonly IGetParticipantsScoreHandler _getParticipantsScoreHandler = new GetParticipantsScoreHandler();



        public IndividualViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            _currentTurn = 1;
            battlePairs = _getBattlePairsHandler.Execute($"Круг {_currentTurn}");
            Participants = _getParticipantsHandler.Execute();
            Fighters = new ObservableCollection<string>(Participants.Select(p => p.Name));
            IsCovered = true;
            elapsedTime = new TimeSpan();
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 1000;
            Time = elapsedTime.ToString(@"mm\:ss");
            StartButtonText = timer.Enabled ? "Стоп" : "Старт";

            GenerateDanteStages();

            StartStageN();

            //TODO: убрать
            nextBattlePair = battlePairs.First();
            selectedBattlePair = battlePairs.Skip(1).Take(1).First();
        }

        private void GenerateDanteStages()
        {
            Stages.Clear();
            Enumerable.Range(1, 9).Select(x => new Stage()
            {
                Id = x,
                MaxScore = 12 - x,
                MaxDoubles = (9 - x) / 3,
                Duration = TimeSpan.FromSeconds(60 + (9-x)*5)
            }).ToList().ForEach(Stages.Add);
        }

        public void StartStopTimer()
        {
            if (timer.Enabled)
                timer.Stop();
            else
                timer.Start();

            StartButtonText = timer.Enabled ? "Стоп" : "Старт";
        }

        public void NewFight()
        {
            if (MessageBox.Show("Вы действительно хотите завершить текущий бой и начать новый?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Hand, MessageBoxResult.No) == MessageBoxResult.No) 
                return;
            if (currentBattlePair is not null)
                FinishStage();
            SetRound();
        }

        private void FinishStage()
        {
            currentBattlePair.FighterRedScore = RedScore;
            currentBattlePair.FighterBlueScore = BlueScore;
            //Запись в файл текущего круга
            _writeBattlePairHandler.Execute(currentBattlePair);

            //Запись в файл итога
            var winnerResult = _battleResultBuilder.BuildWinner(currentBattlePair, Participants, _currentTurn);
            var loserResult = _battleResultBuilder.BuildLoser(currentBattlePair, Participants, _currentTurn);
            _writeBattleResultHandler.Execute(winnerResult);
            _writeBattleResultHandler.Execute(loserResult);
        }

        public void SetRound()
        {
            ClearScore();
            elapsedTime = new TimeSpan();
            timer.Stop();
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 1000;
            Time = elapsedTime.ToString(@"mm\:ss");

            CurrentBattlePair = nextBattlePair;

            StartButtonText = timer.Enabled ? "Стоп" : "Старт";
        }

        private void ClearScore()
        {
            RedScore = 0;
            BlueScore = 0;
            Doubles = 0;
        }

        public void PlusOneBlue()
        {
            BlueScore++;
        }
        public void MinusOneBlue()
        {
            BlueScore--;
        }
        public void PlusOneRed()
        {
            RedScore++;
        }
        public void MinusOneRed()
        {
            RedScore--;
        }
        public void PlusOneDouble()
        {
            Doubles++;
        }
        public void MinusOneDouble()
        {
            Doubles--;
        }
        public void Cancel()
        {
            ClearScore();
        }

        public void Cover()
        {
            IsCovered = !IsCovered;
        }

        public void GetReady()
        {
            nextBattlePair = selectedBattlePair;
        }

        public void StartStageN()
        {
            var current = 2;// currentStage.Id;

            battlePairs = _getBattlePairsHandler.Execute($"Круг {_currentTurn}");

            Participants = _getParticipantsHandler.Execute();

            var participantScores = _getParticipantsScoreHandler.Execute();

            var tmp = PairGenerator.GenerateBattlePairs(GenerationMode.Random, battlePairs.ToList(), participantScores.ToList());

            tmp.AddRange(battlePairs);

            var tmp2 = PairGenerator.GenerateBattlePairs(GenerationMode.Random, tmp.ToList(), participantScores.ToList());

            tmp2.AddRange(battlePairs);

            var tmp3 = PairGenerator.GenerateBattlePairs(GenerationMode.Random, tmp2.ToList(), participantScores.ToList());



        }
    }
}
