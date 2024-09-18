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
    internal class StartupViewModel : INotifyPropertyChanged
    {
        #region Parameters

        private Action<TournamentType, bool> _callback;

        private ObservableCollection<Tournament> tournaments = new ObservableCollection<Tournament>();
        public ObservableCollection<Tournament> Tournaments
        {
            get => tournaments;
            set
            {
                tournaments = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("Tournaments"));
            }
        }

        private Tournament selectedTournament;
        public Tournament SelectedTournament
        {
            get => selectedTournament;
            set
            {
                selectedTournament = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("SelectedTournament"));
            }
        }

        private bool withAdmin;
        public bool WithAdmin
        {
            get => withAdmin;
            set
            {
                withAdmin = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("WithAdmin"));
            }
        }

        private PropertyChangedEventHandler? propertyChanged;
        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        private readonly IGetSettingsHandler _getSettingsHandler = new GetSettingsHandler();

        #endregion

        #region Commands
        private ICommand runCommand;
        public ICommand RunCommand => runCommand ??= new CommandHandler(Run, () => true);
        #endregion

        public StartupViewModel(Action<TournamentType, bool> callback)
        {
            Initialize();
            _callback = callback;
        }

        private void Initialize()
        {
            var settings = _getSettingsHandler.GetSettings();
            Tournaments = new (settings.Select(GetTournamentFromSettings));
        }

        private void Run()
        {
            if (SelectedTournament == null)
                return;

            Settings.Current = SelectedTournament.Settings;

            switch (SelectedTournament.Name)
            {
                case "Рубильник":
                    if (SelectedTournament.Type == "Командные")
                    {
                        _callback(TournamentType.RubilnikTeam, WithAdmin);
                    }
                    else if (SelectedTournament.Type == "Индивидуальные")
                    {
                        _callback(TournamentType.RubilnikIndividual, WithAdmin);
                    }
                    return;
                case "Стальные яйца":
                    _callback(TournamentType.Stahlkugeln, WithAdmin);
                    return;
                case "Олимпийская":
                    return;
                default:
                    return;
            }
        }

        private Tournament GetTournamentFromSettings(Dictionary<string, string> settings) => 
            new Tournament()
            {
                Name = settings["TOURNAMENT_NAME"],
                Weapon = settings["WEAPON"],
                Gender = settings["GENDER"],
                Type = settings["NOMINATION_TYPE"],
                DocumentUrl = settings["DOC_URL"],
                Settings = settings
            };
    }
}