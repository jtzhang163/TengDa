using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Entity;
using TengDa.Encrypt;

namespace TengDa.Wpf
{
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
            //modelBuilder.Entity<User>().HasRequired(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            modelBuilder.Entity<User>().ToTable("t_user");

            // modelBuilder.Entity<Role>().HasMany(g => g.Users).WithRequired();
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
