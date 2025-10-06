// defino mi entidad de cliente
namespace BancoTurnosApp.src.models{ 
    public class Cliente{
        public string? Id { get; set; }
        public required string Cedula { get; set; }          // Identificador único
        public required string Nombre { get; set; }
        public bool RequiereAsistencia { get; set; } // True si necesita atención prioritaria
    }
}

