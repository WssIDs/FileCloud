using System.ComponentModel.DataAnnotations;

namespace FileCloud.Shared.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EqualValueAttribute : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private object DesiredValue { get; set; }

        public EqualValueAttribute(string propertyName, object desiredvalue)
        {
            PropertyName = propertyName;
            DesiredValue = desiredvalue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();
            var propertyValue = type.GetProperty(PropertyName).GetValue(instance, null);

            if (DesiredValue.Equals(propertyValue))
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });
            }
            return ValidationResult.Success;
        }
    }
}
