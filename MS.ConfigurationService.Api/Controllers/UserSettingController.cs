using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MS.ConfigurationService.Contracts;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using ZstdSharp.Unsafe;

namespace MS.ConfigurationService.Api.Controllers
{
    public class UserSetting
    {
        public ObjectId Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string SettingKey { get; set; } = string.Empty;
        public BsonDocument SettingValue { get; set; } = new BsonDocument();
    }

    [Route("[controller]")]
    [ApiController]
    public class UserSettingController : ControllerBase
    {
        private readonly IMongoCollection<UserSetting> _userSettings;

        public UserSettingController()
        {
            var client = new MongoClient("mongodb://db:27017");
            var database = client.GetDatabase("UserSettingsDB");
            _userSettings = database.GetCollection<UserSetting>("UserSettings");
        }

        [HttpGet("{userId}")]
        public ActionResult<List<UserSettingDto>> Get(string userId)
        {
            var filter = Builders<UserSetting>.Filter.Eq("UserId", userId);
            var settings = _userSettings.Find(filter).ToList();

            // Convert the BsonDocument to an ExpandoObject
            var result = settings.Select(setting => new UserSettingDto
            {
                //setting.Id,
                UserId = setting.UserId,
                SettingKey = setting.SettingKey,
                SettingValue = setting.SettingValue.ToDynamic()
            }).ToList();

            return result;
        }

        [HttpPost]
        public ActionResult<UserSetting> Create([FromServices] IBus bus,
            UserSettingDto userSetting)
        {
            string jsonString = JsonSerializer.Serialize(userSetting.SettingValue);
            BsonDocument settingValue = MongoDB.Bson.Serialization.BsonSerializer
                .Deserialize<BsonDocument>(jsonString);

            var setting = new UserSetting
            {
                UserId = userSetting.UserId,
                SettingKey = userSetting.SettingKey,
                SettingValue = settingValue
            };

            _userSettings.InsertOne(setting);

            bus.Publish(userSetting);

            return Ok();
        }
    }
}
