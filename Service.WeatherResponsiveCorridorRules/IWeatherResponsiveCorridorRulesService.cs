// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Models.CorridorRules;
using Econolite.Ode.Models.CorridorRules.Db;
using Econolite.Ode.Models.CorridorRules.Status.Db;

namespace Econolite.Ode.Service.WeatherResponsiveCorridorRules
{
    public interface IWeatherResponsiveCorridorRulesService
    {
        Task<IEnumerable<WeatherResponsiveCorridorDto>> GetAllAsync(Guid tenantId);
        Task<WeatherResponsiveCorridorDto?> GetByIdAsync(Guid id);
        Task<WeatherResponsiveCorridorRulesDto?> AddWeatherResponsiveCorridorRuleAsync(Guid id, WeatherResponsiveCorridorRulesDocument wrCorridorRuleAdd);
        Task<WeatherResponsiveCorridorRulesDto?> UpdateWeatherResponsiveCorridorRuleAsync(Guid corridorId, WeatherResponsiveCorridorRulesUpdateDocument wrCorridorRulesUpdate);
        Task<bool> DeleteWeatherResponsiveCorridorRuleAsync(Guid corridorId, Guid ruleId);
    }
}