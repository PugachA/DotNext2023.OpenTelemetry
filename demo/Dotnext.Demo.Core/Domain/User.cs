#pragma warning disable CS8618

namespace Dotnext.Demo.Core.Domain;
public class User : IEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
