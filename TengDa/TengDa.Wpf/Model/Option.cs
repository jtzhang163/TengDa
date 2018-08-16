using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TengDa.Wpf
{
    public class Option
    {

        public int Id { get; set; }

        /// <summary>
        /// 键名
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(100)]
        public string Remark { get; set; }

        public Option()
        {
            this.Id = -1;
        }

        public Option(string key, string value) : this(key, value, "")
        {
        }

        public Option(string key, string value, string remark)
        {
            Key = key;
            Value = value;
            Remark = remark;
        }

        public static string GetOption(string key)
        {
            using (var data = new OptionContext())
            {
                Option option = data.Options.Where(o => o.Key == key).FirstOrDefault();
                return option != null ? option.Value : string.Empty;
            }
        }

        public static void SetOption(string key, string value)
        {
            SetOption(key, value, string.Empty);
        }

        public static void SetOption(string key, string value, string remark)
        {
            using (var data = new OptionContext())
            {
                Option option = data.Options.Where(o => o.Key == key).FirstOrDefault();
                // return option != null ? option.Value : string.Empty;
                if (option != null)
                {
                    data.Options.Where(o => o.Key == key).First().Value = value;
                }
                else
                {
                    data.Options.Add(new Option { Key = key, Value = value, Remark = remark });
                }
                data.SaveChanges();
            }
        }
    }
}
