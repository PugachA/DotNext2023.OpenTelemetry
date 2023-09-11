#pragma warning disable CS8618

using Dotnext.Demo.Core.Domain;

namespace Dotnext.Demo.Service.Messages;

public class UserMessage : IMessage
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string MessageId { get; set; }
    public DateTime TimestampUtc { get; set; }
}
