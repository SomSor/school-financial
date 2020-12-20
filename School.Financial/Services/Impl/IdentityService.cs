using School.Financial.Dac;
using School.Financial.Models;
using System.Linq;

namespace School.Financial.Services.Impl
{
    public class IdentityService : IIdentityService
    {
        private readonly ISchoolDac schoolDac;

        public dynamic GetUser()
        {
            return new
            {
                Name = "นางสาวภานุมาศ ศรีกัน",
                Position = "ครู"
            };
        }

        public IdentityService(ISchoolDac schoolDac)
        {
            this.schoolDac = schoolDac;
        }

        public SchoolData GetCurrentSchool()
        {
            //TODO: get current school
            return schoolDac.Get().FirstOrDefault();
        }
    }
}
