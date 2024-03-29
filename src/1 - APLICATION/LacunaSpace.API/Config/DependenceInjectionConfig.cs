﻿using GameMasterEnterprise.Domain.Intefaces;
using LacunaSpace.Domain.Intefaces;
using LacunaSpace.Domain.Notificacoes;
using LacunaSpace.Service;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using System.Net.Http;

namespace LacunaSpace.API.Config
{
    public static class DependenceInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<HttpClient, HttpClient>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();




            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<ILacunaSpaceService, LacunaSpaceService>();

            return services;
        }
    }
}
