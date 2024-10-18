using HEMACounter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
    internal class ComedyViewModel : INotifyPropertyChanged
    {
        #region Parameters

        private int backupRedScore;
        private int backupBlueScore;
        private int backupDoubles;

        private IEnumerable<IParticipant> participants = new List<IParticipant>();

        private ObservableCollection<BattlePair> battlePairs = new ObservableCollection<BattlePair>();
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

        public string CoverImagePath => $"/Images/{Settings.CoverFileName}";

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

        private int blueParam;
        public string BlueParam => $"{blueParam}";
       
        private int redParam;
        public string RedParam => $"{redParam}";

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
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(-1));
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
        public ICommand MinusOneRedCommand =>  minusOneRedCommand ??= new CommandHandler(MinusOneRed, () => true);

        private ICommand plusOneDoubleCommand;
        public ICommand PlusOneDoubleCommand => plusOneDoubleCommand ??= new CommandHandler(PlusOneDouble, () => true);

        private ICommand minusOneDoubleCommand;
        public ICommand MinusOneDoubleCommand =>  minusOneDoubleCommand ??= new CommandHandler(MinusOneDouble, () => true);

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

        private ICommand plusOneParamBlueCommand;
        public ICommand PlusOneParamBlueCommand => plusOneParamBlueCommand ??= new CommandHandler(PlusOneParamBlue, () => true);

        private ICommand minusOneParamBlueCommand;
        public ICommand MinusOneParamBlueCommand => minusOneParamBlueCommand ??= new CommandHandler(MinusOneParamBlue, () => true);

        private ICommand plusOneParamRedCommand;
        public ICommand PlusOneParamRedCommand => plusOneParamRedCommand ??= new CommandHandler(PlusOneParamRed, () => true);

        private ICommand minusOneParamRedCommand;
        public ICommand MinusOneParamRedCommand => minusOneParamRedCommand ??= new CommandHandler(MinusOneParamRed, () => true);

        #endregion

        private readonly IGetBattlePairsHandler _getBattlePairsHandler = new GetBattlePairsComedyHandler(Settings.SheetId);
        private readonly IGetParticipantsHandler _getParticipantsHandler = new GetParticipantsHandler(Settings.SheetId);
        private readonly IWriteBattlePairHandler _writeBattlePairHandler = new WriteBattlePairHandler(Settings.SheetId);
        private readonly IBattleResultBuilder _battleResultBuilder = new BattleResultBuilder();
        private readonly IWriteBattleResultHandler _writeBattleResultHandler = new WriteBattleResultHandler(Settings.SheetId);
        private readonly IGetParticipantsScoreHandler _getParticipantsScoreHandler = new GetParticipantsScoreHandler(Settings.SheetId);
        private readonly IGetParamHandler _getParamHandler = new GetParamHandler(Settings.SheetId);
        private readonly IUpdateParamHandler _updateParamHandler = new UpdateParamHandler(Settings.SheetId);

        public ComedyViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            participants = _getParticipantsHandler.Execute();

            IsCovered = true;
            elapsedTime = new TimeSpan();
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 1000;
            Time = elapsedTime.ToString(@"mm\:ss");
            StartButtonText = timer.Enabled ? "Стоп" : "Старт";

            GenerateDanteStages();
            currentStage = Stages.First();
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

            if (!timer.Enabled)
            {
                backupBlueScore = BlueScore;
                backupRedScore = RedScore;
                backupDoubles = Doubles;
            }
        }

        public void NewFight()
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

        private void FinishFight()
        {
            currentBattlePair!.FighterRedScore = RedScore;
            currentBattlePair!.FighterBlueScore = BlueScore;
            currentBattlePair!.DoublesCount = Doubles;
            currentBattlePair!.TimeInSeconds = (int)elapsedTime.TotalSeconds;

            //Запись в файл текущего круга
            _writeBattlePairHandler.Execute(currentBattlePair);

            if (doubles > currentStage.MaxDoubles) //техническое поражение обоим
            {
                var (resultRed, resultBlue) = _battleResultBuilder.BuildTechnicalDefeat(currentBattlePair, participants, currentStage.Id);
                _writeBattleResultHandler.Execute(resultRed);
                _writeBattleResultHandler.Execute(resultBlue);
                UpdateParam(currentBattlePair.FighterBlueName, 2);
                UpdateParam(currentBattlePair.FighterRedName, 2);
            }
            else if (currentBattlePair.IsDraw)
            {
                var (resultRed, resultBlue) = _battleResultBuilder.BuildDraws(currentBattlePair, participants, currentStage.Id);
                _writeBattleResultHandler.Execute(resultRed);
                _writeBattleResultHandler.Execute(resultBlue);
                UpdateParam(currentBattlePair.FighterBlueName, 1);
                UpdateParam(currentBattlePair.FighterRedName, 1);
            }
            else 
            { 
                var winnerResult = _battleResultBuilder.BuildWinner(currentBattlePair, participants, currentStage.Id);
                var loserResult = _battleResultBuilder.BuildLoser(currentBattlePair, participants, currentStage.Id);
                _writeBattleResultHandler.Execute(winnerResult);
                _writeBattleResultHandler.Execute(loserResult);
                UpdateParam(currentBattlePair.LooserName, 2);
                var winnerName = currentBattlePair.LooserName == currentBattlePair.FighterBlueName ? currentBattlePair.FighterRedName : currentBattlePair.FighterBlueName;
                UpdateParam(winnerName, 1);
            }
        }

        private void UpdateParam(string? fighterName, int diff)
        {
            if (fighterName is null)
                return;

            var participant = participants.FirstOrDefault(p => p.Name == fighterName);
            if (participant is not null)
            {
                _updateParamHandler.Execute(participant, diff);
            }

            ReloadParams();
        }

        private void ReloadParams()
        {
            if (currentBattlePair is null)
                return;

            var participantBlue = participants.FirstOrDefault(p => p.Name == currentBattlePair.FighterBlueName);
            if (participantBlue is not null)
            {
                blueParam = _getParamHandler.Execute(participantBlue);
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("BlueParam"));
            }

            var participantRed = participants.FirstOrDefault(p => p.Name == currentBattlePair.FighterRedName);
            if (participantRed is not null)
            {
                redParam = _getParamHandler.Execute(participantRed);
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("RedParam"));
            }
        }

        public void SetRound()
        {
            ReloadStageN();

            ClearScore();
            backupRedScore = 0;
            backupBlueScore = 0;
            backupDoubles = 0;

            elapsedTime = CurrentStage.Duration;
            timer.Stop();
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 1000;
            Time = elapsedTime.ToString(@"mm\:ss");

            CurrentBattlePair = nextBattlePair;
            NextBattlePair = null;

            StartButtonText = timer.Enabled ? "Стоп" : "Старт";

            ReloadParams();
        }

        private void ClearScore()
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

        public void PlusOneParamBlue()
        { 
            if (MessageBox.Show("Обычно мы не добавляем индульгенции таким образом. Вы уверены?", 
                "Подтвердите действие", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                UpdateParam(currentBattlePair?.FighterBlueName, 1);
        }

        public void MinusOneParamBlue() => UpdateParam(currentBattlePair?.FighterBlueName, -1);

        public void PlusOneParamRed()
        {
            if (MessageBox.Show("Обычно мы не добавляем индульгенции таким образом. Вы уверены?",
                "Подтвердите действие", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                UpdateParam(currentBattlePair?.FighterRedName, 1);
        }

        public void MinusOneParamRed() => UpdateParam(currentBattlePair?.FighterRedName, -1);

        public void Cover() => IsCovered = !IsCovered;

        //Это супер кнопка "Галя, отмена!"
        public void Cancel()
        {
            RedScore = backupRedScore;
            BlueScore = backupBlueScore;
            Doubles = backupDoubles;
        }

        public void GetReady()
        {
            if (NextBattlePair is not null)
            {
                NextBattlePair.IsStarted = false;
                _writeBattlePairHandler.Execute(NextBattlePair);
            }

            NextBattlePair = selectedBattlePair;
            SelectedBattlePair = null;

            if (NextBattlePair is null) 
                return;

            NextBattlePair.IsStarted = true;
            _writeBattlePairHandler.Execute(NextBattlePair);
        }

        public void ReloadStageN()
        {
            var current = currentStage.Id;
            var currentPairs = _getBattlePairsHandler.Execute($"Круг {current}", participants.Count())
                .Where(x => !x.IsStarted).ToList();

            BattlePairs.Clear();
            currentPairs.ForEach(BattlePairs.Add);
        }

        public void GenerateStageN()
        {
            var current = currentStage.Id;

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

            var generatedPairs = PairGenerator.GenerateBattlePairs(current > 4 ? GenerationMode.Swiss : GenerationMode.Random, 
                restrictedPairs.ToList(), participantScores.ToList());

            int i = 1;
            foreach (var pair in generatedPairs)
            {
                pair.Range = $"Круг {current}!D{i}:J{i}";
                _writeBattlePairHandler.Execute(pair);
                i++;
            }

            BattlePairs.Clear();
            generatedPairs.ForEach(BattlePairs.Add);
        }
    }
}
