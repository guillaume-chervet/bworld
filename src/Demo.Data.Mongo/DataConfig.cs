using System;
using System.Configuration;
using Microsoft.Extensions.Options;

namespace Demo.Data.Mongo
{
    public class DataConfig 
    {

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

    }
}