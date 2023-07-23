#pragma warning disable CS8618
#pragma warning disable VSSpell001 // Spell Check

namespace Dotnext.Demo.Core.Domain;
public class IMessage
{
    public string MessageId { get; set; }

    public DateTime TimestampUtc { get; set; }
}
