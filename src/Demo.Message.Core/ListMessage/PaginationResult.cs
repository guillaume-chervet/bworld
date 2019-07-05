using System;

namespace Demo.Business.Command.Contact.Message.Models.ListMessage
{
    /// <summary>
    ///     Partially count number of messages from EventHubs
    /// </summary>
    public class PaginationResult
    {
        public DateTime? DatePrevious { get; set; }
        public long NumberNext { get; set; }
        public long NumberPrevious { get; set; }
        public DateTime? DateNext { get; set; }
    }
}