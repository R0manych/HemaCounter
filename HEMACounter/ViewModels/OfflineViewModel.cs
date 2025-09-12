using HEMACounter.ViewModels.Base;
using System;
using System.Timers;

namespace HEMACounter.ViewModels
{
    public class OfflineViewModel : BaseViewModel
    {
        public OfflineViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            IsCovered = true;
            elapsedTime = new TimeSpan();
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 1000;
            Time = elapsedTime.ToString(@"mm\:ss");
            StartButtonText = timer.Enabled ? "Стоп" : "Старт";

            CurrentStage = new Models.Stage()
            {
                Id = 1,
                MaxScore = 15,
                Duration = TimeSpan.FromSeconds(120)
            };
        }

        public override void FinishFight()
        {
            //throw new NotImplementedException();
        }

        public override void GenerateStages()
        {
            //throw new NotImplementedException();
        }

        public override void GetReady()
        {
            //throw new NotImplementedException();
        }

        public override void OnStartTimer()
        {
            //throw new NotImplementedException();
        }

        public override void ReloadStageN()
        {
            //throw new NotImplementedException();
        }
    }
}
