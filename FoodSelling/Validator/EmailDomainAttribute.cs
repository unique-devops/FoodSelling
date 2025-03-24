using System.ComponentModel.DataAnnotations;

namespace FoodSelling.Validator
{
    public class EmailDomainAttribute : ValidationAttribute
    {
        private readonly string _allowedDomain;

        public EmailDomainAttribute(string allowedDomain)
        {
            _allowedDomain = allowedDomain;
        }

        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return true; // Allow nulls or empty strings
            }

            var email = value.ToString();
            return email.EndsWith($"@{_allowedDomain}", StringComparison.OrdinalIgnoreCase);
        }
    }
}
