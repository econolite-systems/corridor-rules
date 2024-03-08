// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Models.CorridorRules;
using Econolite.Ode.Models.CorridorRules.Status.Db;
using Econolite.Ode.Persistence.Common.Repository;

namespace Econolite.Ode.Repository.WeatherResponsiveCorridorRules
{
    public interface IWeatherResponsiveCorridorRulesRepository : IRepository<WeatherResponsiveCorridors, Guid>
    {
        Task<IEnumerable<WeatherResponsiveCorridors>> GetAllAsync(Guid tenantId);
        Task<WeatherResponsiveCorridors?> GetByCorridorIdAsync(Guid id);
        void DeleteWeatherResponsiveCorridorRule(Guid corridorId, Guid ruleId);
    }
}