using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data.Azure
{
    public class ServiceBus
    {


    /*    void Send()
        {
            // Configure Topic Settings.
            TopicDescription td = new TopicDescription("TestTopic");
            td.MaxSizeInMegabytes = 5120;
            td.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);

            // Create a new Topic with custom settings.
            string connectionString =
                CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            var namespaceManager =
                NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.TopicExists("TestTopic"))
            {
                namespaceManager.CreateTopic(td);
            }
        }*/

    }
}
