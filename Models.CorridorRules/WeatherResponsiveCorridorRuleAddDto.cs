// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Econolite.Ode.Models.CorridorRules
{
    public class WeatherResponsiveCorridorRuleAddDto
    {
        public Guid CorridorId { get; set; }
        public Guid Id { get; set; }
        public CorridorRulesWeatherStateEnum WeatherStatus { get; set; }
        public int TimingPlan { get; set; }
        public bool Edaptive { get; set; }
        public bool SpeedHarmonizationEnabled { get; set; }
        public double? SpeedHarmonizationValue { get; set; }
    }
}
