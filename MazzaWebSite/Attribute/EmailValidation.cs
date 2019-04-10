using System.ComponentModel.DataAnnotations;

namespace MazzaWebSite.Attribute
{
    public class EmailValidation : ValidationAttribute
    {
        private readonly DataType _emailAddress;
        public EmailValidation(DataType emailAddress)
        {
            _emailAddress = emailAddress;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                
            }

            else
                return new ValidationResult(Resources.Account.AgeErrorMessage);

            return null;
        }
    }
}