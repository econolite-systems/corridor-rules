// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Models.CorridorRules;
using Econolite.Ode.Models.CorridorRules.Db;
using Econolite.Ode.Models.CorridorRules.Status.Db;
using Econolite.Ode.Persistence.Mongo.Context;
using Econolite.Ode.Persistence.Mongo.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Econolite.Ode.Repository.WeatherResponsiveCorridorRules
{
    public class WeatherResponsiveCorridorRulesRepository : GuidDocumentRepositoryBase<WeatherResponsiveCorridors>, IWeatherResponsiveCorridorRulesRepository
    {
        private readonly IMongoCollection<WeatherResponsiveCorridors> _corridorCollection;

        public WeatherResponsiveCorridorRulesRepository(IMongoContext mongoContext, ILogger<WeatherResponsiveCorridorRulesRepository> logger, IConfiguration configuration) : base(mongoContext, logger)
        {
            _corridorCollection = mongoContext.GetCollection<WeatherResponsiveCorridors>(configuration["Collections:WeatherResponsiveCorridors"]);
        }

        public async Task<IEnumerable<WeatherResponsiveCorridors>> GetAllAsync(Guid tenantId)
        {
            var results = await ExecuteDbSetFuncAsync(collection => collection.FindAsync(i => i.TenantId == tenantId));
            return results?.ToList() ?? new List<WeatherResponsiveCorridors>();
        }

        public async Task<WeatherResponsiveCorridors?> GetByCorridorIdAsync(Guid id)
        {
            var idMatch = Builders<WeatherResponsiveCorridors>.Filter.Eq(s => s.Id, id);
            var cursor = await _corridorCollection.FindAsync(idMatch);
            var configs = await cursor.ToListAsync();
            return configs.FirstOrDefault();
        }

        public void DeleteWeatherResponsiveCorridorRule(Guid corridorId, Guid ruleId)
        {
            AddCommandFunc(collection =>
                async () =>
                {
                    var filter = Builders<WeatherResponsiveCorridors>.Filter.And(Builders<WeatherResponsiveCorridors>.Filter.Eq(c => c.Id, corridorId));
                    var update = Builders<WeatherResponsiveCorridors>.Update.PullFilter(n => n.WeatherResponsiveRules, Builders<WeatherResponsiveCorridorRulesDocument>.Filter.Eq(e => e.Id, ruleId));
                    var editResult = await collection.UpdateOneAsync(filter, update);
                    if (editResult is { IsModifiedCountAvailable: true } &&
                        editResult.MatchedCount != editResult.ModifiedCount)
                        throw new Exception(
                            $"Didn't remove {ruleId} from {editResult.MatchedCount - editResult.ModifiedCount} of {editResult.ModifiedCount} parents");
                }
            );
        }
    }
}
