using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TengDa.Wpf;

namespace Zopoise.Scada.App
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
            if (Battery.Id > 0)
            {
                Context.InsulationContext.DataLogs.Add(new InsulationDataLog()
                {
                    User = AppCurrent.User,
                    Battery = Battery,
                    Resistance = 0,
                    Voltage = 0,
                    TimeSpan = 0,
                    Temperature = 0,
                    IsUploaded = false,
                    DateTime = DateTime.Now
                });
                Context.InsulationContext.SaveChanges();

                AppCurrent.YieldNow.FeedingOK++;

                Battery = new Battery();
            }

            if (Resistance > -1)
            {
                var data = Context.InsulationContext.DataLogs.Where(d => d.Resistance == 0)
                    .OrderByDescending(d => d.DateTime).Take(3).OrderBy(d => d.DateTime).FirstOrDefault();
                if (data != null)
                {
                    data.Resistance = Resistance;
                    data.Voltage = Voltage;
                    data.TimeSpan = TimeSpan;
                    Context.InsulationContext.SaveChanges();
                }
                Resistance = -1;
            }

            if (Temperature > -1)
            {
                var data = Context.InsulationContext.DataLogs.Where(d => d.Temperature == 0)
                    .OrderByDescending(d => d.DateTime).Take(3).OrderBy(d => d.DateTime).FirstOrDefault();
                if (data != null)
                {
                    data.Temperature = Temperature;
                    Context.InsulationContext.SaveChanges();

                    if (data.Id > 0)
                    {
                        MES.Upload(data.Id);
                    }
                }
                Temperature = -1;

            }

        }
    }


    public class InsulationDataLog : Record
    {

        public int BatteryId { get; set; }

        public float Resistance { get; set; }

        public float Voltage { get; set; }

        public float TimeSpan { get; set; }

        public float Temperature { get; set; }

        public bool IsUploaded { get; set; }
    }
}
