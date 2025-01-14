namespace DrsBasicDI;

internal static class Messages
{
    internal const string MsgAbstractResolvingType = "Resolving type {0} for dependency type {1} must not be an abstract class type.";
    internal const string MsgContainerIsEmpty = "The dependency container must contain at least one dependency.";
    internal const string MsgDependencyTypeAlreadySpecified = "Invalid attempt to assign dependency type more than once to dependency {0}.";
    internal const string MsgDuplicateDependency = "A dependency of type {0} has already been added to the container.";
    internal const string MsgFactoryAlreadySpecified = "Invalid attempt to assign factory more than once to dependency type {0}.";
    internal const string MsgGenericDependencyTypeIsOpen = "Generic dependency type {0} must be fully constructed.";
    internal const string MsgIncompatibleFactory = "The type returned from the factory must be assignable to dependency type {0}.";
    internal const string MsgIncompatibleResolvingType = "Resolving type {0} is not assignable to dependency type {1}.";
    internal const string MsgInvalidDependencyType = "The dependency type {0} must be a class type or interface type.";
    internal const string MsgInvalidResolvingType = "The resolving type {0} for dependency type {1} is not a class type.";
    internal const string MsgLifetimeAlreadySpecified = "Invalid attempt to assign lifetime more than once to dependency type {0}.";
    internal const string MsgResolvingGenericTypeIsOpen = "Resolving generic type {0} for dependency type {1} must be fully constructed.";
    internal const string MsgResolvingTypeAlreadySpecified = "Invalid attempt to assign resolving type more than once to dependency type {0}.";
    internal const string MsgUndefinedLifetime = "Dependency type {0} must not have an undefined lifetime.";
    internal const string MsgUnspecifiedDependencyType = "Unable to build dependency having an unspecified dependency type.";
    internal const string MsgUnspecifiedResolvingType = "Dependency type {0} must not have an unspecified resolving type.";
}