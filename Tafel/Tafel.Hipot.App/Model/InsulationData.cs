using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    public class InsulationData
    {
        public static Battery Battery = new Battery();

        public static float Resistance;

        public static float Voltage;

        public static float TimeSpan;

        public static float Temperature;

        public static void Insert()
        {
            if (Battery.Id < 0)
            {
                return;
            }

            if (Resistance == 0)
            {
                return;
            }

            if (Temperature == 0)
            {
                return;
            }

            Context.InsulationContext.DataLogs.Add(new InsulationDataLog()
            {
                User = AppCurrent.User,
                Battery = Battery,
                Resistance = Resistance,
                Voltage = Voltage,
                TimeSpan = TimeSpan,
                Temperature = Temperature,
                IsUploaded = false,
                DateTime = DateTime.Now
            });
            Context.InsulationContext.SaveChanges();

            AppCurrent.YieldNow.FeedingOK++;

            Battery = new Battery();
            Resistance = 0;
            Temperature = 0;

        }
    }


    public class InsulationDataLog : Record
    {

        public int BatteryId { get; set; }

        public Battery Battery { get; set; }

        public float Resistance { get; set; }

        public float Voltage { get; set; }

        public float TimeSpan { get; set; }

        public float Temperature { get; set; }

        public bool IsUploaded { get; set; }
    }
}
