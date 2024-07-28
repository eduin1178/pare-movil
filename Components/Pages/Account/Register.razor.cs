using Microsoft.AspNetCore.Components;
using MudBlazor;
using PARE.Models;
using PARE.WebApiClient;
using PAREMAUI.Essentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAREMAUI.Components.Pages.Account
{
    public partial class Register : ComponentBase
    {
        [Inject] UsersApiService UsersApiService { get; set; }
        [Inject] CitiesApiService citiesApiService { get; set; }
        [Inject] AuthService AuthService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        public RegisterModel RegisterModel { get; set; } = new();
        public List<CityModel> CitiesList { get; set; }
        public List<DepartmentCitiesModel> DepartmentsCitiesList { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadCities();
        }
        protected async Task LoadCities()
        {
            //Colombia
            var countryCode = "03bc3ec7-2769-40af-9d32-2a91ae7d9944";
            var res = await citiesApiService.Cities(countryCode);
            CitiesList = res.IsSuccess ? res.Data : new List<CityModel>();
            if (CitiesList.Count != 0)
            {
                DepartmentsCitiesList = CitiesList
               .GroupBy(c => new { c.Department.Id, c.Department.Code, c.Department.Name, c.Department.Sort })
               .Select(g => new DepartmentCitiesModel
               {
                   Id = g.Key.Id,
                   Sort = g.Key.Sort,
                   Code = g.Key.Code,
                   Name = g.Key.Name,
                   Cities = g.OrderBy(x => x.Name).ToList()
               }).OrderBy(x => x.Name)
               .ToList();

                RegisterModel.CityId = DepartmentsCitiesList.FirstOrDefault().Cities.FirstOrDefault().Id;
            }
            else
            {
                DepartmentsCitiesList = new List<DepartmentCitiesModel>();
            }
        }
        protected async Task RegisterAccount()
        {
            RegisterModel.Code = Guid.NewGuid().ToString();
            var res = await UsersApiService.Register(RegisterModel);
            if (res.IsSuccess)
            {
                Snackbar.Add("Registro completado.", Severity.Success);

                LoginModel logInModel = new LoginModel()
                {
                    EmailOrPhoneNumber = RegisterModel.Email,
                    Password = RegisterModel.Password
                };

                var loginRes = await AuthService.Login(logInModel);
                if (loginRes.IsSuccess)
                {
                    _navigationManager.NavigateTo("/");
                }
                else
                {
                    Snackbar.Add(res.Message, Severity.Error);
                }
            }
            else
            {
                Snackbar.Add(res.Message, Severity.Error);
            }
        }
    }
}
