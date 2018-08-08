using System;

namespace TengDa.Wpf
{
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
