using System;
using System.ComponentModel.DataAnnotations;

namespace MazzaWebSite.Attribute
{
    public class DataValidation : ValidationAttribute
    {
        private readonly DataType _date;
        public DataValidation(DataType date)
        {
            _date = date;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime bday = DateTime.Parse(value.ToString());
                if (bday >= DateTime.Now)
                    return new ValidationResult(Resources.Account.IncorrectDate);
                else
                {
                    DateTime today = DateTime.Today;
                    int age = today.Year - bday.Year;
                    if (age < 16)
                        return new ValidationResult(Resources.Account.Underage);
                }
            }
            else
                return new ValidationResult(Resources.Account.AgeErrorMessage);

            return null;
        }
    }
}