using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Working_time_management
{
    internal class holidayRequest
    {
        public bool checkedHoliday;

        public bool isHolidayChecked (bool holidayChecked)
        {
            this.checkedHoliday = holidayChecked;
            return checkedHoliday;
        }
    }
}
