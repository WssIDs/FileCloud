using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
