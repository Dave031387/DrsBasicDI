namespace DrsBasicDI;

/// <summary>
/// The <see cref="Container" /> class implements a basic dependency injection container.
/// </summary>
public sealed class Container
{
    /// <summary>
    /// A dictionary of <see cref="Dependency" /> objects whose keys are the corresponding
    /// dependency type specified by the <see cref="Dependency.DependencyType" /> property.
    /// </summary>
    internal readonly Dictionary<Type, Dependency> _dependencies = [];

    /// <summary>
    /// Default constructor for the <see cref="Container" /> class.
    /// </summary>
    /// <remarks>
    /// This constructor is declared <see langword="internal" /> to force the user to use the
    /// <see cref="ContainerBuilder" /> object to construct new <see cref="Container" /> objects.
    /// </remarks>
    internal Container()
    {
    }
}