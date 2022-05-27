using Microsoft.AspNetCore.Components.Forms;

namespace FileCloudClient.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public class BootstrapValidationCssProvider : FieldCssClassProvider
    {
        private readonly string _valid = "is-valid";
        private readonly string _invalid = "is-invalid";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valid"></param>
        /// <param name="invalid"></param>
        public BootstrapValidationCssProvider(string invalid = "is-invalid", string valid = null)
        {
            if (valid == null)
            {
                valid = string.Empty;
            }

            _valid = valid;

            if(invalid != null)
            {
                _invalid = invalid;
            }
        }

        public override string GetFieldCssClass(EditContext editContext,
               in FieldIdentifier fieldIdentifier)
        {
            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            if (fieldIdentifier.FieldName == "Name")
            {
                return isValid ? _valid : _invalid;
            }
            else
            {
                if (editContext.IsModified(fieldIdentifier))
                {
                    return isValid ? $"modified {_valid}" : $"modified {_invalid}";
                }
                else
                {
                    return isValid ? _valid : _invalid;
                }
            }
        }
    }
}
