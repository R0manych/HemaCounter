using HEMACounter.ViewModels.Base;
using System.Linq;
using System.Timers;
using System;
using TournamentBuilderLib.Models;

namespace HEMACounter.ViewModels;

internal class IndividualViewModel : BaseSwissViewModel<ParticipantWithClub>
{
    public IndividualViewModel()
    {
        Initialize();
    }

    public void Initialize()
    {
        participants = _getParticipantsHandler.Execute();
        IsCovered = true;
        elapsedTime = new TimeSpan();
        timer = new System.Timers.Timer();
        timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        timer.Interval = 1000;
        Time = elapsedTime.ToString(@"mm\:ss");
        StartButtonText = timer.Enabled ? "Стоп" : "Старт";
        GenerateStages();
        CurrentStage = Stages.First();
    }

    public override void GetReady()
    {
        base.GetReady();

        if (NextBattlePair == null)
            return;

        NextRedFighter = NextBattlePair.FighterRedName;
        NextBlueFighter = NextBattlePair.FighterBlueName;
    }

    public override void ReloadParticipants()
    {
        participants = _getParticipantsHandler.Execute();
        ReloadStageN();

        ResetNextFighters();
    }

    private void ResetNextFighters()
    {
        NextBattlePair = null;
        NextRedFighter = null;
        NextBlueFighter = null;
    }

    public override void SetRound()
    {
        base.SetRound();
        ResetNextFighters();
    }
}