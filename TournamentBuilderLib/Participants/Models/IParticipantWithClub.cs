namespace TournamentBuilderLib.Participants.Models;

internal interface IParticipantWithClub : IParticipant
{
    public string? ClubName { get; set; }
}

public class ParticipantWithClub : IParticipantWithClub
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ClubName { get; set; }
}
