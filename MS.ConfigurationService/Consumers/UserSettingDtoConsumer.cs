using MassTransit;
using MS.ConfigurationService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ConfigurationService.Consumers
{
    public class UserSettingDtoConsumer
        : IConsumer<UserSettingDto>
    {
        ILogger<UserSettingDtoConsumer> _logger;

        public UserSettingDtoConsumer(ILogger<UserSettingDtoConsumer> logger) 
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<UserSettingDto> context)
        {
            _logger.LogInformation("User Settings {SettingsKey}", context.Message.SettingKey);

            return Task.CompletedTask;
        }
    }
}
