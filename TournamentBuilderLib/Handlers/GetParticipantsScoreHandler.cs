using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers
{
    public interface IGetParticipantsScoreHandler
    {
        IEnumerable<ParticipantScore> Execute();
    }

    public class GetParticipantsScoreHandler : IGetParticipantsScoreHandler
    {
        private readonly string SHEET_ID = GoogleSheet.Default.SHEET_ID;
        private readonly string SHEET_NAME = GoogleSheet.Default.SCORE_SHEET_NAME;

        public IEnumerable<ParticipantScore> Execute()
        {
            var range = $"{SHEET_NAME}";
            var values = ExcelReader.Read(SHEET_ID, range);
            var participantScores = new List<ParticipantScore>();
            foreach (var value in values)
            {
                ParticipantScore item = new()
                {
                    Name = value[0]?.ToString(),
                    WinScore = string.IsNullOrEmpty(value[1].ToString()) ? 0 : Convert.ToDecimal(value[1]),
                    PointsScore = string.IsNullOrEmpty(value[2].ToString()) ? 0 : Convert.ToInt32(value[2]),
                };
                participantScores.Add(item);
            }
            return participantScores;
        }
    }
}