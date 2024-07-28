using Microsoft.AspNetCore.Components;
using MudBlazor;
using PARE.Models;
using PARE.WebApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAREMAUI.Components.Pages.Account
{
    public partial class RecoverPassword : ComponentBase
    {
        [Inject] UsersApiService UsersApiService  { get; set; }
        [Inject] ISnackbar Snackbar  { get; set; }
        public RecoverPasswordModel RecoverPasswordModel { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            
        }

        private async Task Recover()
        {
            var res = await UsersApiService.RecoverPassword(RecoverPasswordModel);
            if (res.IsSuccess)
            {
                Snackbar.Add(res.Message, Severity.Success);
            }
            else
            {
                Snackbar.Add(res.Message, Severity.Error);
            }
        }
    }
}
