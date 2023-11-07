using System.Collections.Generic;
using System.IO;
using System.Linq;
using TournamentBuilderLib.Models;

namespace HEMACounter;

internal class ParticipantsExport
{
    public static List<TeamParticipant> ExportTeam()
    {
        var ret = new List<TeamParticipant>();
        foreach (string line in File.ReadLines(@"teams.txt"))
        {
            var parts = line.Split(';');
            ret.Add(new TeamParticipant() { Name = parts.FirstOrDefault(), Fighters = parts.Skip(1).ToList() });
        }
        return ret;
    }
    public static List<string> ExportIndividuals()
    {
        return File.ReadLines(@"individuals.txt").ToList();
    }
}
