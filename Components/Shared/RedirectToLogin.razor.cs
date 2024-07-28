using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAREMAUI.Components.Shared
{
    public partial class RedirectToLogin : ComponentBase
    {
        protected override void OnInitialized()
        {
            _navigationManager.NavigateTo("/account/login");
        }
    }
}
