// ... tus using
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using sbBancoTurnos.src.models;
using sbBancoTurnos.src.Data;

namespace sbBancoTurnos.src.services
{
    public class MqttSubscriberService : BackgroundService
    {
        private readonly ILogger<MqttSubscriberService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private IMqttClient _mqttClient;

        public MqttSubscriberService(ILogger<MqttSubscriberService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var brokerHost = Environment.GetEnvironmentVariable("MQTT_BROKER") ?? "mqtt-broker";
            var options = new MqttClientOptionsBuilder()
                .WithClientId("subscriber-service")
                .WithTcpServer(brokerHost, 1883)
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                try
                {
                    var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? Array.Empty<byte>());
                    _logger.LogInformation($"📥 Mensaje recibido en tópico {e.ApplicationMessage.Topic}: {payload}");

                    if (e.ApplicationMessage.Topic == "banco/turnos/nuevo")
                    {
                        var turno = JsonSerializer.Deserialize<Turno>(payload);

                        if (turno != null)
                        {
                            using var scope = _scopeFactory.CreateScope();
                            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                            // Evitar duplicados por Id del turno
                            var existeTurno = await db.Turnos.AnyAsync(t => t.Id == turno.Id);
                            if (!existeTurno)
                            {
                                // Buscar o crear el servicio
                                var servicioExistente = await db.Servicios
                                    .FirstOrDefaultAsync(s => s.Codigo == turno.Servicio.Codigo);

                                if (servicioExistente == null)
                                {
                                    servicioExistente = new Servicio
                                    {
                                        id = turno.Servicio.id,
                                        Codigo = turno.Servicio.Codigo,
                                        Nombre = turno.Servicio.Nombre,
                                        Descripcion = turno.Servicio.Descripcion,
                                        TopicMQTT = turno.Servicio.TopicMQTT
                                    };

                                    db.Servicios.Add(servicioExistente);
                                    await db.SaveChangesAsync();
                                    _logger.LogInformation($"🆕 Servicio {servicioExistente.Codigo} agregado a la DB");
                                }

                                // Asignar el servicio existente al turno
                                turno.Servicio = servicioExistente;

                                db.Turnos.Add(turno);
                                await db.SaveChangesAsync();
                                _logger.LogInformation($"✅ Turno {turno.Codigo} guardado en base de datos");
                            }
                            else
                            {
                                _logger.LogInformation($"⚠️ Turno {turno.Codigo} ya existe en la base de datos");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"❌ Error procesando mensaje MQTT: {ex.Message}");
                }
            };

            _mqttClient.ConnectedAsync += async e =>
            {
                _logger.LogInformation("🔗 Conectado al broker MQTT");
                await _mqttClient.SubscribeAsync("banco/turnos/nuevo");
                _logger.LogInformation("📡 Suscrito al tópico banco/turnos/nuevo");
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (!_mqttClient.IsConnected)
                    {
                        _logger.LogInformation($"🔄 Intentando conectar al broker {brokerHost}:1883...");
                        await _mqttClient.ConnectAsync(options, stoppingToken);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"❌ Conexión fallida al broker MQTT: {ex.Message}. Reintentando en 5 segundos...");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_mqttClient?.IsConnected == true)
            {
                await _mqttClient.DisconnectAsync();
                _logger.LogInformation("🔌 Desconectado del broker MQTT");
            }

            await base.StopAsync(cancellationToken);
        }
    }
}
