namespace School.Financial.Dac
{
    public interface IEducationAreaDac : IDataDac<Models.EducationArea>
    {
        Models.EducationArea Get(string id);
    }
}
