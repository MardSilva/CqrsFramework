using System;
using System.Collections.Generic;

namespace CqrsFramework.Infrastructure
{
    public static class CommandFactory
    {
        public static Type CreateCommand(string commandName, Dictionary<string, Type> properties)
        {
            var typeBuilder = DynamicTypeBuilder.CreateType(commandName);

            foreach (var property in properties)
            {
                typeBuilder.AddProperty(property.Key, property.Value);
            }

            return typeBuilder.Build();
        }
    }
}