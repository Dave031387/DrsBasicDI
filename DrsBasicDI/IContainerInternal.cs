namespace DrsBasicDI;

/// <summary>
/// The <see cref="IContainerInternal" /> interface extends the <see cref="IContainer" /> interface
/// by adding a couple properties that should be accessible only to other classes in the
/// <see cref="DrsBasicDI" /> class library.
/// </summary>
internal interface IContainerInternal : IContainer
{
    /// <summary>
    /// Get the dictionary of <see cref="Dependency" /> objects whose keys are the corresponding
    /// dependency type specified by the <see cref="Dependency.DependencyType" /> property.
    /// </summary>
    Dictionary<Type, Dependency> Dependencies
    {
        get;
    }

    /// <summary>
    /// Get the <see cref="IResolvingObjectsService" /> instance used to manage all the non-scoped
    /// resolved dependencies in this dependency injection container.
    /// </summary>
    IResolvingObjectsService ResolvingObjectsService
    {
        get;
    }
}