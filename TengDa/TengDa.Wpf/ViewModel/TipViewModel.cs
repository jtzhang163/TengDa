namespace TengDa.Wpf
{
    /// <summary>
    /// 界面提示框
    /// </summary>
    public class TipViewModel : BindableObject
    {
        private string tips = string.Empty;
        public string Tips
        {
            get
            {
                return tips;
            }
            set
            {
                SetProperty(ref tips, value);
            }
        }
    }
}
