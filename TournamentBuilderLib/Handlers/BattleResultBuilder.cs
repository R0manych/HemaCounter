using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers
{
    public interface IBattleResultBuilder
    {
        BattleResult BuildWinner(BattlePair pair, IEnumerable<IParticipant> participantsWithClub, int turn);

        BattleResult BuildLoser(BattlePair pair, IEnumerable<IParticipant> participantsWithClub, int turn);
    }

    public class BattleResultBuilder : IBattleResultBuilder
    {
        public BattleResult BuildWinner(BattlePair pair, IEnumerable<IParticipant> participantsWithClub, int turn)
        {
            var winner = pair.FighterRedScore >= pair.FighterBlueScore ? pair.FighterRedName : pair.FighterBlueName;
            var winnerId = participantsWithClub.FirstOrDefault(p => p.Name == winner)?.Id + 1;
            var adressRange = ResultAddressMap[turn];
            var result = new BattleResult
            {
                Result = 1,
                Score = Math.Abs(pair.FighterRedScore - pair.FighterBlueScore),
                Range = $"{adressRange.Item1}{winnerId}:{adressRange.Item2}{winnerId}"
            };
            return result;
        }

        public BattleResult BuildLoser(BattlePair pair, IEnumerable<IParticipant> participantsWithClub, int turn)
        {
            var loser = pair.FighterRedScore < pair.FighterBlueScore ? pair.FighterRedName : pair.FighterBlueName;
            var loserId = participantsWithClub.FirstOrDefault(p => p.Name == loser)?.Id + 1;
            var adressRange = ResultAddressMap[turn];
            var result = new BattleResult
            {
                Result = 0,
                Score = Math.Abs(pair.FighterRedScore - pair.FighterBlueScore) * -1,
                Range = $"{adressRange.Item1}{loserId}:{adressRange.Item2}{loserId}"
            };
            return result;
        }

        private Dictionary<int, (string, string)> ResultAddressMap = new Dictionary<int, (string, string)>()
        {
            { 1, ("B", "C") },
            { 2, ("D", "E") },
            { 3, ("F", "G") },
            { 4, ("H", "I") },
            { 5, ("J", "K") },
            { 6, ("L", "M") },
            { 7, ("N", "O") },
            { 8, ("P", "Q") },
            { 9, ("R", "S") },
        };
    }
}
