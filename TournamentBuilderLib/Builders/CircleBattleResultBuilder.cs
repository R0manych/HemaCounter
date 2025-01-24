namespace TournamentBuilderLib.Builders
{
    public class CircleBattleResultBuilder : BattleResultBuilder
    {
        public CircleBattleResultBuilder() : base() { }

        protected override Dictionary<int, (string, string)> SetResultAddressMap()
        {
            return new Dictionary<int, (string, string)>()
            {
                { 1, ("B", "C") },
                { 2, ("B", "C") }
            };
        }
    }
}
