using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IUpdateParamHandler
{
    public void Execute(IParticipant participant, int diff);
}

public class UpdateParamHandler : BaseHandler, IUpdateParamHandler
{
    private const string _sheetName = "Список участников";

    public UpdateParamHandler(string sheetId) : base(sheetId)
    {
    }

    public void Execute(IParticipant participant, int diff)
    {
        var cellAdress = $"{_sheetName}!D{participant.Id}";
        var values = ExcelReader.Read(_sheetId, cellAdress);

        var curCount = Convert.ToInt32(values.FirstOrDefault()?[0]);

        var objectList = new List<object>()
        {
            curCount + diff
        };

        var rangeData = new List<IList<object>> { objectList };
        ExcelWriter.Write(_sheetId, cellAdress, rangeData);
    }
}
