// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Auditing;
using Econolite.Ode.Auditing.Extensions;
using Econolite.Ode.Authorization;
using Econolite.Ode.Models.CorridorRules;
using Econolite.Ode.Models.CorridorRules.Db;
using Econolite.Ode.Models.CorridorRules.Status.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Econolite.Ode.Service.WeatherResponsiveCorridorRules
{
    [ApiController]
    [Route("weather-responsive-corridor-rules")]
    [AuthorizeOde(MoundRoadRole.ReadOnly)]
    public class WeatherResponsiveCorridorRulesController : ControllerBase
    {
        private readonly IWeatherResponsiveCorridorRulesService _weatherResponsiveCorridorRulesService;
        private readonly ILogger<WeatherResponsiveCorridorRulesController> _logger;
        private readonly Guid _tenantId;
        private readonly IAuditCrudScopeFactory _auditCrudScopeFactory;
        private readonly string _auditEventType;

        /// <summary>
        /// Constructs a Weather Responsive Corridor Rules controller
        /// </summary>
        /// <param name="weatherResponsiveCorridorRulesService">A weather responsive corridor rules service instance</param>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="auditCrudScopeFactory"></param>
        public WeatherResponsiveCorridorRulesController(
            IWeatherResponsiveCorridorRulesService weatherResponsiveCorridorRulesService,
            IConfiguration config,
            ILogger<WeatherResponsiveCorridorRulesController> logger,
            IAuditCrudScopeFactory auditCrudScopeFactory)
        {
            _weatherResponsiveCorridorRulesService = weatherResponsiveCorridorRulesService;
            _tenantId = Guid.Parse(config["TenantId"] ?? throw new NullReferenceException("TenantId missing from config")); //Guid.Parse(httpContextAccessor.HttpContext.User.Claims.Single(claim => claim.Type == "tenantId").Value);
            _logger = logger;
            _auditCrudScopeFactory = auditCrudScopeFactory;
            _auditEventType = SupportedAuditEventTypes.AuditEventTypes[AuditEventType.CorridorRules].Event;
        }

        /// <summary>
        /// Get all Weather Responsive Corridors
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<WeatherResponsiveCorridorDto>))]
        public async Task<ActionResult<IEnumerable<WeatherResponsiveCorridorDto>>> Index()
        {
            var wrCorridorConfigs = await _weatherResponsiveCorridorRulesService.GetAllAsync(_tenantId);
            return Ok(wrCorridorConfigs);
        }

        /// <summary>
        /// Get a specified Weather Responsive Corridor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(WeatherResponsiveCorridorDto))]
        public async Task<ActionResult<WeatherResponsiveCorridorDto>> Get(Guid id)
        {
            var config = await _weatherResponsiveCorridorRulesService.GetByIdAsync(id);

            if (config == null) return NotFound();

            return Ok(config);
        }

        /// <summary>
        /// Add a rule to a weather responsive corridor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("{id}/rule")]
        [ProducesResponseType(200, Type = typeof(WeatherResponsiveCorridorRulesDto))]
        [AuthorizeOde(MoundRoadRole.Contributor)]
        public async Task<IActionResult> Post(Guid id, [FromBody] WeatherResponsiveCorridorRulesDocument value)
        {
            _logger.LogDebug("Adding {@}", value);
            var scope = _auditCrudScopeFactory.CreateAddAsync(_auditEventType, () => value);
            await using (await scope)
            {
                if (value == null) return BadRequest();

                try
                {
                    var created = await _weatherResponsiveCorridorRulesService.AddWeatherResponsiveCorridorRuleAsync(id, value);
                    return Ok(created);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to add rule {@}", new { CorridorId = id, Rule = value });
                    return StatusCode(500, ex.Message);
                }
            }
        }

        /// <summary>
        /// Update a rule for a weather responsive corridor
        /// </summary>
        /// <param name="newCorridorId">The corridor the rule will be saved to. If the old corridor ID is different, the rule will be saved to the new corridor ID and deleted from the old corridor ID</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{newCorridorId}/rule")]
        [ProducesResponseType(200, Type = typeof(WeatherResponsiveCorridorRulesDto))]
        [AuthorizeOde(MoundRoadRole.Contributor)]
        public async Task<IActionResult> Put(Guid newCorridorId, [FromBody] WeatherResponsiveCorridorRulesUpdateDocument value)
        {
            _logger.LogDebug("Updating {@}", value);
            var scope = _auditCrudScopeFactory.CreateUpdateAsync(_auditEventType, () => value);
            await using (await scope)
            {
                if (value == null) return BadRequest();

                try
                {
                    var updated = await _weatherResponsiveCorridorRulesService.UpdateWeatherResponsiveCorridorRuleAsync(newCorridorId, value);
                    return Ok(updated);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to update rule {@}", new { CorridorId = newCorridorId, Rule = value });
                    return StatusCode(500, ex.Message);
                }
            }
        }

        /// <summary>
        /// Delete a rule from a weather responsive corridor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        [HttpDelete("{id}/rule/{ruleId}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [AuthorizeOde(MoundRoadRole.Administrator)]
        public async Task<IActionResult> Delete(Guid id, Guid ruleId)
        {
            _logger.LogDebug("Deleting {@}", ruleId);
            var scope = _auditCrudScopeFactory.CreateDeleteAsync(_auditEventType, ruleId.ToString);
            await using (await scope)
            {
                var deleted = false;
                try
                {
                    deleted = await _weatherResponsiveCorridorRulesService.DeleteWeatherResponsiveCorridorRuleAsync(id, ruleId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to delete rule {@}", new { CorridorId = id, RuleId = ruleId });
                }
                if (!deleted) return NotFound();
                return Ok();
            }
        }
    }
}
