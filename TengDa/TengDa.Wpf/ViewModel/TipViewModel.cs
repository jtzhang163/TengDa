using System;
using System.Collections.Generic;

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
                var tipsList = new List<string>(value.Split('\n'));
                if(tipsList.Count > 50)
                {
                    //过长删除
                    tipsList.RemoveAt(0);
                }
                SetProperty(ref tips, string.Join("\n", tipsList));
            }
        }
    }
}
