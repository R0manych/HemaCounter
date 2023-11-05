namespace TournamentBuilderLib.Models;

public interface IParticipantWithClub : IParticipant
{
    public string? ClubName { get; set; }
}

public class ParticipantWithClub : IParticipantWithClub
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ClubName { get; set; }
}
