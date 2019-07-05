using Core.Model.Base;

namespace Core.Model
{
    public class Usuario : EntityBase<int>, IAggregateRoot
    {
        public string Nome { get; set; }
        public int IdCondominio { get; set; }
        public Condominio Condominio { get; set; }
        public TipoUsuario Tipo { get; set; }
    }

    public enum TipoUsuario: byte
    {
        Morador = 1,
        Sindico = 2,
        Administradora = 3,
        Zelador = 4
    }
}
