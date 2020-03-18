using Convey.CQRS.Commands;
using System;

namespace Pacco.Services.Availability.Application.Commands
{
    public class ReserveResource: ICommand
    {
        public Guid ResourceId { get; }
        public DateTime DateTime { get; }
        public int Priority { get; }

        public ReserveResource(Guid resourceId, DateTime dateTime, int priority)
        {
            ResourceId = resourceId;
            DateTime = dateTime;
            Priority = priority;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
