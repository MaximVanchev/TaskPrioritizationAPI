using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPrioritizationAPI.Infrastructure.Data.Constants
{
    public class DatabaseConstants
    {
        public const string Connection_String = "Server=192.168.1.2,1433;Database=Task_Prioritization_API_Container;User Id=sa;Password=Maxmen111;TrustServerCertificate=True;";
        public const int Task_Title_Max_Length = 50;
        public const int Task_Description_Max_Length = 200;
    }
}
