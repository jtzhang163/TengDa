using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TengDa;

namespace Zopoise.Scada.App
{
  public class UserCVDataViewModel
  {
    [DisplayName("用户名称")]
    public string UserName { get; set; }

    /// <summary>
    /// 测试器名称
    /// </summary>
    [DisplayName("测试器名称")]
    public string TesterName { get; set; }

    /// <summary>
    /// 电压
    /// </summary>
    [DisplayName("电压")]
    public float Voltage { get; set; }

    /// <summary>
    /// 电流1
    /// </summary>
    [DisplayName("电流1")]
    public float Current1 { get; set; }

    /// <summary>
    /// 电流2
    /// </summary>
    [DisplayName("电流2")]
    public float Current2 { get; set; }

    /// <summary>
    /// 电流3
    /// </summary>
    [DisplayName("电流3")]
    public float Current3 { get; set; }

    /// <summary>
    /// 电流4
    /// </summary>
    [DisplayName("电流4")]
    public float Current4 { get; set; }

    /// <summary>
    /// 电流5
    /// </summary>
    [DisplayName("电流5")]
    public float Current5 { get; set; }

    /// <summary>
    /// 电流6
    /// </summary>
    [DisplayName("电流6")]
    public float Current6 { get; set; }

    /// <summary>
    /// 记录时间
    /// </summary>
    [DisplayName("电流类型")]
    public CurrentType CurrentType { get; set; }
    /// <summary>
    /// 记录时间
    /// </summary>
    [DisplayName("记录时间")]
    public DateTime RecordTime { get; set; }
  }
}
