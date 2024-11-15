using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IGetParamHandler
{
    public int Execute(IParticipant participant);
}

public class GetParamHandler : BaseHandler, IGetParamHandler
{
    private const string _sheetName = "Участники";

    public GetParamHandler(string sheetId) : base(sheetId)
    {
    }

    public int Execute(IParticipant participant)
    {
        var cellAdress = $"{_sheetName}!D{participant.Id}";
        var values = ExcelReader.Read(_sheetId, cellAdress);
        return Convert.ToInt32(values.FirstOrDefault()?[0]);
    }
}
