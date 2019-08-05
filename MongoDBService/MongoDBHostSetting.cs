using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDBService
{
    public class MongoDBHostSetting
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

    }
}
