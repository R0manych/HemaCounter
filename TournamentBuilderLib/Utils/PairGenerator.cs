using HEMACounter;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Utils;

public static class PairGenerator
{
    private static Random random = new Random();

    public static List<BattlePair> GenerateBattlePairs(GenerationMode mode, List<BattlePair> restrictedPairs, List<ParticipantScore> rating)
    {
        if (mode == GenerationMode.Random)
        {
            rating.Shuffle();
        }

        if (mode == GenerationMode.Swiss)
        {
            rating = rating.OrderByDescending(x => x.WinScore).ThenByDescending(x => x.PointsScore).ToList();
        }

        return GetPairs(rating, new(), restrictedPairs)!;
    }

    private static List<BattlePair>? GetPairs(List<ParticipantScore> rating, HashSet<string> busy, List<BattlePair> restrictedPairs)
    {
        var ret = new List<BattlePair>();

        //берём первого в рейтинге, кто ещё не занят
        var fighter = rating.First(x => !busy.Contains(x.Name));

        //идём по всем кандидатам, кто уже не в парах
        var candidates = rating.Where(x => !busy.Contains(x.Name) && x.Name != fighter.Name);

        //если остался только один неспаренный чувак
        if (candidates.Count() == 1)
        {
            //и если эти двое не встречались, мы их сразу возвращаем, иначе - null
            if (!IsPairRestricted(fighter.Name, candidates.First().Name, restrictedPairs))
                return new List<BattlePair>() { new BattlePair(fighter.Name, candidates.First().Name) };
            else
                return null;
        }

        //если таких больше, мы идём по кандидатам 
        foreach (var candidate in candidates)
        {
            //если кандидат уже встречался с нашим первым, скипаем
            if (IsPairRestricted(fighter.Name, candidate.Name, restrictedPairs)) continue;

            //если нет - предполагаем их парой и ныряем глубже
            var busyClone = busy.ToHashSet();
            busyClone.Add(fighter.Name);
            busyClone.Add(candidate.Name);
            var deeperPairs = GetPairs(rating, busyClone, restrictedPairs);

            //если те, что внизу, сматчились, возвращаем нашу пару + то, что внизу
            if (deeperPairs is not null)
            {
                deeperPairs.Add(new BattlePair(fighter.Name, candidate.Name));
                return deeperPairs;
            }

            //если не сматчились, идём по кандидатам дальше
        }
        //если дошли до конца - и нихрена, значит эта ветка битая
        return null;
    }

    private static bool IsPairRestricted(string fighter1, string fighter2, List<BattlePair> restrictedPairs) =>
        restrictedPairs.Any(x => (x.FighterRedName == fighter1 && x.FighterBlueName == fighter2) 
        || (x.FighterRedName == fighter2 && x.FighterBlueName == fighter1));
    

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}