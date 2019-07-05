using Core.Model;

namespace Core.Business.DTOs
{
    public class Comunicado
    {
        public int IdUsuario { get; set; }
        public string Assunto { get; set; }
        public string Mensagem { get; set; }
    }
}
