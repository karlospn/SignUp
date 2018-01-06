using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignUp.Consumer.ElasticSearch.Configuration
{
    public class ESOptions
    {
        public string Uri { get; set; }
        public string DefaultIndex { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
