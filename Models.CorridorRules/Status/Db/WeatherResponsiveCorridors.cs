// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Models.CorridorRules.Db;
using Econolite.Ode.Persistence.Common.Interfaces;

namespace Econolite.Ode.Models.CorridorRules.Status.Db
{
    public class WeatherResponsiveCorridors : IIndexedEntity<Guid>
    {
        public Guid TenantId { get; set; } = Guid.Empty;
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = String.Empty;
        public List<WeatherResponsiveCorridorRulesDocument> WeatherResponsiveRules { get; set; }
    }
    
}
