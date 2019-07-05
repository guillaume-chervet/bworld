namespace Demo.Domain.Contact
{
    public class ContactCheckDomain
    {
        public string Domain { get; set; }
        public bool? Owner { get; set; }
        public bool? Admin { get; set; }
        public bool? Bill { get; set; }
        public bool? Tech { get; set; }
    }
}