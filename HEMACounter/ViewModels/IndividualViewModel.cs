﻿using HEMACounter.ViewModels.Base;
using System.Linq;
using System.Timers;
using System;

namespace HEMACounter.ViewModels;

internal class IndividualViewModel : BaseSwissViewModel
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
}
