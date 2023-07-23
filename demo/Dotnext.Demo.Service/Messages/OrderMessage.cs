#pragma warning disable CS8618

using Dotnext.Demo.Core.Domain;

namespace Dotnext.Demo.Service.Messages;

public class OrderMessage : IMessage
{
    public long Id { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public DateTime Date { get; set; }
}
