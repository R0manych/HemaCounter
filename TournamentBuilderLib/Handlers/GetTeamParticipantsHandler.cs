using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public class GetTeamParticipantsHandler
{
    private const string SHEET_NAME = "Участники";
    private string SHEET_ID = GoogleSheet.Default.SHEET_ID;

    public GetTeamParticipantsHandler()
    {

    }

    public IEnumerable<TeamParticipant> Execute()
    {
        var range = $"{SHEET_NAME}";
        var values = ExcelReader.Read(SHEET_ID, range);
        var participants = new List<TeamParticipant>();
        foreach (var value in values)
        {
            TeamParticipant item = new()
            {
                Id = Convert.ToInt32(value[0]),
                Name = value[1].ToString(),
            };
            item.Fighters.Add(value[2].ToString());
            item.Fighters.Add(value[3].ToString());
            item.Fighters.Add(value[4].ToString());
            participants.Add(item);
        }
        return participants;
    }
}
