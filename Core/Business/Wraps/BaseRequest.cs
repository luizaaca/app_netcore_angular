namespace Core.Business.Wraps
{
    public class BaseRequest<T>
    {
        public BaseRequest() { }
        public BaseRequest(T value) => Value = value;

        public T Value { get; set; }
    }
}
