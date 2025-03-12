namespace DrsBasicDI;

internal class MockServiceLocater : IServiceLocater, IMockServiceLocater
{
    private readonly Dictionary<ServiceKey, Mock> _mockObjects = [];

    public T Get<T>(string key = EmptyKey) where T : class
    {
        ServiceKey serviceKey = new(typeof(T), key);

        _mockObjects
            .Should()
            .ContainKey(serviceKey);

        return ((Mock<T>)_mockObjects[serviceKey]).Object;
    }

    public Mock<T> GetMock<T>(string key = EmptyKey) where T : class
    {
        ServiceKey serviceKey = new(typeof(T), key);

        if (_mockObjects.TryGetValue(serviceKey, out Mock? value))
        {
            return (Mock<T>)value;
        }

        Mock<T> mock = new(MockBehavior.Strict);
        _mockObjects[serviceKey] = mock;

        return mock;
    }

    public void VerifyMocks() => Mock.VerifyAll([.. _mockObjects.Values]);
}
