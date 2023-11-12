using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IGetParticipantsHandler
{
    public IEnumerable<ParticipantWithClub> Execute();
}

public class GetParticipantsHandler : BaseHandler, IGetParticipantsHandler
{
    private const string _sheetName = "Список участников";

    public GetParticipantsHandler(string sheetId) : base(sheetId)
    {
    }

    public IEnumerable<ParticipantWithClub> Execute()
    {
        var range = $"{_sheetName}";
        var values = ExcelReader.Read(_sheetId, range);
        var participants = new List<ParticipantWithClub>();
        foreach (var value in values)
        {
            ParticipantWithClub item = new()
            {
                Id = Convert.ToInt32(value[0]),
                Name = value[1].ToString(),
                ClubName = value[2]?.ToString(),
            };
            participants.Add(item);
        }
        return participants;
    }
}
