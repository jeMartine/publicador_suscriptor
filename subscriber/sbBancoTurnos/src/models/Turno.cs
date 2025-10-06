namespace sbBancoTurnos.src.models
{
    public class Turno
{
    public string? Id { get; set; }
    public required string Codigo { get; set; }              // Ejemplo: "TG01", "SD02", "TGA03"
    public required DateTime FechaCreacion { get; set; } = DateTime.Now;
    public required Cliente Cliente { get; set; }
    public required Servicio Servicio { get; set; }
    public required EstadoTurno Estado { get; set; } = EstadoTurno.EnEspera;

    // Genera el mensaje MQTT que se publicará
    public string ToMQTTMessage()
    {
        return System.Text.Json.JsonSerializer.Serialize(new
        {
            Codigo,
            Servicio = Servicio.Nombre,
            Cliente = Cliente.Nombre,
            Estado = Estado.ToString(),
            Fecha = FechaCreacion
        });
    }
}

public enum EstadoTurno
{
    EnEspera,
    Atendiendo,
    Finalizado
}

}


