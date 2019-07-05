namespace Demo.Domain.Domain
{
    public class ModelBase
    {
        public string ApiKey { get; set; }
    }

    public class ModelBase<T> : ModelBase
    {
        public T Data { get; set; }
    }
}