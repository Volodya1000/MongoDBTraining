using MongoDBTraining.Domain.Common;

namespace MongoDBTraining.Domain.Entities;

public class Actor : BaseEntity
{
    public required string Name { get; set; }

    public required string LastName { get; set; }

    public List<string> MovieIds { get; set; } = new();
}
