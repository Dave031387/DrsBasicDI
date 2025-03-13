namespace DrsBasicDI;

internal class MockServiceLocater : IMockServiceLocater
{
    private readonly Dictionary<ServiceKey, Mock> _mockObjects = [];

    public void CreateMock<T>(string key = EmptyKey) where T : class
    {
        ServiceKey serviceKey = new(typeof(T), key);

        _mockObjects
            .Should()
            .NotContainKey(serviceKey, "the mock object can only be created once");

        _mockObjects[serviceKey] = new Mock<T>(MockBehavior.Strict);
    }

    public T Get<T>(string key = EmptyKey) where T : class
    {
        ServiceKey serviceKey = new(typeof(T), key);

        _mockObjects
            .Should()
            .ContainKey(serviceKey, "the mock object should have already been created");

        return ((Mock<T>)_mockObjects[serviceKey]).Object;
    }

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

    public void VerifyMocks() => Mock.VerifyAll([.. _mockObjects.Values]);
}