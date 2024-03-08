// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Econolite.Ode.Models.CorridorRules
{
    public sealed record WeatherResponsiveCorridorRuleAddDocument(
        Guid CorridorId,
        Guid Id,
        CorridorRulesWeatherStateEnum WeatherStatus,
        int TimingPlan,
        bool Edaptive,
        bool SpeedHarmonizationEnabled,
        double? SpeedHarmonizationValue
        );
}
