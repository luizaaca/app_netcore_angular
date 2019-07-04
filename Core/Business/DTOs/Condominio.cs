namespace Core.Business.DTOs
{
    public class Condominio
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Administradora Administradora { get; set; }
        public Usuario Responsavel { get; set; }
    }
}
