// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

namespace Econolite.Ode.Models.CorridorRules.Status.Db
{
    public partial record WeatherResponsiveCorridorRulesUpdateDocument
    {
        //The corridor Id the rule was last assigned to
        public Guid OldCorridorId { get; set; } = Guid.Empty;
        public Guid Id { get; set; } = Guid.Empty;
        public string? Name { get; set; }
        public CorridorRulesWeatherStateEnum WeatherStatus { get; set; }
        public int TimingPlan { get; set; }
        public bool Edaptive { get; set; }
        public bool SpeedHarmonizationEnabled { get; set; }
        public double? SpeedHarmonizationValue { get; set; }
    }
}
