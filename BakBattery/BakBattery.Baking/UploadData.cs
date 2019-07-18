using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakBattery.Baking
{
    /// <summary>
    /// MES上传数据实体
    /// </summary>
    public class UploadData
    {
        public string batch_number { get; set; }
        public string device_status { get; set; }
        public string collector_time { get; set; }
        public string parameter_flag { get; set; }
        public List<MaterialCodeInfo> materialCodeInfo { get; set; }
        public List<DeviceCodeInfo> deviceCodeInfo { get; set; }
        public List<DeviceParamData> deviceParamData { get; set; }
    }

    public class MaterialCodeInfo
    {
        public string materialCodeName { get; set; }
        public string materialCode { get; set; }
    }

    public class DeviceCodeInfo
    {
        public string deviceCodeName { get; set; }
        public string deviceCode { get; set; }
    }

    public class DeviceParamData
    {
        public string parameter_name { get; set; }
        public string parameter_unit { get; set; }
        public string parameter_value { get; set; }
    }
}
