﻿using HEMACounter.Models;
using HEMACounter.ViewModels.Base;
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

namespace HEMACounter.ViewModels;

internal class TeamViewModel : BaseSwissViewModel<TeamParticipant>
{
    #region Parameters

    private List<(int, int)> matches = new List<(int, int)>() { (3, 6), (1, 5), (2, 4), (1, 6), (3, 4), (2, 5), (1, 4), (2, 6), (3, 5) };
    private Dictionary<int, string> currentBlueTeam = new Dictionary<int, string>();
    private Dictionary<int, string> currentRedTeam = new Dictionary<int, string>();
    private int currentRoundIndex = 0;

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

    #endregion

    #region Commands

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

    #endregion

    //TODO: рефакторинг
    private readonly GetTeamParticipantsHandler _getTeamParticipantsHandler = new(Settings.SheetId);

    public TeamViewModel()
    {
        Initialize();
    }

    private void Initialize()
    {
        IsCovered = true;
        ScorePerRound = Settings.ScoresPerRound ?? 6;
        elapsedTime = new TimeSpan();
        timer = new System.Timers.Timer();
        timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        timer.Interval = 1000;
        Time = elapsedTime.ToString(@"mm\:ss");
        ReloadParticipants();
        Teams = new ObservableCollection<TeamParticipant>(participants);
        StartButtonText = timer.Enabled ? "Стоп" : "Старт";
        GenerateStages();
        CurrentStage = Stages.First();
    }

    public override void ReloadParticipants()
    {
        participants = _getTeamParticipantsHandler.Execute();
        ReloadStageN();

        ResetNextFighters();
    }

    private void ResetNextFighters()
    {
        NextBattlePair = null;
        NextTeamRed = "";
        NextTeamBlue = "";
        NextFighter1 = "";
        NextFighter2 = "";
        NextFighter3 = "";
        NextFighter4 = "";
        NextFighter5 = "";
        NextFighter6 = "";
    }

    public void NextPair()
    {
        currentRoundIndex++;
        SetRoundInTeamFight(currentRoundIndex);
    }
    public void PreviousPair()
    {
        currentRoundIndex--;
        SetRoundInTeamFight(currentRoundIndex);
    }

    public void SetTeamRound()
    {
        ReloadStageN();
        ClearScore();
        ClearBackup();
        Rounds.Clear();

        elapsedTime = CurrentStage.Duration;
        timer.Stop();
        timer = new Timer();
        timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        Time = elapsedTime.ToString(@"mm\:ss");

        CurrentBattlePair = NextBattlePair;

        ResetNextFighters();

        StartButtonText = timer.Enabled ? "Стоп" : "Старт";

        timer.Stop();
        
        if (CurrentBattlePair == null)
            return;

        var redTeam = participants.FirstOrDefault(p => p.Name == CurrentBattlePair?.FighterRedName);
        var blueTeam = participants.FirstOrDefault(p => p.Name == CurrentBattlePair?.FighterBlueName);

        if (redTeam is null || blueTeam is null) 
            return;

        BlueTeamName = blueTeam.Name;
        RedTeamName = redTeam.Name;

        currentRedTeam = new Dictionary<int, string>
        {
            { 1, redTeam.Fighters[0] },
            { 2, redTeam.Fighters[1] },
            { 3, redTeam.Fighters[2] }
        };

        currentBlueTeam = new Dictionary<int, string>
        {
            { 4, blueTeam.Fighters[3] },
            { 5, blueTeam.Fighters[4] },
            { 6, blueTeam.Fighters[5] }
        };

        Rounds = new ObservableCollection<Round>(matches.Select((x, i) => new Round(currentRedTeam[x.Item1], currentBlueTeam[x.Item2], (i + 1) * ScorePerRound, $"({x.Item1} - {x.Item2})")));

        currentRoundIndex = 0;
        SetRoundInTeamFight(currentRoundIndex);
    }

    public void SetRoundInTeamFight(int roundIndex)
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

    public override void GetReady()
    {
        base.GetReady();
        if (NextBattlePair == null) 
            return;

        NextTeamRed = NextBattlePair.FighterRedName;
        NextTeamBlue = NextBattlePair.FighterBlueName;

        var nextRedTeam = participants.FirstOrDefault(p => p.Name == NextTeamRed);
        var nextBlueTeam = participants.FirstOrDefault(p => p.Name == NextTeamBlue);
        NextFighter1 = nextRedTeam.Fighters[0];
        NextFighter2 = nextRedTeam.Fighters[1];
        NextFighter3 = nextRedTeam.Fighters[2];

        NextFighter4 = nextBlueTeam.Fighters[3];
        NextFighter5 = nextBlueTeam.Fighters[4];
        NextFighter6 = nextBlueTeam.Fighters[5];
    }

    public override void NewFight()
    {
        if (CurrentBattlePair is not null)
        {
            if (MessageBox.Show("Вы действительно хотите завершить текущий бой и начать новый?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
                return;

            if (Doubles > CurrentStage.MaxDoubles && Settings.TechDefeatByDoublesEnabled)
            {
                if (MessageBox.Show("Счётчик обоюдных поражений превышает допустимое значение! \n Бой будет завершён техническим поражение обоих бойцов! \n Продолжить?",
                    "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Hand, MessageBoxResult.No) == MessageBoxResult.No)
                    return;
            }

            FinishFight();
        }
        SetTeamRound();
    }
}
