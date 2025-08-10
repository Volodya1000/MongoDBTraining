using MongoDBTraining.Domain.Common;

namespace MongoDBTraining.Domain.Entities;

public class Movie:BaseEntity
{
    public required string Name { get; set; }

    public required string Description { get; set; }

    public List<string> ActorIds { get; set; } = new();
}
