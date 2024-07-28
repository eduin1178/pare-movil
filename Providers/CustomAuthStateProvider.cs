using Microsoft.AspNetCore.Components.Authorization;
using PARE.Models;
using PARE.WebApiClient;
using PAREMAUI.Essentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PARE.Core;
using System.IdentityModel.Tokens.Jwt;

namespace PAREMAUI.Providers
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AppState _appState;
        private readonly Client _client;
        private readonly UsersApiService _usersApiService;
        public LoginModel LoginModel { get; set; }
        public CustomAuthStateProvider(AppState appState, Client client, UsersApiService usersApiService)
        {
            _client = client;
            _usersApiService = usersApiService;
            _appState = appState;
        }
        /// <summary>
        /// Retomar la sesion del usuario
        /// </summary>
        /// <returns></returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await SecureStorage.GetAsync("accounttoken");
            if (token == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _client.SetAutorization(token);
            var res = await _usersApiService.Profile();
            if (!res.IsSuccess)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            var user = res.Data;

            //var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            //string nameIdentifier = jwt.Claims.FirstOrDefault(c => c.Type == "nameidentifier")?.Value;


            var identity = new ClaimsIdentity();
            try
            {
                if (token != null)
                {
                    var claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("UserTypeId", user.UserTypeId.ToString()),
                        new Claim(ClaimTypes.Role, user.UserTypeName),
                        new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                        new Claim(ClaimValueTypes.Email, user.Email)
                    };
                    identity = new ClaimsIdentity(claims, "Server authentication");
                    _appState.CurrentUser = user;
                }
                else
                {
                    var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
                    var authState = Task.FromResult(new AuthenticationState(anonymousUser));
                    NotifyAuthenticationStateChanged(authState);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        /// <summary>
        /// Accion de iniciar sesion
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task SignIn(LoginResponseModel userModel)
        {
            var identity = new ClaimsIdentity();
            await SecureStorage.SetAsync("accounttoken", userModel.Token);

            try
            {
                var claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, userModel.Id.ToString()),
                        new Claim(ClaimTypes.Name, userModel.FullName),
                        new Claim(ClaimTypes.Email, userModel.Email),
                        new Claim("UserTypeId", userModel.UserTypeId.ToString()),
                        new Claim(ClaimTypes.Role, userModel.UserTypeName),
                        new Claim(ClaimTypes.MobilePhone, userModel.PhoneNumber),
                        new Claim(ClaimValueTypes.Email, userModel.Email)
                    };

                identity = new ClaimsIdentity(claims, "Server authentication");
                var authenticatedUser = new ClaimsPrincipal(identity);
                var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

                _appState.CurrentUser = userModel;
                NotifyAuthenticationStateChanged(authState);
            }
            catch (Exception)
            {
            }

        }

        public async Task SignOut()
        {
            SecureStorage.Remove("accounttoken");
            _appState.CurrentUser = new LoginResponseModel() ;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
