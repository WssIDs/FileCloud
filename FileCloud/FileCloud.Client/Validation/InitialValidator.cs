using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FileCloudClient.Validation
{
    public class InitialValidator : ComponentBase
    {
        // Get the EditContext from the parent component (EditForm)
        [CascadingParameter]
        private EditContext CurrentEditContext { get; set; }

        protected override void OnParametersSet()
        {
            CurrentEditContext?.Validate();
        }
    }
}
