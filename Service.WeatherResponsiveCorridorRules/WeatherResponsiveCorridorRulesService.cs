// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Helpers.Exceptions;
using Econolite.Ode.Models.CorridorRules;
using Econolite.Ode.Models.CorridorRules.Db;
using Econolite.Ode.Models.CorridorRules.Status.Db;
using Econolite.Ode.Repository.WeatherResponsiveCorridorRules;
using Microsoft.Extensions.Logging;

namespace Econolite.Ode.Service.WeatherResponsiveCorridorRules
{
    public class WeatherResponsiveCorridorRulesService : IWeatherResponsiveCorridorRulesService
    {
        private readonly IWeatherResponsiveCorridorRulesRepository _weatherResponsiveCorridorRulesRepository;
        private readonly ILogger<WeatherResponsiveCorridorRulesService> _logger;

        public WeatherResponsiveCorridorRulesService(IWeatherResponsiveCorridorRulesRepository weatherResponsiveCorridorRulesRepository, 
            ILogger<WeatherResponsiveCorridorRulesService> logger)
        {
            _weatherResponsiveCorridorRulesRepository = weatherResponsiveCorridorRulesRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<WeatherResponsiveCorridorDto>> GetAllAsync(Guid tenantId)
        {
            var result = await _weatherResponsiveCorridorRulesRepository.GetAllAsync(tenantId);
            return result.Select(r => r.AdaptWeatherResponsiveCorridorDocumentToDto());
        }

        public async Task<WeatherResponsiveCorridorDto?> GetByIdAsync(Guid id)
        {
            var result = await _weatherResponsiveCorridorRulesRepository.GetByCorridorIdAsync(id);
            return result?.AdaptWeatherResponsiveCorridorDocumentToDto();
        }

        public async Task<WeatherResponsiveCorridorRulesDto?> AddWeatherResponsiveCorridorRuleAsync(Guid id, WeatherResponsiveCorridorRulesDocument wrCorridorRuleAdd)
        {
            var wrCorridorRule = await _weatherResponsiveCorridorRulesRepository.GetByCorridorIdAsync(id);
            if (wrCorridorRule is null)
            {
                return null;
            }

            wrCorridorRuleAdd.Id = Guid.NewGuid();

            wrCorridorRule.WeatherResponsiveRules ??= new List<WeatherResponsiveCorridorRulesDocument>();

            wrCorridorRule.WeatherResponsiveRules.Add(wrCorridorRuleAdd);
            _weatherResponsiveCorridorRulesRepository.Update(wrCorridorRule);
            var (success, errors) = await _weatherResponsiveCorridorRulesRepository.DbContext.SaveChangesAsync();
            if (!success && !string.IsNullOrWhiteSpace(errors)) throw new UpdateException(errors);

            return wrCorridorRuleAdd.AdaptWeatherResponsiveCorridorRulesDocumentToDto();
        }

        public async Task<WeatherResponsiveCorridorRulesDto?> UpdateWeatherResponsiveCorridorRuleAsync(Guid newCorridorId, WeatherResponsiveCorridorRulesUpdateDocument wrCorridorRulesUpdate)
        {
            //If the newCorridorId matches the corridorId of the rule being updated, the rule is being updated within the same corridor
            if(newCorridorId == wrCorridorRulesUpdate.OldCorridorId)
            {
                var wrCorridorRule = await _weatherResponsiveCorridorRulesRepository.GetByCorridorIdAsync(newCorridorId);
                if (wrCorridorRule is null)
                {
                    return null;
                }

                var index = wrCorridorRule.WeatherResponsiveRules.FindIndex(c => c.Id == wrCorridorRulesUpdate.Id);

                //don't save a null do an empty list; can't change it in the mapper extensions because those are auto generated
                wrCorridorRule.WeatherResponsiveRules ??= new List<WeatherResponsiveCorridorRulesDocument>();

                var wrUpdateSameCorridor = wrCorridorRulesUpdate.AdaptWeatherResponsiveUpdateRuleDocumentToRuleDocument();

                wrCorridorRule.WeatherResponsiveRules[index] = wrUpdateSameCorridor;

                _weatherResponsiveCorridorRulesRepository.Update(wrCorridorRule);

                var (success, errors) = await _weatherResponsiveCorridorRulesRepository.DbContext.SaveChangesAsync();
                if (!success && !string.IsNullOrWhiteSpace(errors)) throw new UpdateException(errors);

                return wrUpdateSameCorridor.AdaptWeatherResponsiveCorridorRulesDocumentToDto();
            }
            //If the newCorridorId does not match the corridorId of the rule being updated, the rule is being updated and moved to a new corridor.
            //In this case, the rule must be deleted from the old corridor and add to the new corridor
            else
            {
                try
                {
                    var success = await DeleteWeatherResponsiveCorridorRuleAsync(wrCorridorRulesUpdate.OldCorridorId, wrCorridorRulesUpdate.Id);

                    if (success)
                    {
                        var wrUpdateDifferentCorridor = wrCorridorRulesUpdate.AdaptWeatherResponsiveUpdateRuleDocumentToRuleDocument();
                        var addRuleToNewCorridor = await AddWeatherResponsiveCorridorRuleAsync(newCorridorId, wrUpdateDifferentCorridor);
                        return addRuleToNewCorridor;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return null;
                }
            }            
        }

        public async Task<bool> DeleteWeatherResponsiveCorridorRuleAsync(Guid corridorId, Guid ruleId)
        {
            try
            {
                _weatherResponsiveCorridorRulesRepository.DeleteWeatherResponsiveCorridorRule(corridorId, ruleId);
                var (success, _) = await _weatherResponsiveCorridorRulesRepository.DbContext.SaveChangesAsync();

                if (success)
                {
                    var corridors = await _weatherResponsiveCorridorRulesRepository.GetByIdAsync(corridorId);
                }

                return success;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }
    }
}
