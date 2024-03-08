// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Microsoft.Extensions.DependencyInjection;
using Econolite.Ode.Repository.WeatherResponsiveCorridorRules;

namespace Econolite.Ode.Service.WeatherResponsiveCorridorRules
{
    public static class WeatherResponsiveCorridorRulesServiceExtension
    {
        public static IServiceCollection AddWeatherResponsiveCorridorRulesSupport(this IServiceCollection services)
        {
            services.AddScoped<IWeatherResponsiveCorridorRulesService, WeatherResponsiveCorridorRulesService>();
            services.AddScoped<IWeatherResponsiveCorridorRulesRepository, WeatherResponsiveCorridorRulesRepository>();

            return services;
        }
    }
}
