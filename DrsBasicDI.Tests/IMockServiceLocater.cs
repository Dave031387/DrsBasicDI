namespace DrsBasicDI;

internal interface IMockServiceLocater
{
    public Mock<T> GetMock<T>(string key = "") where T : class;
}
