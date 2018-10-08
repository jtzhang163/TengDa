namespace TengDa
{
    /// <summary>
    /// 系统运行状态
    /// </summary>
    public enum RunStatus
    {
        未知 = 0,
        闲置 = 1,
        运行 = 2,
        暂停 = 3,
        异常 = 4
    }

    /// <summary>
    /// 任务模式
    /// </summary>
    public enum TaskMode
    {
        未知 = 0,
        自动任务 = 1,
        手动任务 = 2
    }

    /// <summary>
    /// 常见PLC生产厂商
    /// </summary>
    public enum PlcCompany
    {
        Unknown = 0,
        KEYENCE = 1,
        Mitsubishi = 2,
        OMRON = 3,
        Panasonnic = 4,
        Siemens = 5
    }

    /// <summary>
    /// 常见扫码枪生产厂商
    /// </summary>
    public enum ScanerCompany
    {
        Unknown = 0,
        Datalogic = 1,
        KEYENCE = 2
    }

    /// <summary>
    /// 三色灯状态
    /// </summary>
    public enum TriLamp
    {
        Unknown = 0,
        Red = 1,
        Yellow = 2,
        Green = 3
    }

    /// <summary>
    /// 扫码结果
    /// </summary>
    public enum ScanResult
    {
        OK = 1,
        NG = 2,
        Unknown = 3,
        Error = 4
    }
    /// <summary>
    /// 产量类型
    /// </summary>
    public enum YieldKey
    {
        FeedingOK = 1,
        FeedingNG = 2,
        BlankingOK = 3,
        BlankingNG = 4
    }

    /// <summary>
    /// 电流类型
    /// 1. DC（Direct Current，直流） 
    /// 2. AC（Alternating Current，交流）
    /// </summary>
    public enum CurrentType
    {
        DC = 1,
        AC = 2
    }
}
