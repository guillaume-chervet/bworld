namespace Demo.Domain.Domain
{
    public class DomainCreate
    {
        public string Admin { get; set; }
        public string Bill { get; set; }
        public int Duration { get; set; } = 1;
        public string Owner { get; set; }
        public string Tech { get; set; }
    }
}