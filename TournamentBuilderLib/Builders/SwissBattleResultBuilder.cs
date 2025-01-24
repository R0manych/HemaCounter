namespace TournamentBuilderLib.Builders
{
    public class SwissBattleResultBuilder : BattleResultBuilder
    {
        public SwissBattleResultBuilder() : base() { }

        protected override Dictionary<int, (string, string)> SetResultAddressMap()
        {
            return new Dictionary<int, (string, string)>()
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
}
