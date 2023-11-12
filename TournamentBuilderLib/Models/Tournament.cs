using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentBuilderLib.Models
{
    public class Tournament
    {
        public string Name { get; set; }
        public string Weapon { get; set; }
        public string Gender { get; set; }
        public string Type { get; set; }
        public string DocumentUrl { get; set; }

        public Dictionary<string, string> Settings { get; set; }
    }
}
