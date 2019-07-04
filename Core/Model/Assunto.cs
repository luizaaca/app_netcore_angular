using Core.Model.Base;

namespace Core.Model
{
    public class Assunto : EntityBase<int>, IAggregateRoot
    {
        public string Descricao { get; set; }
    }    
}
