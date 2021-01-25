namespace School.Financial.Dac
{
    public interface IUserInfoDac : IDataDac<Models.UserInfo>
    {
        Models.UserInfo Get(string username);
        Models.UserInfo Get(string username, string password);
    }
}
