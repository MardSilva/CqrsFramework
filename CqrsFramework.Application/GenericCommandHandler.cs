using CqrsFramework.CqrsFramework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsFramework.CqrsFramework.Application
{
    public class GenericCommandHandler : ICommandHandler<ICommand>
    {
        public Task HandleAsync(ICommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Handling command: {command.CommandName}");

            var commandType = command.GetType();
            foreach (var property in commandType.GetProperties())
            {
                var value = property.GetValue(command);
                Console.WriteLine($"{property.Name}: {value}");
            }

            return Task.CompletedTask;
        }
    }
}