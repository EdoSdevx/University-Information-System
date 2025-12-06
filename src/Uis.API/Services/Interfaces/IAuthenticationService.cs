using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs.Authentication;

namespace Uis.API.Services.Interfaces;
public interface IAuthenticationService
{
    Task<ResultService> RegisterAsync(RegisterRequest request);
    Task<ResultService<AuthenticationResponse>> LoginAsync(LoginRequest request);
    Task<ResultService<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);

}
