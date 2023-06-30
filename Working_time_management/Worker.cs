using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Working_time_management
{
    class Worker
    {
            public string ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

        public override string ToString()
        {
            return LastName + ", " + FirstName + ", " + ID;
        }
    }
}
