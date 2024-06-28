using AuthAppDemoLog;
using AuthAppDemoService;
using AuthAppDemoService.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthAppDemo.Controllers
{
    [Route("api/[Action]")]
    [ApiController]
    [LogAspect]

    public class AuthGateController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthorize authService;       
        private readonly JwtInfo jwtInfo;
        private readonly ILogger<AuthGateController> _logger;
        public AuthGateController(IUserService _userService, IAuthorize _authService, JwtInfo _jwtInfo, ILogger<AuthGateController> logger)
        {
            userService = _userService;
            jwtInfo = _jwtInfo;
            authService = _authService;
            _logger = logger;
        }

        [HttpPost(Name ="Login")]
        
        public async Task<LoginDto> Login(LoginRequest param) 
        {

            var customLogState = new CustomLogState
            {
                Message = "This is a log message with additional info.",
                RequestHeaders = Request.Headers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString())
            };

            _logger.Log(LogLevel.Error, new EventId(1, "Auth App Login"), customLogState, null, (state, ex) =>
            {
                var logState = state as CustomLogState;
                return $"{logState.Message} | Headers: {string.Join(", ", logState.RequestHeaders.Select(kvp => $"{kvp.Key}: {kvp.Value}"))}";
            });



            var userInfo = await userService.Login(param);
            if (userInfo != null)
            {
                userInfo.AccessToken = authService.CreateAccessToken(new TokenInfoDto { UserId = userInfo.UserId,UserName = userInfo.UserName});
            }

            return userInfo;
        }


        [Authorize]
        [HttpPost(Name = "GetUser")]
        
        public async Task<LoginDto> GetUser(GetUserRequest param)
        {
            /*
            _logger.LogInformation("This is an informational log message.");
            _logger.LogWarning("This is a warning log message.");
            _logger.LogError("This is an error log message.");
            */
            var userInfo = await userService.GetUser(param);
            return userInfo;
        }
    }
}
