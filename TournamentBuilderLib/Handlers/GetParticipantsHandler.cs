using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IGetParticipantsHandler
{
    public IEnumerable<IParticipant> Execute();
}

public class GetParticipantsHandler : IGetParticipantsHandler
{
    private const string SHEET_NAME = "Список участников";
    private string SHEET_ID = GoogleSheet.Default.SHEET_ID;

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
