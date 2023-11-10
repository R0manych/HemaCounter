using HEMACounter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using TournamentBuilderLib.Models;

namespace HEMACounter.ViewModels.Base
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Parameters

        private int backupRedScore;
        private int backupBlueScore;
        private int backupDoubles;

        protected IEnumerable<IParticipant> participants = new List<IParticipant>();

        private ObservableCollection<BattlePair> battlePairs = new();
        public ObservableCollection<BattlePair> BattlePairs
        {
            get => battlePairs;
            set
            {
                battlePairs = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("BattlePairs"));
            }
        }

        private BattlePair? selectedBattlePair;
        public BattlePair? SelectedBattlePair
        {
            get => selectedBattlePair;
            set
            {
                selectedBattlePair = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("SelectedBattlePair"));
            }
        }

        private BattlePair? currentBattlePair;
        public BattlePair? CurrentBattlePair
        {
            get => currentBattlePair;
            set
            {
                currentBattlePair = value;
                CurrentRedFighter = value?.FighterRedName;
                CurrentBlueFighter = value?.FighterBlueName;
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

        private string? currentRedFighter;
        public string? CurrentRedFighter
        {
            get { return currentRedFighter; }
            set
            {
                currentRedFighter = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs(nameof(CurrentRedFighter)));
            }
        }

        private string? currentBlueFighter;
        public string? CurrentBlueFighter
        {
            get { return currentBlueFighter; }
            set
            {
                currentBlueFighter = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs(nameof(CurrentBlueFighter)));
            }
        }

        private string? nextRedFighter;
        public string? NextRedFighter
        {
            get { return nextRedFighter; }
            set
            {
                nextRedFighter = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs(nameof(NextRedFighter)));
            }
        }

        private string? nextBlueFighter;
        public string? NextBlueFighter
        {
            get { return nextBlueFighter; }
            set
            {
                nextBlueFighter = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs(nameof(NextBlueFighter)));
            }
        }

        private BattlePair? nextBattlePair;
        public BattlePair? NextBattlePair
        {
            get => nextBattlePair;
            set
            {
                nextBattlePair = value;
                NextRedFighter = value?.FighterRedName;
                NextBlueFighter = value?.FighterBlueName;
                if (propertyChanged != null)
                {
                    propertyChanged(this, new PropertyChangedEventArgs("NextBattlePair.FighterBlueName"));
                    propertyChanged(this, new PropertyChangedEventArgs("NextBattlePair.FighterRedName"));
                    propertyChanged(this, new PropertyChangedEventArgs("NextBattlePairCaption"));
                    propertyChanged(this, new PropertyChangedEventArgs("NextRedFighter"));
                    propertyChanged(this, new PropertyChangedEventArgs("NextBlueFighter"));
                }
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
                    propertyChanged(this, new PropertyChangedEventArgs("DoublesCaption"));
            }
        }

        public string DoublesCaption => $"{doubles} / {currentStage.MaxDoubles}";

        private Stage currentStage;
        public Stage CurrentStage
        {
            get => currentStage;
            set
            {
                currentStage = value;
                if (propertyChanged != null)
                {
                    propertyChanged(this, new PropertyChangedEventArgs("CurrentStage"));
                    propertyChanged(this, new PropertyChangedEventArgs("StageCaption"));
                    propertyChanged(this, new PropertyChangedEventArgs("MaxScoreCaption"));
                    propertyChanged(this, new PropertyChangedEventArgs("DurationCaption"));
                    propertyChanged(this, new PropertyChangedEventArgs("MaxDoublesCaption"));
                }
            }
        }

        public string StageCaption => $"Круг: {currentStage.Id}";

        public string MaxScoreCaption => $"Макс. баллов: {currentStage.MaxScore}";

        public string MaxDoublesCaption => $"Макс. обоюдок: {currentStage.MaxDoubles}";

        public string DurationCaption => $"Время боя: " + currentStage.Duration.ToString(@"mm\:ss");

        public string? NextBattlePairCaption => nextBattlePair?.Caption;

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

        protected PropertyChangedEventHandler? propertyChanged;
        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        #endregion

        #region Timer

        protected static System.Timers.Timer timer;
        protected TimeSpan elapsedTime;
        protected void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
            Time = elapsedTime.ToString(@"mm\:ss");
        }

        #endregion

        #region Commands

        private ICommand startStopCommand;
        public ICommand StartStopCommand => startStopCommand ??= new CommandHandler(StartStopTimer, () => true);

        private ICommand newFightCommand;
        public ICommand NewFightCommand => newFightCommand ??= new CommandHandler(NewFight, () => true);

        private ICommand plusOneBlueCommand;
        public ICommand PlusOneBlueCommand => plusOneBlueCommand ??= new CommandHandler(PlusOneBlue, () => true);

        private ICommand minusOneBlueCommand;
        public ICommand MinusOneBlueCommand => minusOneBlueCommand ??= new CommandHandler(MinusOneBlue, () => true);

        private ICommand plusOneRedCommand;
        public ICommand PlusOneRedCommand => plusOneRedCommand ??= new CommandHandler(PlusOneRed, () => true);

        private ICommand minusOneRedCommand;
        public ICommand MinusOneRedCommand => minusOneRedCommand ??= new CommandHandler(MinusOneRed, () => true);

        private ICommand plusOneDoubleCommand;
        public ICommand PlusOneDoubleCommand => plusOneDoubleCommand ??= new CommandHandler(PlusOneDouble, () => true);

        private ICommand minusOneDoubleCommand;
        public ICommand MinusOneDoubleCommand => minusOneDoubleCommand ??= new CommandHandler(MinusOneDouble, () => true);

        private ICommand cancelCommand;
        public ICommand CancelCommand => cancelCommand ??= new CommandHandler(Cancel, () => true);

        private ICommand coverCommand;
        public ICommand CoverCommand => coverCommand ??= new CommandHandler(Cover, () => true);

        private ICommand getReadyCommand;
        public ICommand GetReadyCommand => getReadyCommand ??= new CommandHandler(GetReady, () => true);

        private ICommand generateStageNCommand;
        public ICommand GenerateStageNCommand => generateStageNCommand ??= new CommandHandler(GenerateStageN, () => true);

        private ICommand loadStageNCommand;
        public ICommand LoadStageNCommand => loadStageNCommand ??= new CommandHandler(ReloadStageN, () => true);

        #endregion

        public void StartStopTimer()
        {
            if (timer.Enabled)
                timer.Stop();
            else
                timer.Start();

            StartButtonText = timer.Enabled ? "Стоп" : "Старт";

            if (!timer.Enabled)
            {
                backupBlueScore = BlueScore;
                backupRedScore = RedScore;
                backupDoubles = Doubles;
            }
        }

        public void ClearScore()
        {
            RedScore = 0;
            BlueScore = 0;
            Doubles = 0;
        }

        public void PlusOneBlue() => BlueScore++;

        public void MinusOneBlue() => BlueScore--;

        public void PlusOneRed() => RedScore++;

        public void MinusOneRed() => RedScore--;

        public void PlusOneDouble() => Doubles++;

        public void MinusOneDouble() => Doubles--;

        public void Cover() => IsCovered = !IsCovered;

        //Это супер кнопка "Галя, отмена!"
        public void Cancel()
        {
            RedScore = backupRedScore;
            BlueScore = backupBlueScore;
            Doubles = backupDoubles;
        }

        public void ClearBackup()
        {
            backupRedScore = 0;
            backupBlueScore = 0;
            backupDoubles = 0;
        }

        public virtual void NewFight()
        {
            if (currentBattlePair is not null)
            {
                if (MessageBox.Show("Вы действительно хотите завершить текущий бой и начать новый?",
                    "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
                    return;

                if (doubles > currentStage.MaxDoubles)
                {
                    if (MessageBox.Show("Счётчик обоюдных поражений превышает допустимое значение! \n Бой будет завершён техническим поражение обоих бойцов! \n Продолжить?",
                        "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Hand, MessageBoxResult.No) == MessageBoxResult.No)
                        return;
                }

                FinishFight();
            }
            SetRound();
        }

        public virtual void SetRound()
        {
            ReloadStageN();

            ClearScore();
            ClearBackup();

            elapsedTime = CurrentStage.Duration;
            timer.Stop();
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 1000;
            Time = elapsedTime.ToString(@"mm\:ss");

            CurrentBattlePair = nextBattlePair;
            NextBattlePair = null;

            StartButtonText = timer.Enabled ? "Стоп" : "Старт";
        }

        public abstract void GetReady();

        public abstract void ReloadStageN();

        protected abstract void GenerateStageN();

        public abstract void FinishFight();

        public abstract void GenerateStages();
    }
}
