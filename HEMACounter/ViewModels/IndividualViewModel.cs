﻿using HEMACounter.Models;
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
    internal class IndividualViewModel : INotifyPropertyChanged
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
                {
                    propertyChanged(this, new PropertyChangedEventArgs("CurrentStage"));
                    propertyChanged(this, new PropertyChangedEventArgs("StageCaption"));
                    propertyChanged(this, new PropertyChangedEventArgs("MaxScoreCaption"));
                    propertyChanged(this, new PropertyChangedEventArgs("DurationCaption"));
                    propertyChanged(this, new PropertyChangedEventArgs("MaxDoublesCaption"));
                }
            }
        }

        public string StageCaption
        {
            get => $"Круг: {currentStage.Id}";
        }

        public string MaxScoreCaption
        {
            get => $"Макс. баллов: {currentStage.MaxScore}";
        }

        public string MaxDoublesCaption
        {
            get => $"Макс. обоюдок: {currentStage.MaxDoubles}";
        }

        public string DurationCaption
        {
            get => $"Время боя: " + currentStage.Duration.ToString(@"mm\:ss");
        }

        public string? NextBattlePairCaption
        {
            get => nextBattlePair?.Caption;
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
                    "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Hand, MessageBoxResult.No) == MessageBoxResult.No) 
                    return;

                FinishFight();
            }
            SetRound();
        }

        private void FinishFight()
        {
            currentBattlePair!.FighterRedScore = RedScore;
            currentBattlePair!.FighterBlueScore = BlueScore;

            //Запись в файл текущего круга
            _writeBattlePairHandler.Execute(currentBattlePair);

            //Запись в файл итога
            var participants = _getParticipantsHandler.Execute();

            if (currentBattlePair.IsDraw)
            {
                var (resultRed, resultBlue) = _battleResultBuilder.BuildDraws(currentBattlePair, participants, currentStage.Id);
                _writeBattleResultHandler.Execute(resultRed);
                _writeBattleResultHandler.Execute(resultBlue);
            }
            else 
            { 
                var winnerResult = _battleResultBuilder.BuildWinner(currentBattlePair, participants, currentStage.Id);
                var loserResult = _battleResultBuilder.BuildLoser(currentBattlePair, participants, currentStage.Id);
                _writeBattleResultHandler.Execute(winnerResult);
                _writeBattleResultHandler.Execute(loserResult);
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

            var generatedPairs = PairGenerator.GenerateBattlePairs(current > 3 ? GenerationMode.Swiss : GenerationMode.Random, 
                restrictedPairs.ToList(), participantScores.ToList());

            int i = 1;
            foreach (var pair in generatedPairs)
            {
                pair.Range = $"Круг {current}!D{i}:H{i}";
                _writeBattlePairHandler.Execute(pair);
                i++;
            }

            BattlePairs.Clear();
            generatedPairs.ForEach(BattlePairs.Add);
        }
    }
}
