using ExcelLib;
using TournamentBuilderLib.Participants.Models;

namespace TournamentBuilderLib.Participants.Handlers;

public interface IGetParticipantsHandler
{
    public IEnumerable<IParticipant> Execute();
}

public class GetParticipantsHandler : IGetParticipantsHandler
{
    private const string SHEET_NAME = "Список участников";
    private const string SHEET_ID = "1Q7oySMjF3tiB-dlPkiIur9dyyqyGQz7qd3SKtarqS2Q";

    public GetParticipantsHandler()
    {

    }

    public IEnumerable<IParticipant> Execute()
    {
        var range = $"{SHEET_NAME}";
        var values = ExcelReader.Read(SHEET_ID, range);
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
