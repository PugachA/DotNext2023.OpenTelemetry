#pragma warning disable CS8618

namespace Dotnext.Demo.Core.Domain;
public interface IMessage
{
    public string MessageId { get; set; }

    public DateTime TimestampUtc { get; set; }
}
