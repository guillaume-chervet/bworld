namespace Demo.Business.Command
{
    public class UserInput<T>
    {
        public string UserId { get; set; }
        public T Data { get; set; }
    }
}