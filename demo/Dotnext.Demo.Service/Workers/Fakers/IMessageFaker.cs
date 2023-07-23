namespace Dotnext.Demo.Service.Workers.Fakers;

public interface IMessageFaker<TMessage>
{
    TMessage GenerateMessage();
}
