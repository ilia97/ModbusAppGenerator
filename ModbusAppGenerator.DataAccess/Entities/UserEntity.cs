using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ModbusAppGenerator.DataAccess.Entities
{
    // Add profile data for application users by adding properties to the UserEntity class
    public class UserEntity : IdentityUser
    {
        public UserEntity(): base()
        {
            Projects = new List<ProjectEntity>();
        }

        public string FirstName { set; get; }

        public string LastName { set; get; }

        public virtual List<ProjectEntity> Projects { set; get; }
    }
}
