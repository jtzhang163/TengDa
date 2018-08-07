using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using TengDa.Encrypt;

namespace TengDa.Wpf
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : BindableObject
    {
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                SetProperty(ref id, value);
            }
        }
        private int id = -1;
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetProperty(ref name, value);
            }
        }
        private string name = string.Empty;

        /// <summary>
        /// 用户头像
        /// </summary>
        public string ProfilePicture { get; set; }
        /// <summary>
        /// 用户登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegisterTime { get; set; }
        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginTimes { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 所属类别Id
        /// </summary>
        public int RoleId { get; set; }

        public Role Role { get; set; }
        /// <summary>
        /// 所属类别
        /// </summary>
        //private UserGroup group = new UserGroup();
        //public UserGroup Group
        //{
        //  get
        //  {
        //    using (var data = new UserContext())
        //    {
        //      group = data.UserGroups.FirstOrDefault(g => g.Id == this.GroupId) ?? new UserGroup();
        //    }
        //    return group;
        //  }
        //  set
        //  {
        //    group = value;
        //  }
        //}

        public User() : this(-1)
        {
            Id = -1;
        }

        public User(int id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// 用户组别
    /// </summary>
    public class Role
    {

        public Role() : this(-1)
        {

        }

        public Role(int id)
        {
            Id = id;
        }


        #region 属性
        public int Id { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 用户组别名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 该用户组别下所有用户
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
        #endregion

        public static List<string> GetRoleNameList()
        {
            List<string> list = new List<string>();
            using (var context = new UserContext())
            {
                context.Roles.ToList().ForEach(r => { list.Add(r.Name); });
            }
            return list;
        }

    }
    /// <summary>
    /// 用户上下文
    /// </summary>
    public class UserContext : DbContext
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public UserContext() : base(connectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.Name).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.ProfilePicture).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Password).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Number).HasMaxLength(30);
            modelBuilder.Entity<User>().Property(u => u.PhoneNumber).HasMaxLength(20);
            modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(20);
            modelBuilder.Entity<User>().Property(u => u.RegisterTime).HasColumnType("datetime");
            modelBuilder.Entity<User>().Property(u => u.LastLoginTime).HasColumnType("datetime");
            modelBuilder.Entity<User>().HasRequired(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            modelBuilder.Entity<User>().ToTable("t_user");

            modelBuilder.Entity<Role>().Property(g => g.Name).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<Role>().Property(g => g.Level).IsRequired();
            modelBuilder.Entity<Role>().HasMany(g => g.Users).WithRequired();
            modelBuilder.Entity<Role>().ToTable("t_role");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }

    public class UserInitializer : DropCreateDatabaseIfModelChanges<UserContext>
    {
        protected override void Seed(UserContext context)
        {
            var Roles = new List<Role>
            {
                new Role
                {
                    Name = "系统管理员",
                    Level = 4,
                    Users = new List<User>
                    {
                        new User
                        {
                            Name = "Administrator",
                            Password =  Base64.EncodeBase64("Administrator"),
                            RegisterTime = DateTime.Now,
                            ProfilePicture = "/Images/DefaultProfile.jpg"
                        }
                    }
                },

                new Role
                {
                    Name = "管理员",
                    Level = 3,
                    Users = new List<User>
                    {
                        new User
                        {
                            Name = "Admin",
                            Password =  Base64.EncodeBase64("Admin"),
                            RegisterTime = DateTime.Now,
                            ProfilePicture = "/Images/DefaultProfile.jpg"
                        }
                    }
                },

                new Role
                {
                    Name = "维护人员",
                    Level = 2,
                    Users = new List<User>
                    {
                        new User
                        {
                            Name = "Maintainer",
                            Password =  Base64.EncodeBase64("Maintainer"),
                            RegisterTime = DateTime.Now,
                            ProfilePicture = "/Images/DefaultProfile.jpg"
                        }
                    }
                },

                new Role
                {
                    Name = "操作员",
                    Level = 1,
                    Users = new List<User>
                    {
                        new User
                        {
                            Name = "Operator",
                            Password =  Base64.EncodeBase64("Operator"),
                            RegisterTime = DateTime.Now,
                            ProfilePicture = "/Images/DefaultProfile.jpg"
                        }
                    }
                }
            };
            Roles.ForEach(g => context.Roles.Add(g));
        }
    }
}
