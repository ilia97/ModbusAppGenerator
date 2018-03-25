using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ModbusAppGenerator.DataAccess.Entities
{
    public class UserEntity : IdentityUser
    {
        public UserEntity() : base()
        {
            Projects = new List<ProjectEntity>();
        }

        public string FirstName { set; get; }

        public string LastName { set; get; }

        public virtual List<ProjectEntity> Projects { set; get; }
    }
}
