using Convey.CQRS.Events;

using System.Threading.Tasks;

namespace Pacco.Services.Availability.Application.Events.External.Handlers
{
    public class SignedUpHandler : IEventHandler<SignedUp>
    {
        public Task HandleAsync(SignedUp @event)
        {
            return Task.CompletedTask;
        }
    }
}
