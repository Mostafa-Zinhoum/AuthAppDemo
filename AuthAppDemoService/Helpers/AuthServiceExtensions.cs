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
using System.Reflection;

namespace AuthAppDemoService.Helpers
{
    public static class AuthServiceExtensions
    {
        public static void AddAuthSerices(this IServiceCollection services,
            JwtInfo jwtInfo, IServiceCollection DBServices)
        {
            // Register AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register IObjectMapper
            services.AddScoped<IObjectMapper, ObjectMapper>();

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
            //return services;
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            var serviceType = typeof(IApplicationService);
            var assembly = Assembly.GetExecutingAssembly();

            var implementations = assembly.GetTypes()
            .Where(t => serviceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            foreach (var implementation in implementations)
            {
                var interfaceTypes = implementation.GetInterfaces();
                //.FirstOrDefault(i => i != serviceType && serviceType.IsAssignableFrom(i));

                var interfaceType = interfaceTypes.FirstOrDefault(i => i != serviceType && i.Name.EndsWith(implementation.Name));//.FirstOrDefault(i => i != serviceType && serviceType.IsAssignableFrom(i));
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, implementation);
                }
                else
                {
                    services.AddScoped(serviceType, implementation);
                }
            }

        }
    }
}



