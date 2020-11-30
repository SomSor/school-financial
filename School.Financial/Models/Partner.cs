namespace School.Financial.Models
{
    public class Partner : DbModelBase
    {
        public string Name { get; set; }
        public string VatNumber { get; set; }
        public string Address { get; set; }
        public string PartnerType { get; set; }
    }

    public class PartnerType
    {
        public const string Normal = nameof(Normal);
        public const string Person = nameof(Person);
        public const string Shop = nameof(Shop);
    }
}
