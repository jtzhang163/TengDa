using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TengDa.WF
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class Service
    {
        [ReadOnly(true), DisplayName("ID")]
        public int Id { get; set; }

        private string TableName
        {
            get
            {
                Type type = this.GetType();
                PropertyInfo propertyInfo = type.GetProperty("TableName");
                return (string)propertyInfo.GetValue(this, null);
            }
        }

        protected void UpdateDbField(string field, object value)
        {
            string msg = string.Empty;
            if (!Database.UpdateField(Id, TableName, field, value.ToString(), out msg))
            {
                LogHelper.WriteError(msg);
            }
        }
    }
}
