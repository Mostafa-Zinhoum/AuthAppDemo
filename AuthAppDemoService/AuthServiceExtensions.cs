using AuthAppDemoLog;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AuthAppDemoService.Basics.Impelmentation;
using AuthAppDemoService.Basics.Interfaces;

namespace AuthAppDemoService
{
    public static class AuthServiceExtensions
    {
        public static IServiceCollection AddAuthSerices(this IServiceCollection services,
            JwtInfo jwtInfo, IServiceCollection DBServices)
        {
            
            services.AddSingleton<IAuthorize, Authorize>();
            services.AddSingleton<IUserService, UserService>();
            services.AddScoped<IWorxDB>(x =>
                    new WorxDB(services, DBServices)
                );
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IContextsFactor, ContextsFactor>();

            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
                {
                    option.SaveToken = true;
                    option.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtInfo.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtInfo.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtInfo.SigningKey))
                    };
                    option.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = async context =>
                        {
                            if (context.Request.Headers.ContainsKey("Authorization"))
                            {
                                var encryptedToken = context.Request.Headers["Authorization"].ToString();
                                encryptedToken = encryptedToken.Replace("Bearer ", "");
                                var decryptedToken = Encryption.AIS_Decryption(encryptedToken, jwtInfo.SigningKey);
                                context.Token = decryptedToken;
                                context.Request.Headers["Authorization"] = string.Concat("Bearer ", decryptedToken);

                            }
                        }
                    };
                }

                );
            return services;
        }
    }
}
