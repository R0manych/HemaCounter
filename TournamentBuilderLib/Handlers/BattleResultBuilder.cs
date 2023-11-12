using HEMACounter;
using System;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers
{
    public interface IBattleResultBuilder
    {
        BattleResult BuildWinner(BattlePair pair, IEnumerable<IParticipant> participantsWithClub, int turn, bool withPenalty = false);

        BattleResult BuildLoser(BattlePair pair, IEnumerable<IParticipant> participantsWithClub, int turn, bool withPenalty = false);

        (BattleResult, BattleResult) BuildDraws(BattlePair pair, IEnumerable<IParticipant> participantsWithClub, int turn, bool withPenalty = false);

        (BattleResult, BattleResult) BuildTechnicalDefeat(BattlePair pair, IEnumerable<IParticipant> participantsWithClub, int turn, bool withPenalty = false);
    }

    public class BattleResultBuilder : IBattleResultBuilder
    {
        public BattleResult BuildWinner(BattlePair pair, IEnumerable<IParticipant> participantsWithClub, int stage, bool withPenalty = false)
        {
            var winner = pair.FighterRedScore > pair.FighterBlueScore ? pair.FighterRedName : pair.FighterBlueName;
            var winnerId = participantsWithClub.FirstOrDefault(p => p.Name == winner)?.Id + 1;
            var adressRange = ResultAddressMap[stage];
            var result = new BattleResult
            {
                Result = 1 + (withPenalty ? Settings.DoublesPenalty : 0),
                Score = Math.Abs(pair.FighterRedScore - pair.FighterBlueScore),
                Range = $"{adressRange.Item1}{winnerId}:{adressRange.Item2}{winnerId}"
            };
            return result;
        }

        public BattleResult BuildLoser(BattlePair pair, IEnumerable<IParticipant> participantsWithClub, int turn, bool withPenalty = false)
        {
            var loser = pair.FighterRedScore < pair.FighterBlueScore ? pair.FighterRedName : pair.FighterBlueName;
            var loserId = participantsWithClub.FirstOrDefault(p => p.Name == loser)?.Id + 1;
            var adressRange = ResultAddressMap[turn];
            var result = new BattleResult
            {
                Result = withPenalty ? Settings.DoublesPenalty : 0,
                Score = Math.Abs(pair.FighterRedScore - pair.FighterBlueScore) * -1,
                Range = $"{adressRange.Item1}{loserId}:{adressRange.Item2}{loserId}"
            };
            return result;
        }

        public (BattleResult, BattleResult) BuildDraws(BattlePair pair, IEnumerable<IParticipant> participantsWithClub, int turn, bool withPenalty = false)
        {
            var fighterRed = pair.FighterRedName;
            var fighterRedId = participantsWithClub.FirstOrDefault(p => p.Name == fighterRed)?.Id + 1;
            var adressRedRange = ResultAddressMap[turn];
            var resultRed = new BattleResult
            {
                Result = 0.5 + (withPenalty ? Settings.DoublesPenalty : 0),
                Score = 0,
                Range = $"{adressRedRange.Item1}{fighterRedId}:{adressRedRange.Item2}{fighterRedId}"
            };

            var fighterBlue = pair.FighterBlueName;
            var fighterBlueId = participantsWithClub.FirstOrDefault(p => p.Name == fighterBlue)?.Id + 1;
            var adressBlueRange = ResultAddressMap[turn];
            var resultBlue = new BattleResult
            {
                Result = 0.5 + (withPenalty ? Settings.DoublesPenalty : 0),
                Score = 0,
                Range = $"{adressBlueRange.Item1}{fighterBlueId}:{adressBlueRange.Item2}{fighterBlueId}"
            };

            return (resultRed, resultBlue);
        }

        public (BattleResult, BattleResult) BuildTechnicalDefeat(BattlePair pair, IEnumerable<IParticipant> participantsWithClub, int turn, bool withPenalty = false)
        {
            var fighterRed = pair.FighterRedName;
            var fighterRedId = participantsWithClub.FirstOrDefault(p => p.Name == fighterRed)?.Id + 1;
            var adressRedRange = ResultAddressMap[turn];
            var resultRed = new BattleResult
            {
                Result = withPenalty ? Settings.DoublesPenalty : 0,
                Score = pair.FighterRedScore - pair.FighterBlueScore,
                Range = $"{adressRedRange.Item1}{fighterRedId}:{adressRedRange.Item2}{fighterRedId}"
            };

            var fighterBlue = pair.FighterBlueName;
            var fighterBlueId = participantsWithClub.FirstOrDefault(p => p.Name == fighterBlue)?.Id + 1;
            var adressBlueRange = ResultAddressMap[turn];
            var resultBlue = new BattleResult
            {
                Result = withPenalty ? Settings.DoublesPenalty : 0,
                Score = pair.FighterBlueScore - pair.FighterRedScore,
                Range = $"{adressBlueRange.Item1}{fighterBlueId}:{adressBlueRange.Item2}{fighterBlueId}"
            };

            return (resultRed, resultBlue);
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
