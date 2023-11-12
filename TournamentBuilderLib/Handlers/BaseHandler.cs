namespace TournamentBuilderLib.Handlers;

public class BaseHandler
{
    protected readonly string _sheetId;

    public BaseHandler(string sheetId)
    {
        _sheetId = sheetId;
    }
}
