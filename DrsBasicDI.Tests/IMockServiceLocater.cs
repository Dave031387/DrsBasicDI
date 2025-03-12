namespace DrsBasicDI;

internal interface IMockServiceLocater
{
    public Mock<T> GetMock<T>(string key = EmptyKey) where T : class;
}
