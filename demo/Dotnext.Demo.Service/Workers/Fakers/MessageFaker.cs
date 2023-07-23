using Bogus;
using Dotnext.Demo.Service.Messages;

namespace Dotnext.Demo.Service.Workers.Fakers;

public class MessageFaker : IMessageFaker<UserMessage>, IMessageFaker<OrderMessage>
{
    private Faker<UserMessage> _userFaker;
    private Faker<OrderMessage> _orderFaker;

    public MessageFaker()
    {
        _userFaker = new Faker<UserMessage>()
            .RuleFor(l => l.Id, f => f.IndexFaker)
            .RuleFor(l => l.Email, f => f.Person.Email)
            .RuleFor(l => l.Name, f => f.Person.UserName)
            .RuleFor(l => l.TimestampUtc, f => DateTime.UtcNow);

        _orderFaker = new Faker<OrderMessage>()
            .RuleFor(l => l.Id, f => f.IndexFaker)
            .RuleFor(l => l.ProductName, f => f.Lorem.Word())
            .RuleFor(l => l.Date, f => f.Date.Recent())
            .RuleFor(l => l.Price, f => f.Finance.Amount())
            .RuleFor(l => l.TimestampUtc, f => DateTime.UtcNow);
    }

    public UserMessage GenerateMessage()
    {
        return _userFaker.Generate();
    }

    OrderMessage IMessageFaker<OrderMessage>.GenerateMessage()
    {
        return _orderFaker.Generate();
    }
}
