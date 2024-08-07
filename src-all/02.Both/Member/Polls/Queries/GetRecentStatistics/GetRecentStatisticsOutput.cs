namespace Delta.Polling.Both.Member.Polls.Queries.GetRecentStatistics;

public record GetRecentStatisticsOutput : Output<StatisticsUser>
{
}

public record StatisticsUser
{
    public required int NumParticipatedPoll { get; init; }
    public required int NumActivePollNotParticipated { get; init; }
}
