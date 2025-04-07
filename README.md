# DrsBasicDI Dependency Injection Container Class Library
## Overview
The ***DrsBasicDI*** class library is a simple and lightweight dependency injection container designed for .NET applications. It provides a
straightforward way to manage object lifetimes and dependencies, making it easier to build maintainable and testable applications.

## Background
I started this project as a way to learn more about dependency injection and how to implement it in a .NET application. I wanted to create a simple
and lightweight container that could be used in small to medium-sized projects without the overhead of larger frameworks. The goal was to provide a
basic implementation that could meet the needs of moderately complex projects.

## Features
- **Simple API**: The fluent API is designed to be easy to use and understand, making it accessible for developers of all skill levels.
- **Lightweight**: The library is lightweight and has minimal dependencies, making it suitable for small to medium-sized projects.
- **Flexible**: The container supports various lifetimes (transient, singleton, and scoped) to accommodate different use cases.
- **Constructor Injection**: The container uses constructor injection to resolve dependencies, promoting best practices in software design.
- **Keyed Services**: The container allows for the registration of keyed services, enabling the resolution of specific implementations based on keys.
- **Thread-Safe**: The container is designed to be thread-safe, ensuring that it can be used in multi-threaded environments without issues.
- **Unit Tested**: The library includes unit tests to ensure the correctness and reliability of the implementation.

## The ***ContainerBuilder*** Class
The ***ContainerBuilder*** class is the main entry point for configuring the dependency injection container. It provides methods for registering
services, specifying their lifetimes, and building the container. The builder pattern is used to create a fluent API that allows for easy chaining of
methods. The ***ContainerBuilder*** class is designed to be simple and intuitive, making it easy for developers to configure their services without
the need for extensive documentation or examples. The builder pattern allows for a clean and readable syntax, enabling developers to focus on the
configuration of their services rather than the underlying implementation details.

### Retrieving an Instance of the ***ContainerBuilder*** Class
To retrieve an instance of the ***ContainerBuilder*** class, you can use the static method ***Instance***. This method returns a singleton instance of
the ***ContainerBuilder*** class, which can then be used to configure the container. The ***Instance*** method is a static factory method that
provides a convenient way to get the ***ContainerBuilder*** class without the need for a constructor. This approach allows for a more streamlined and
readable syntax when configuring the container, as developers can chain method calls directly after calling ***Instance***.

> [!NOTE]
> *The **Instance** method actually returns an **IContainerBuilder** interface object. This provides a level of abstraction between the user's source
> code and the internals of the **DrsBasicDI** class library.*

Example:
```csharp
using DrsBasicDI;
IContainerBuilder builder = ContainerBuilder.Instance;
```

### Registering Services
The ***ContainerBuilder*** class provides methods for registering services with different lifetimes. The following methods are available for
registering services that don't require the use of a resolving key or a factory method:

- ***AddTransient<TDependency, TResolving>()***: Registers a service with a transient lifetime. A new instance of the service will be created each
  time it is requested.
- ***AddSingleton<TDependency, TResolving>()***: Registers a service with a singleton lifetime. A single instance of the service will be created and
  shared across all requests.
- ***AddScoped<TDependency, TResolving>()***: Registers a service with a scoped lifetime. A new instance of the service will be created for each
  scope, and the same instance will be shared within that scope.

The *TDependency* type parameter is often referred to as the "service type" or "interface type," while the *TResolving* type parameter is
referred to as the "implementation type" or "concrete type." The *TDependency* type parameter represents the type that clients will depend on, while
the *TResolving* type parameter represents the actual implementation of that type. This separation allows for better abstraction and decoupling of
dependencies, making it easier to swap out implementations or create mock objects for testing purposes.

The following code snippet illustrates the use of these methods:
```csharp
using DrsBasicDI;
IContainer container = ContainerBuilder.Instance
    .AddTransient<IService, Service>()
    .AddSingleton<IRepository, Repository>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .Build();
```

There are a few things to take note of in this example:

- The fluent API allows these methods to be chained together. All of your services can be registered in a single statement.
- The chain of method calls ends with a call to the ***Build()*** method. This method creates an instance of the ***Container*** class, which is the
  actual dependency injection container that will be used to resolve dependencies. (The class is returned to the user as an **IContainer** interface
  object.)
- Once the container is built it can't be altered. This means that you can't add or remove services after the container has been built. This is a
  design decision that helps to ensure the integrity of the container and prevents accidental changes to the configuration after it has been set up.
- The ***ContainerBuilder*** is of no further use after the container has been built. In other words, you can't use the ***ContainerBuilder*** to
  build another container or to alter the existing one.

### Specifying Factory Methods and Resolving Keys
The ***ContainerBuilder*** class also provides methods for registering services that require a factory method and/or a resolving key. The following
methods are available for registering these types of services:

- ***AddTransient<TDependency, TResolving>(Func<DependencyBuilder, DependencyBuilder> builder)***
- ***AddSingleton<TDependency, TResolving>(Func<DependencyBuilder, DependencyBuilder> builder)***
- ***AddScoped<TDependency, TResolving>(Func<DependencyBuilder, DependencyBuilder> builder)***

Note that each of these methods takes a function that accepts a ***DependencyBuilder*** object and returns a ***DependencyBuilder*** object. This
allows you to further configure the service using the ***DependencyBuilder*** class. The ***DependencyBuilder*** class is a helper class that provides
a fluent API for configuring the properties of the service being registered. The ***DependencyBuilder*** class contains two methods that are pertinent
to our discussion:

- ***WithFactory(Func\<object> factory)***: Specifies a factory method that will be used to create the instance of the service. This allows for more
  complex initialization logic or the use of external resources when creating the service.
- ***WithResolvingKey(string resolvingKey)***: Specifies a resolving key that can be used to resolve the service. This allows for the registration of
  multiple implementations of the same service type, enabling the use of different implementations based on the resolving key.

The ***DependencyBuilder*** class isn't accessible directly by the end user. Instead, it must be accessed through the use of a lambda expression which
is passed to the ***AddTransient***, ***AddSingleton***, or ***AddScoped*** methods. This is done to ensure that the ***DependencyBuilder*** is only
used in the context of service registration and to prevent accidental misuse or confusion with other parts of the API. By requiring the use of a
lambda expression, we can ensure that the ***DependencyBuilder*** is only used in the appropriate context and that it is properly disposed of after
the service has been registered. This helps to maintain the integrity of the container and prevents accidental changes to the configuration after it
has been set up.

The following code snippet illustrates the use of these methods:
```csharp
using DrsBasicDI;

public interface IMyService
{
    string Name { get; }
}

public class MyService : IMyService
{
    public MyService(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

public const string Key1 = "Test1";
public const string Key2 = "Test2";

IContainer container = ContainerBuilder.Instance
    .AddTransient<IMyService, MyService>(builder => builder
        .WithFactory(() => new MyService("Transient"))
        .WithResolvingKey(Key1))
    .AddSingleton<IMyService, MyService>(builder => builder
        .WithResolvingKey(Key2))
    .AddScoped<IMyService, MyService>(builder => builder
        .WithFactory(() => new MyService("Scoped"))
    .Build();
```

In this example, we register three different implementations of the ***IMyService*** interface using different lifetimes and resolving keys. Two of
the implementations use the ***WithFactory*** method to specify a factory method that creates the instance of the service, and two use the
***WithResolvingKey*** method to specify a resolving key that can be used to resolve the service. Also, note that the factory method must not take any
parameters and it must return an object whose type is assignable to the service type. This is a design decision that helps to ensure that the factory
method is simple and easy to use, while still providing the flexibility needed to create complex objects.

One other thing to note is that services that are registered without the ***WithResolvingKey*** method will default to a key that is an empty string.
That is why in the example we don't need to call the ***WithResolvingKey*** method for the ***AddScoped*** method even though we are registering
another implementation of the ***IMyService*** interface. The ***AddScoped*** method will automatically use the empty string as the resolving key
unless we specify otherwise. This allows for a more streamlined and readable syntax, as we don't need to specify the resolving key for every
implementation of the service.
