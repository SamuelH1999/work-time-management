﻿using System;
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
            public string DateOfBirth { get; set; }
            public string Residence { get; set; }
            public string Password { get; set; }

        public override string ToString()
        {
            return LastName + ", " + FirstName + ", " + ID;
        }
        public string ToEdit()
        {
            return LastName + ", " + FirstName + ", " + DateOfBirth + ", " + Residence + ", " + Password + ", " + ID;
        }
    }
}
