using System;

namespace School.Financial.Helpers
{
    public class VatHelper
    {
        private const int VatPercentage = 7;
        private const int KeepDecimal = 2;

        public static decimal GetShopVatFromFullAmount(decimal amount)
        {
            return Math.Round((amount - (amount * VatPercentage) / (100 + VatPercentage)) / 100, KeepDecimal);
        }

        public static decimal GetPersonVatFromFullAmount(decimal amount)
        {
            return Math.Round(amount / 100, 2);
        }

        public static decimal GetCostBeforeIncludeVatForShop(decimal amount)
        {
            return amount - GetShopVatFromFullAmount(amount);
        }

        public static decimal GetCostBeforeIncludeVatForPerson(decimal amount)
        {
            return amount - GetPersonVatFromFullAmount(amount);
        }
    }
}
