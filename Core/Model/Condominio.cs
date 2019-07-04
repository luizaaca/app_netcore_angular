using Core.Model.Base;

namespace Core.Model
{
    public class Condominio : EntityBase<int>, IAggregateRoot
    {
        public string Nome { get; set; }
        public Administradora Administradora { get; set; }
        public Usuario Responsavel { get; set; }
    }
}
