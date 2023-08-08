using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEMACounter
{
    public class Round : INotifyPropertyChanged
    {
        private int maxScore;
        public int MaxScore
        {
            get => maxScore;
            set
            {
                maxScore = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("MaxScore"));
            }
        }

        private string order;
        public string Order
        {
            get => order;
            set
            {
                order = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("Order"));
            }
        }

        private bool isCurrent;
        public bool IsCurrent
        {
            get => isCurrent;
            set
            {
                isCurrent = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("IsCurrent"));
            }
        }


        private string redFighter;
        public string RedFighter
        {
            get => redFighter;
            set
            {
                redFighter = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("RedFighter"));
            }
        }

        private string blueFighter;
        public string BlueFighter
        {
            get => blueFighter;
            set
            {
                blueFighter = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("BlueFighter"));
            }
        }

        private string fighters;
        public string Fighters
        {
            get => fighters;
            set
            {
                fighters = value;
                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs("Fighters"));
            }
        }


        public Round(string redFighter, string blueFighter, int maxScore, string order)
        {
            MaxScore = maxScore;
            Order = order;
            RedFighter = redFighter;
            BlueFighter = blueFighter;
            Fighters = $"{RedFighter} - {BlueFighter}";
        }   

        private PropertyChangedEventHandler? propertyChanged;
        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add { this.propertyChanged += value; }
            remove { this.propertyChanged -= value; }
        }
    }
}
