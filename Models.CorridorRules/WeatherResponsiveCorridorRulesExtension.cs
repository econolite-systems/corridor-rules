// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Models.CorridorRules.Db;
using Econolite.Ode.Models.CorridorRules.Status.Db;

namespace Econolite.Ode.Models.CorridorRules
{
    public static partial class WeatherResponsiveCorridorRulesExtension
    {
        public static WeatherResponsiveCorridorRulesDto AdaptWeatherResponsiveCorridorRulesDocumentToDto(this WeatherResponsiveCorridorRulesDocument corridorRulesMessage)
        {
            return new WeatherResponsiveCorridorRulesDto() 
            {
                Name = corridorRulesMessage.Name,
                Id = corridorRulesMessage.Id,
                WeatherStatus = corridorRulesMessage.WeatherStatus,
                TimingPlan = corridorRulesMessage.TimingPlan,
                Edaptive = corridorRulesMessage.Edaptive,
                SpeedHarmonizationEnabled = corridorRulesMessage.SpeedHarmonizationEnabled,
                SpeedHarmonizationValue = corridorRulesMessage.SpeedHarmonizationValue
            };
        }

        public static WeatherResponsiveCorridorDto AdaptWeatherResponsiveCorridorDocumentToDto(this WeatherResponsiveCorridors weatherResponsiveCorridorMessage)
        {
            return new WeatherResponsiveCorridorDto()
            {
                TenantId = weatherResponsiveCorridorMessage.TenantId,
                Id = weatherResponsiveCorridorMessage.Id,
                Name = weatherResponsiveCorridorMessage.Name,
                WeatherResponsiveRules = weatherResponsiveCorridorMessage.WeatherResponsiveRules
            };
        }

        public static WeatherResponsiveCorridorRulesDocument AdaptWeatherResponsiveUpdateRuleDocumentToRuleDocument(this WeatherResponsiveCorridorRulesUpdateDocument weatherResponsiveUpdateRuleMessage)
        {
            return new WeatherResponsiveCorridorRulesDocument()
            {
                Id = weatherResponsiveUpdateRuleMessage.Id,
                Name = weatherResponsiveUpdateRuleMessage.Name,
                WeatherStatus = weatherResponsiveUpdateRuleMessage.WeatherStatus,
                TimingPlan = weatherResponsiveUpdateRuleMessage.TimingPlan,
                Edaptive = weatherResponsiveUpdateRuleMessage.Edaptive,
                SpeedHarmonizationEnabled = weatherResponsiveUpdateRuleMessage.SpeedHarmonizationEnabled,
                SpeedHarmonizationValue = weatherResponsiveUpdateRuleMessage.SpeedHarmonizationValue
            };
        }

        //public static WeatherResponsiveCorridorRuleAddDto AdaptWeatherResponsiveCorridorAddDocumentToDto(this WeatherResponsiveCorridorRuleAddDocument weatherResponsiveAddRuleMessage)
        //{
        //    return new WeatherResponsiveCorridorRuleAddDto()
        //    {
        //        CorridorId = weatherResponsiveAddRuleMessage.CorridorId,
        //        Id = weatherResponsiveAddRuleMessage.Id,
        //        WeatherStatus = weatherResponsiveAddRuleMessage.WeatherStatus,
        //        TimingPlan = weatherResponsiveAddRuleMessage.TimingPlan,
        //        Edaptive = weatherResponsiveAddRuleMessage.Edaptive,
        //        SpeedHarmonizationEnabled = weatherResponsiveAddRuleMessage.SpeedHarmonizationEnabled,
        //        SpeedHarmonizationValue = weatherResponsiveAddRuleMessage.SpeedHarmonizationValue
        //    };
        //}
    }
}
