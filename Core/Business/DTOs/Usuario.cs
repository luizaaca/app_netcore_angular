namespace Core.Business.DTOs
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Condominio Condominio { get; set; }
        public TipoUsuario Tipo { get; set; }
        public int IdCondominio { get; internal set; }
    }
}
