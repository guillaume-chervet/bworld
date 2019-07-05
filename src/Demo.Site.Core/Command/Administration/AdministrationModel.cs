using System.Runtime.CompilerServices;

namespace Demo.Business.Command.Administration.Models
{
    public class AdministrationModel
    {
        public long NumberUnreadMessage { get; set; }

        public bool IdSeoWarning { get; set; }

        public int TotalSizeBytes { get; set; }
        public int MaxTotalSizeBytes { get; set; }
    }
}