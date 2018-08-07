using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tafel.MES
{


    public class Table
    {
        string _namespace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;
        public Table()
        {
            Type t = Type.GetType(string.Format("{0}.{1}", _namespace, "CheckUser"));
            rows = (BaseClass)Activator.CreateInstance(t);
        }
        public Table(string objClassName)
        {
            Type t = Type.GetType(string.Format("{0}.{1}", _namespace, objClassName));
            rows = (BaseClass)Activator.CreateInstance(t);
        }

        [XmlElement(ElementName ="rows")]
        public BaseClass rows { get; set; }
    }
}
