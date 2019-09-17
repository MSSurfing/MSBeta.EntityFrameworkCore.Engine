using MSSurfing.Data.Domain.Users;
using System.Data.Entity.ModelConfiguration;

namespace MSSurfing.Data.Mapping.Users
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("User");
        }
    }
}
