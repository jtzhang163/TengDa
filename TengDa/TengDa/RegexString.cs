namespace TengDa
{
    /// <summary>
    /// 常用到的正则表达式
    /// </summary>
    public class RegexString
    {
        public static string IPv4 = @"^(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}$";
        public static string UserName = "^[A-Z0-9a-z]+$";
        public static string Password = "^[A-Z0-9a-z]+$";
    }
}
