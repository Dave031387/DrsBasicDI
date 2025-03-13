namespace DrsBasicDI;

internal interface IMockServiceLocater : IServiceLocater
{
    void CreateMock<T>(string key = EmptyKey) where T : class;

    public Mock<T> GetMock<T>(string key = EmptyKey) where T : class;

    void VerifyMocks();
}