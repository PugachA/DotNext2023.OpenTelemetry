#pragma warning disable CS8618

namespace Dotnext.Demo.Core.Domain;
public class Order : IEntity
{
    public long Id { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public DateTime Date { get; set; }
}
