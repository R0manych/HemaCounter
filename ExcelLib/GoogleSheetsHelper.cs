using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace ExcelLib;

public class GoogleSheetsHelper
{
    private static GoogleSheetsHelper? _instance;
    public SheetsService Service { get; set; }
    static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };

    public static GoogleSheetsHelper Instance
    {
        get
        {
            _instance ??= new GoogleSheetsHelper();
            return _instance;
        }
    }

    private GoogleSheetsHelper()
    {
        InitializeService("D:\\Projects\\HEMACounter\\HEMACounter\\Secrets\\hemacounterapi-20a1e98718e8.json", "HemaCounter");
    }

    private GoogleSheetsHelper(string fileName, string appName)
    {
        InitializeService(fileName, appName);
    }

    private void InitializeService(string fileName, string appName)
    {
        var credential = GetCredentialsFromFile(fileName);
        Service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = appName
        });
    }

    private GoogleCredential GetCredentialsFromFile(string fileName)
    {
        GoogleCredential credential;
        using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
        }
        return credential;
    }
}
