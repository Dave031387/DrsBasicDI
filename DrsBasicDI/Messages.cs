namespace DrsBasicDI;

internal static class Messages
{
    internal const string MsgAbstractResolvingType = "The {0} for {1} must not be an abstract class type.";
    internal const string MsgCantAddToContainerAfterBuild = "Adding a dependency to the container after the container has already been built is not allowed.";
    internal const string MsgContainerCantBeBuiltMoreThanOnce = "The container has already been built. It can't be built again.";
    internal const string MsgContainerIsEmpty = "The dependency container must contain at least one dependency.";
    internal const string MsgDependencyMappingNotFound = "No mapping exists for {0}.";
    internal const string MsgDependencyTypeAlreadySpecified = "Invalid attempt to assign dependency type more than once to {0}.";
    internal const string MsgDependencyTypeName = "dependency type {0}";
    internal const string MsgDuplicateDependency = "The {0} has already been added to the container.";
    internal const string MsgErrorDuringConstruction = "An exception was thrown while trying to construct an instance of {0} for {1}.";
    internal const string MsgFactoryAlreadySpecified = "Invalid attempt to assign factory more than once to {0}.";
    internal const string MsgFactoryInvocationError = "An exception was thrown when invoking the Factory method for {0}.";
    internal const string MsgGenericDependencyTypeIsOpen = "Generic {0} must be fully constructed.";
    internal const string MsgIncompatibleFactory = "The {0} returned from the factory is not assignable to {1}.";
    internal const string MsgIncompatibleResolvingType = "The {0} is not assignable to {1}.";
    internal const string MsgInvalidDependencyType = "The {0} must be a class type or interface type.";
    internal const string MsgInvalidResolvingType = "The {0} for {1} must be a class type.";
    internal const string MsgLifetimeAlreadySpecified = "Invalid attempt to assign lifetime more than once to {0}.";
    internal const string MsgNoSuitableConstructors = "No suitable constructor could be found for {0}.";
    internal const string MsgNullDependencyObject = "Unexpected null dependency object returned for {0}.";
    internal const string MsgNullDependencyType = "Invalid attempt to assign a null value to the dependency type.";
    internal const string MsgNullFactory = "Invalid attempt to assign a null value to the factory for {0}.";
    internal const string MsgNullResolvingKey = "Invalid attempt to assign a null value to the resolving key for {0}.";
    internal const string MsgNullResolvingType = "Invalid attempt to assign a null value to the resolving type for {0}.";
    internal const string MsgResolveMethodInfoNotFound = "The attempt to retrieve the MethodInfo for the Resolve method failed.";
    internal const string MsgResolveMethodInvocationError = "An exception was thrown when invoking the Resolve method for {0}.";
    internal const string MsgResolvingGenericTypeIsOpen = "The generic {0} for {1} must be fully constructed.";
    internal const string MsgResolvingKeyAlreadySpecified = "Invalid attempt to assign a resolving key more than once to {0}.";
    internal const string MsgResolvingKeyName = " having resolving key \"{0}\"";
    internal const string MsgResolvingObjectNotCreated = "An instance of {0} could not be constructed for {1}.";
    internal const string MsgResolvingTypeAlreadySpecified = "Invalid attempt to assign resolving type more than once to {0}.";
    internal const string MsgResolvingTypeName = "resolving type {0}";
    internal const string MsgScopedServiceAlreadySet = "The scoped resolving objects service has already been set in the scoped dependency resolver object.";
    internal const string MsgScopedServiceIsNull = "Invalid attempt to assign a null value to the scoped resolving objects service.";
    internal const string MsgScopedServiceSameAsNonScopedService = "The scoped resolving objects service must not be the same as the non-scoped service.";
    internal const string MsgServiceNotRegistered = "No service has been registered for type {0}.";
    internal const string MsgUnableToMakeGenericResolveMethod = "Unable to create a generic Resolve method for {0}";
    internal const string MsgUndefinedLifetime = "The {0} must not have an undefined lifetime.";
    internal const string MsgUnspecifiedDependencyType = "Unable to build dependency having an unspecified dependency type.";
    internal const string MsgUnspecifiedResolvingType = "The {0} must not have an unspecified resolving type.";

    internal static string GetDependencyName(string dependencyType, string resolvingKey)
    {
        string part1 = string.Format(MsgDependencyTypeName, dependencyType);
        string part2 = resolvingKey == EmptyKey ? string.Empty : string.Format(MsgResolvingKeyName, resolvingKey);
        return part1 + part2;
    }

    internal static string GetResolvingName(string resolvingType)
        => string.Format(MsgResolvingTypeName, resolvingType);
}