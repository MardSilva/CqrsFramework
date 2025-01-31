# CQRS Framework

Introduction

CQRS Framework is a .NET library designed to implement the Command Query Responsibility Segregation (CQRS) pattern efficiently. This framework simplifies the creation and management of Commands, with future plans to support Queries, ensuring a structured, scalable, and maintainable application architecture.

CQRS allows for a clear separation between write (commands) and read (queries) operations, optimizing performance and maintainability. Traditional architectures often mix these responsibilities, leading to complex models that are harder to scale and maintain.

With this framework, you can dynamically generate commands at runtime, defining whether they should have mutable (get + set) or immutable (get only)` properties, providing flexibility while enforcing business rules where needed.


### Installation
Using NuGet (if published)

```bash
 dotnet add package CqrsFramework --version 1.0.0
```

### Building the Library Manually

If you want to use the project directly:

```bash
git clone https://github.com/your-username/CqrsFramework.git
cd CqrsFramework
dotnet build
```

### Why Use CQRS Framework?

1. Separation of Commands and Queries

- Mixing read and write operations within the same model can lead to complexity. CQRS ensures a clean distinction between them, improving scalability and maintainability.

2. Dynamic Command Generation

- Commands can be generated dynamically at runtime, adapting to business needs without requiring static model definitions.

3. Strict or Flexible Property Management

- Developers can choose between strict mode (immutable commands) or flexible mode (modifiable commands) depending on application requirements.

4. Scalability and Maintainability

- CQRS enables independent optimization of read and write operations, making it particularly useful for distributed systems and microservices.

### How to Use
Creating a Dynamic Command

The framework allows you to create commands dynamically with predefined properties. Here’s an example of generating a UserCommand dynamically:

```csharp
var properties = new Dictionary<string, Type>
{
    { "Id", typeof(Guid) },
    { "Name", typeof(string) },
    { "Email", typeof(string) },
    { "CreatedAt", typeof(DateTime) }
};

var typeBuilder = DynamicTypeBuilder.CreateType("UserCommand");
foreach (var property in properties)
{
    typeBuilder.AddProperty(property.Key, property.Value, isStrictCqrs: false);
}
var commandType = typeBuilder.Build();
var commandInstance = Activator.CreateInstance(commandType);

commandType.GetProperty("Id")?.SetValue(commandInstance, Guid.NewGuid());
commandType.GetProperty("Name")?.SetValue(commandInstance, "John Doe");
commandType.GetProperty("Email")?.SetValue(commandInstance, "john.doe@email.com");
commandType.GetProperty("CreatedAt")?.SetValue(commandInstance, DateTime.UtcNow);
```

Creating an Immutable CQRS Command (get only)

If you need an immutable command, you can define all properties as read-only:

```csharp
var typeBuilder = DynamicTypeBuilder.CreateType("UserCommand");
foreach (var property in properties)
{
    typeBuilder.AddProperty(property.Key, property.Value, isStrictCqrs: true);
}
var commandType = typeBuilder.Build();

var constructor = commandType.GetConstructors().First();
var commandInstance = constructor.Invoke(new object[]
{
    Guid.NewGuid(),
    "John Doe",
    "john.doe@email.com",
    DateTime.UtcNow
});
```

### Processing Commands with a Handler

```csharp
public class GenericCommandHandler : ICommandHandler<object>
{
    public Task HandleAsync(object command, CancellationToken cancellationToken)
    {
        var type = command.GetType();
        Console.WriteLine($"Handling command: {type.Name}");
        return Task.CompletedTask;
    }
}
```

### Future Plans: Query Support
While CQRS Framework currently focuses on command generation, future updates will include query handling, allowing complete separation of read and write models in .NET applications.

### Contributing
Contributions are welcome! Open a Pull Request or Issue in the repository to discuss improvements.

### Support
For questions and support, contact us via your-email@example.com or create an Issue on GitHub.

### License
This project is licensed under MIT. See the LICENSE file for more details.
