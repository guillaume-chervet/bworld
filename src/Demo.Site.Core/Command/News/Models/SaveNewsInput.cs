using System.Collections.Generic;
using Demo.Business.Command.Free.Models;

namespace Demo.Business.Command.News.Models
{
    public class SaveNewsInput : SaveFreeInput
    {
        public string DisplayMode { get; set; }
        public int NumberItemPerPage { get; set; } = 10;
    }
}