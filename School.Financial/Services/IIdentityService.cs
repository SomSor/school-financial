namespace School.Financial.Services
{
    public interface IIdentityService
    {
        dynamic GetUser();
        Models.SchoolData GetCurrentSchool();
        Models.SchoolConfig GetConfig();
    }
}
