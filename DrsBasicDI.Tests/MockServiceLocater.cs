namespace DrsBasicDI;

/// <summary>
/// The <see cref="MockServiceLocater" /> class is a mock implementation of the
/// <see cref="ServiceLocater" /> class that returns mock objects instead of live objects.
/// </summary>
internal class MockServiceLocater : IMockServiceLocater
{
    /// <summary>
    /// A dictionary of mock objects that have been created by the <see cref="MockServiceLocater" />
    /// instance.
    /// </summary>
    private readonly Dictionary<ServiceKey, Mock> _mockObjects = [];

    /// <summary>
    /// Create a mock object of type <see cref="Mock{T}" /> having the given resolving
    /// <paramref name="key" /> and save it for later use.
    /// </summary>
    /// <typeparam name="T">
    /// The type of mock object to be created.
    /// </typeparam>
    /// <param name="key">
    /// The resolving key used to uniquely identify the mock object.
    /// </param>
    public void CreateMock<T>(string key = EmptyKey) where T : class
    {
        ServiceKey serviceKey = new(typeof(T), key);

        _mockObjects
            .Should()
            .NotContainKey(serviceKey, "the mock object can only be created once");

        _mockObjects[serviceKey] = new Mock<T>(MockBehavior.Strict);
    }

    /// <summary>
    /// Get a mock object of type <typeparamref name="T" /> having the given resolving
    /// <paramref name="key" />.
    /// </summary>
    /// <typeparam name="T">
    /// The type of mock object to be retrieved.
    /// </typeparam>
    /// <param name="key">
    /// The resolving key used to uniquely identify the mock object.
    /// </param>
    /// <returns>
    /// A mock object of type <typeparamref name="T" />.
    /// </returns>
    public T Get<T>(string key = EmptyKey) where T : class
    {
        ServiceKey serviceKey = new(typeof(T), key);

        _mockObjects
            .Should()
            .ContainKey(serviceKey, "the mock object should have already been created");

        return ((Mock<T>)_mockObjects[serviceKey]).Object;
    }

    /// <summary>
    /// Create a mock object of type <see cref="Mock{T}" /> having the given resolving
    /// <paramref name="key" /> and save it for later use.
    /// </summary>
    /// <typeparam name="T">
    /// The type of mock object to be created.
    /// </typeparam>
    /// <param name="key">
    /// The resolving key used to uniquely identify the mock object.
    /// </param>
    /// <returns>
    /// A mock object of type <see cref="Mock{T}" />.
    /// </returns>
    public Mock<T> GetMock<T>(string key = EmptyKey) where T : class
    {
        ServiceKey serviceKey = new(typeof(T), key);

        if (_mockObjects.TryGetValue(serviceKey, out Mock? value))
        {
            return (Mock<T>)value;
        }

        CreateMock<T>(key);
        return (Mock<T>)_mockObjects[serviceKey];
    }

    /// <summary>
    /// Verify all the mock objects created by the <see cref="MockServiceLocater" /> instance.
    /// </summary>
    public void VerifyMocks() => Mock.VerifyAll([.. _mockObjects.Values]);
}