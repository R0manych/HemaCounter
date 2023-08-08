using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEMACounter
{
    internal class ParticipantsExport
    {
        public static List<TeamInfo> ExportTeam()
        {
            var ret = new List<TeamInfo>();
            foreach (string line in File.ReadLines(@"teams.txt"))
            {
                var parts = line.Split(';');
                ret.Add(new TeamInfo() { Name = parts.FirstOrDefault(), Fighters = parts.Skip(1).ToList() });
            }
            return ret;
        }
        public static List<string> ExportIndividuals()
        {
            return File.ReadLines(@"individuals.txt").ToList();
        }
    }
}
