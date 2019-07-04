using Core.Model.Base;

namespace Core.Model
{
    public class Administradora : EntityBase<int>, IAggregateRoot
    {
        public string Nome { get; set; }
    }
}
