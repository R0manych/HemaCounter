using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IGetIndulgetnciaHandler
{
    public int Execute(IParticipant participant);
}

public class GetIndulgetnciaHandler : IGetIndulgetnciaHandler
{
    private const string SHEET_NAME = "Список участников";
    private string SHEET_ID = GoogleSheet.Default.SHEET_ID;

    public int Execute(IParticipant participant)
    {
        var cellAdress = $"{SHEET_NAME}!D{participant.Id}";
        var values = ExcelReader.Read(SHEET_ID, cellAdress);
        return Convert.ToInt32(values.FirstOrDefault()?[0]);
    }
}
