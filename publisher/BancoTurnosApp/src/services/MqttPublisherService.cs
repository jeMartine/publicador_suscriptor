using MQTTnet;
using MQTTnet.Client;
using System.Text;
using System.Text.Json;
using BancoTurnosApp.src.models;
using MQTTnet.Protocol;

namespace BancoTurnosApp.src.services
{
    public class MqttPublisherService
    {
        private readonly IMqttClient _mqttClient;
        private readonly ILogger<MqttPublisherService> _logger;
        private readonly string _brokerAddress;

        public MqttPublisherService(ILogger<MqttPublisherService> logger)
        {
            _logger = logger;
            _brokerAddress = Environment.GetEnvironmentVariable("MQTT_BROKER") ?? "mqtt-broker";

            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
        }

        private async Task EnsureConnectedAsync()
        {
            if (!_mqttClient.IsConnected)
            {
                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer(_brokerAddress, 1883)
                    .WithCleanSession()
                    .Build();

                await _mqttClient.ConnectAsync(options);
                _logger.LogInformation($"Conectado al broker MQTT en {_brokerAddress}");
            }
        }

        public async Task PublishTurnoNuevoAsync(Turno turno)
        {
            await EnsureConnectedAsync();

            var payload = JsonSerializer.Serialize(turno);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("banco/turnos/nuevo")
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await _mqttClient.PublishAsync(message);
            _logger.LogInformation($"Publicado turno nuevo {turno.Codigo} en banco/turnos/nuevo");
        }

        public async Task PublishTurnoActualizadoAsync(Turno turno)
        {
            await EnsureConnectedAsync();

            var payload = JsonSerializer.Serialize(turno);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("banco/turnos/actualizado")
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await _mqttClient.PublishAsync(message);
            _logger.LogInformation($"Publicado turno actualizado {turno.Codigo} en banco/turnos/actualizado");
        }
    }
}
