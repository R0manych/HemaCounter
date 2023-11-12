using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public class GetTeamParticipantsHandler: BaseHandler
{
    private const string _sheetName = "Участники";

    public GetTeamParticipantsHandler(string sheetId) : base(sheetId)
    {
    }

    public IEnumerable<TeamParticipant> Execute()
    {
        var range = $"{_sheetName}";
        var values = ExcelReader.Read(_sheetId, range);
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
            item.Fighters.Add(value[5].ToString());
            item.Fighters.Add(value[6].ToString());
            item.Fighters.Add(value[7].ToString());
            participants.Add(item);
        }
        return participants;
    }
}
