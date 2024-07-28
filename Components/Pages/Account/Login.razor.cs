using Microsoft.AspNetCore.Components;
using MudBlazor;
using PARE.Models;
using PAREMAUI.Essentials;
using PAREMAUI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAREMAUI.Components.Pages.Account
{
    public partial class Login : ComponentBase
    {
        [Inject] AuthService AuthService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        public LoginModel LoginModel { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {

        }

        protected async Task LogIn()
        {
            var res = await AuthService.Login(LoginModel);
            if (res.IsSuccess)
            {
                _navigationManager.NavigateTo("/");
            }
            else
            {
                Snackbar.Add(res.Message, Severity.Error);
            }
        }
    }
}
