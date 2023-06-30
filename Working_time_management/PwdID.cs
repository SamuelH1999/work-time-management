using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Working_time_management
{
    internal class PwdID
    {
        public string Id { get; set; }
        public string Pwd { get; set; }

        public override string ToString()
        {
            return Id + ", " + Pwd;
        }
    }

}

