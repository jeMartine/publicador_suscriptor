namespace sbBancoTurnos.src.models{
    public class Servicio
    {
        public string? id { get; set; }
        public required string Codigo { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }

        public string TopicMQTT { get; set; }
    }
}