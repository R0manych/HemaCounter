using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IUpdateParamHandler
{
    public void Execute(IParticipant participant, int diff);
}

public class UpdateParamHandler : IUpdateParamHandler
{
    private readonly string SHEET_ID = GoogleSheet.Default.SHEET_ID;
    private const string SHEET_NAME = "Список участников";

    public void Execute(IParticipant participant, int diff)
    {
        var cellAdress = $"{SHEET_NAME}!D{participant.Id}";
        var values = ExcelReader.Read(SHEET_ID, cellAdress);

        var curCount = Convert.ToInt32(values.FirstOrDefault()?[0]);

        var objectList = new List<object>()
        {
            curCount + diff
        };

        var rangeData = new List<IList<object>> { objectList };
        ExcelWriter.Write(SHEET_ID, cellAdress, rangeData);
    }
}
