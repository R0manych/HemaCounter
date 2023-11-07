using HEMACounter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using TournamentBuilderLib.Handlers;
using TournamentBuilderLib.Models;
using TournamentBuilderLib.Utils;

namespace HEMACounter.ViewModels;

internal class TeamViewModel : INotifyPropertyChanged
{
    #region Parameters

    private int backupRedScore;
    private int backupBlueScore;
    private int backupDoubles;
    private int currentRoundIndex = 0;

    private List<(int, int)> matches = new List<(int, int)>() { (3, 6), (1, 5), (2, 4), (1, 6), (3, 4), (2, 5), (1, 4), (2, 6), (3, 5) };
    private Dictionary<int, string> currentBlueTeam = new Dictionary<int, string>();
    private Dictionary<int, string> currentRedTeam = new Dictionary<int, string>();

    private ObservableCollection<Round> rounds = new ObservableCollection<Round>();
    public ObservableCollection<Round> Rounds
    {
        get => rounds;
        set
        {
            rounds = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("Rounds"));
        }
    }

    private ObservableCollection<TeamParticipant> teams = new ObservableCollection<TeamParticipant>();
    public ObservableCollection<TeamParticipant> Teams
    {
        get => teams;
        set
        {
            teams = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("Teams"));
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

    private TeamParticipant selectedBlueTeam;
    public TeamParticipant SelectedBlueTeam
    {
        get => selectedBlueTeam;
        set
        {
            selectedBlueTeam = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("SelectedBlueTeam"));


            if (selectedBlueTeam.Fighters.Count >= 6)
            {
                NextTeamBlue = selectedBlueTeam.Name;
                NextFighter4 = selectedBlueTeam.Fighters[3];
                NextFighter5 = selectedBlueTeam.Fighters[4];
                NextFighter6 = selectedBlueTeam.Fighters[5];
            }
        }
    }

    private TeamParticipant selectedRedTeam;
    public TeamParticipant SelectedRedTeam
    {
        get => selectedRedTeam;
        set
        {
            selectedRedTeam = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("SelectedRedTeam"));

            if (selectedRedTeam.Fighters.Count >= 6)
            {
                NextTeamRed = selectedRedTeam.Name;
                NextFighter1 = selectedRedTeam.Fighters[0];
                NextFighter2 = selectedRedTeam.Fighters[1];
                NextFighter3 = selectedRedTeam.Fighters[2];
            }
        }
    }

    private bool isTeamCompetition;
    public bool IsTeamCompetition
    {
        get => isTeamCompetition;
        set
        {
            isTeamCompetition = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("IsTeamCompetition"));
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

    private string blueTeamName;
    public string BlueTeamName
    {
        get => blueTeamName;
        set
        {
            blueTeamName = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("BlueTeamName"));
        }
    }

    private string redTeamName;
    public string RedTeamName
    {
        get => redTeamName;
        set
        {
            redTeamName = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("RedTeamName"));
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

    private string currentRedFighter;
    public string CurrentRedFighter
    {
        get => currentRedFighter;
        set
        {
            currentRedFighter = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("CurrentRedFighter"));
        }
    }

    private string currentBlueFighter;
    public string CurrentBlueFighter
    {
        get => currentBlueFighter;
        set
        {
            currentBlueFighter = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("CurrentBlueFighter"));
        }
    }

    private string nextRedFighter;
    public string NextRedFighter
    {
        get => nextRedFighter;
        set
        {
            nextRedFighter = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("NextRedFighter"));
        }
    }

    private string nextBlueFighter;
    public string NextBlueFighter
    {
        get => nextBlueFighter;
        set
        {
            nextBlueFighter = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("NextBlueFighter"));
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

    private string nextTeamRed;
    public string NextTeamRed
    {
        get => nextTeamRed;
        set
        {
            nextTeamRed = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("NextTeamRed"));
        }
    }

    private string nextTeamBlue;
    public string NextTeamBlue
    {
        get => nextTeamBlue;
        set
        {
            nextTeamBlue = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("NextTeamBlue"));
        }
    }

    private string nextFighter1;
    public string NextFighter1
    {
        get => nextFighter1;
        set
        {
            nextFighter1 = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("NextFighter1"));
        }
    }
    private string nextFighter2;
    public string NextFighter2
    {
        get => nextFighter2;
        set
        {
            nextFighter2 = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("NextFighter2"));
        }
    }
    private string nextFighter3;
    public string NextFighter3
    {
        get => nextFighter3;
        set
        {
            nextFighter3 = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("NextFighter3"));
        }
    }
    private string nextFighter4;
    public string NextFighter4
    {
        get => nextFighter4;
        set
        {
            nextFighter4 = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("NextFighter4"));
        }
    }
    private string nextFighter5;
    public string NextFighter5
    {
        get => nextFighter5;
        set
        {
            nextFighter5 = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("NextFighter5"));
        }
    }
    private string nextFighter6;
    public string NextFighter6
    {
        get => nextFighter6;
        set
        {
            nextFighter6 = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("NextFighter6"));
        }
    }

    private int scorePerRound;
    public int ScorePerRound
    {
        get => scorePerRound;
        set
        {
            scorePerRound = value;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("ScorePerRound"));
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

    private ICommand nextPairCommand;
    public ICommand NextPairCommand
    {
        get
        {
            return nextPairCommand ?? (nextPairCommand = new CommandHandler(() => NextPair(), () => { return true; })); ;
        }
    }

    private ICommand previousPairCommand;
    public ICommand PreviousPairCommand
    {
        get
        {
            return previousPairCommand ?? (previousPairCommand = new CommandHandler(() => PreviousPair(), () => { return true; })); ;
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

    #endregion

    //TODO: Рефакторинг
    #region NEW

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

    private IEnumerable<TeamParticipant> _participants = new List<TeamParticipant>();

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

    #endregion

    private readonly GetTeamParticipantsHandler _getTeamParticipantsHandler = new();

    private readonly IGetTeamBattlePairsHandler _getBattlePairsHandler = new GetTeamBattlePairsHandler();
    private readonly IGetParticipantsHandler _getParticipantsHandler = new GetParticipantsHandler();
    private readonly IWriteBattlePairHandler _writeBattlePairHandler = new WriteBattlePairHandler();
    private readonly IBattleResultBuilder _battleResultBuilder = new BattleResultBuilder();
    private readonly IWriteBattleResultHandler _writeBattleResultHandler = new WriteBattleResultHandler();
    private readonly IGetParticipantsScoreHandler _getParticipantsScoreHandler = new GetParticipantsScoreHandler();
    private readonly IGetParamHandler _getParamHandler = new GetParamHandler();
    private readonly IUpdateParamHandler _updateParamHandler = new UpdateParamHandler();

    public TeamViewModel()
    {
        Initialize();
    }

    private void Initialize()
    {
        IsCovered = true;
        IsTeamCompetition = true;
        ScorePerRound = 7;
        elapsedTime = new TimeSpan();
        timer = new System.Timers.Timer();
        timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        timer.Interval = 1000;
        Time = elapsedTime.ToString(@"mm\:ss");
        _participants = _getTeamParticipantsHandler.Execute();
        Teams = new ObservableCollection<TeamParticipant>(_participants);
        StartButtonText = timer.Enabled ? "Стоп" : "Старт";
        GenerateStages();
        currentStage = Stages.First();
        GenerateStageN();
    }

    public void StartStopTimer()
    {
        if (timer.Enabled)
            timer.Stop();
        else
            timer.Start();

        if (!timer.Enabled)
        {
            backupBlueScore = BlueScore;
            backupRedScore = RedScore;
            backupDoubles = Doubles;
        }
        StartButtonText = timer.Enabled ? "Стоп" : "Старт";
    }

    public void NextPair()
    {
        currentRoundIndex++;
        SetRound(currentRoundIndex);
    }
    public void PreviousPair()
    {
        currentRoundIndex--;
        SetRound(currentRoundIndex);
    }

    public void SetRound(int roundIndex)
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
    }

    public void NewFight()
    {
        if (MessageBox.Show("Вы действительно хотите завершить текущий бой и начать новый?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Hand, MessageBoxResult.No) == MessageBoxResult.No) return;

        timer.Stop();

        currentRoundIndex = 0;

        BlueTeamName = nextTeamBlue;
        RedTeamName = nextTeamRed;

        currentRedTeam = new Dictionary<int, string>
        {
            { 1, nextFighter1 },
            { 2, nextFighter2 },
            { 3, nextFighter3 }
        };

        currentBlueTeam = new Dictionary<int, string>
        {
            { 4, nextFighter4 },
            { 5, nextFighter5 },
            { 6, nextFighter6 }
        };

        Rounds = new ObservableCollection<Round>(matches.Select((x, i) => new Round(currentRedTeam[x.Item1], currentBlueTeam[x.Item2], (i + 1) * ScorePerRound, $"({x.Item1} - {x.Item2})")));

        RedScore = 0;
        BlueScore = 0;
        Doubles = 0;

        backupBlueScore = 0;
        backupRedScore = 0;
        backupDoubles = 0;

        SetRound(currentRoundIndex);
    }

    private void FinishFight()
    {
        currentBattlePair!.FighterRedScore = RedScore;
        currentBattlePair!.FighterBlueScore = BlueScore;

        //Запись в файл текущего круга
        _writeBattlePairHandler.Execute(currentBattlePair);

        if (doubles > currentStage.MaxDoubles) //техническое поражение обоим
        {
            var (resultRed, resultBlue) = _battleResultBuilder
                .BuildTechnicalDefeat(currentBattlePair, _participants, currentStage.Id);
            _writeBattleResultHandler.Execute(resultRed);
            _writeBattleResultHandler.Execute(resultBlue);
        }
        else if (currentBattlePair.IsDraw)
        {
            var (resultRed, resultBlue) = _battleResultBuilder
                .BuildDraws(currentBattlePair, _participants, currentStage.Id);
            _writeBattleResultHandler.Execute(resultRed);
            _writeBattleResultHandler.Execute(resultBlue);
        }
        else
        {
            var winnerResult = _battleResultBuilder
                .BuildWinner(currentBattlePair, _participants, currentStage.Id);
            var loserResult = _battleResultBuilder
                .BuildLoser(currentBattlePair, _participants, currentStage.Id);
            _writeBattleResultHandler.Execute(winnerResult);
            _writeBattleResultHandler.Execute(loserResult);
        }
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
        RedScore = backupRedScore;
        BlueScore = backupBlueScore;
        Doubles = backupDoubles;
    }
    public void Cover()
    {
        IsCovered = !IsCovered;
    }

    public void GenerateStageN()
    {
        var current = currentStage.Id;

        if (_getBattlePairsHandler.Execute($"Круг {current}", _participants.Count())
            .Where(x => x.IsStarted).Any())
        {
            MessageBox.Show("Круг уже начался!");
            return;
        }

        var restrictedPairs = new List<BattlePair>();

        for (int turn = 1; turn < current; turn++)
        {
            restrictedPairs.AddRange(_getBattlePairsHandler.Execute($"Круг {turn}", _participants.Count()));
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

    private void GenerateStages()
    {
        Stages.Clear();
        Enumerable.Range(1, 5).Select(x => new Stage()
        {
            Id = x,
            MaxScore = 7 * x,
            MaxDoubles = x * 2,
            Duration = TimeSpan.FromSeconds(60)
        }).ToList().ForEach(Stages.Add);
    }
}
