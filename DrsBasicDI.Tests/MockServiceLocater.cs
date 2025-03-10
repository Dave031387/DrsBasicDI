namespace DrsBasicDI;

internal class MockServiceLocater : IServiceLocater, IMockServiceLocater
{
    private readonly Dictionary<ServiceKey, object> _mockObjects = [];

    public T Get<T>(string key = "") where T : class
    {
        ServiceKey serviceKey = new(typeof(T), key);

        _mockObjects
            .Should()
            .ContainKey(serviceKey);

        return ((Mock<T>)_mockObjects[serviceKey]).Object;
    }

    public Mock<T> GetMock<T>(string key = "") where T : class
    {
        ServiceKey serviceKey = new(typeof(T), key);

        if (_mockObjects.TryGetValue(serviceKey, out object? value))
        {
            return (Mock<T>)value;
        }

        Mock<T> mock = new(MockBehavior.Strict);
        _mockObjects[serviceKey] = mock;

        return mock;
    }
}
