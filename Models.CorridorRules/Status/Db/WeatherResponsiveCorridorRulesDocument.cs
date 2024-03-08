// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

namespace Econolite.Ode.Models.CorridorRules.Db
{
    public partial record WeatherResponsiveCorridorRulesDocument
    {
        public string? Name { get; set; }
        public Guid Id { get; set; } = Guid.Empty;
        public CorridorRulesWeatherStateEnum WeatherStatus { get; set; }
        public int TimingPlan { get; set; }
        public bool Edaptive { get; set; }
        public bool SpeedHarmonizationEnabled { get; set; }
        public double? SpeedHarmonizationValue { get; set; }
    }    
        
}
