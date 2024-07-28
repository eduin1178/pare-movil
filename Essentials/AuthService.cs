using Microsoft.AspNetCore.Components.Authorization;
using PARE.Models;
using PARE.WebApiClient;
using PAREMAUI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAREMAUI.Essentials
{
    public class AuthService
    {
        readonly Client _client;
        readonly UsersApiService _usersApiService;
        readonly AuthenticationStateProvider _authenticationStateProvider;
        public AuthService(UsersApiService usersApiService, Client client, AuthenticationStateProvider authStateProvider)
        {
            _usersApiService = usersApiService;
            _client = client;
            _authenticationStateProvider = authStateProvider;
        }
        public async Task<TypedResult<LoginResponseModel>> Login(LoginModel logInModel)
        {
            var result = new TypedResult<LoginResponseModel>();
            var res = await _usersApiService.Login(logInModel);

            if (!res.IsSuccess)
            {
                result.Message = res.Message;
                result.IsSuccess = false;
                return res;
            }

            var authenticatedUser = res.Data;
            var token = authenticatedUser.Token;

            _client.SetAutorization(token);

            await ((CustomAuthStateProvider)_authenticationStateProvider).SignIn(authenticatedUser);

            result.IsSuccess = true;
            result.Message = "Usuario autenticado correctamente";
            result.Data = authenticatedUser;
            return result;
        }
    }
}
