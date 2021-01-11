using School.Financial.Dac;
using School.Financial.Models;
using System.Linq;

namespace School.Financial.Services.Impl
{
    public class IdentityService : IIdentityService
    {
        private readonly ISchoolDac schoolDac;
        private readonly ISchoolYearDac schoolYearDac;

        public dynamic GetUser()
        {
            return new
            {
                Name = "นางสาวภานุมาศ ศรีกัน",
                Position = "ครู"
            };
        }

        public IdentityService(
            ISchoolDac schoolDac,
            ISchoolYearDac schoolYearDac
            )
        {
            this.schoolDac = schoolDac;
            this.schoolYearDac = schoolYearDac;
        }

        public SchoolData GetCurrentSchool()
        {
            //TODO: get current school
            return schoolDac.Get().FirstOrDefault();
        }

        public SchoolConfig GetConfig()
        {
            return new SchoolConfig
            {
                SchoolYear = schoolYearDac.Get().OrderBy(x => x.StartDate).LastOrDefault().Year,
            };
        }
    }
}
