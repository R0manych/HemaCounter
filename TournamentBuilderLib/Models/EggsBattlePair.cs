using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentBuilderLib.Models
{
    public class EggsBattlePair
    {
        /// <summary>
        /// Это команда в командной номинации или участник в индивидуальной
        /// </summary>
        /// <param name="fighter1Name"></param>
        /// <param name="fighter2Name"></param>
        public EggsBattlePair(string fighter1Name, string fighter2Name)
        {
            if (DateTime.Now.Ticks % 2 == 0)
            {
                FighterRedName = fighter1Name;
                FighterBlueName = fighter2Name;
            }
            else
            {
                FighterRedName = fighter2Name;
                FighterBlueName = fighter1Name;
            }
        }

        public EggsBattlePair()
        { }

        public string FighterRedName { get; set; }

        public int FighterRedScore { get; set; }

        public string FighterBlueName { get; set; }

        public int FighterBlueScore { get; set; }

        public bool IsStarted { get; set; }

        public string FighterRedRange { get; set; }

        public string FighterBlueRange { get; set; }

        public string Caption => $"{FighterRedName} - {FighterBlueName}";

        public bool IsDraw => FighterBlueScore == FighterRedScore;

        public string? LooserName => IsDraw ? null : FighterBlueScore > FighterRedScore ? FighterRedName : FighterBlueName;
    }
}
