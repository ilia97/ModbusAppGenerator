using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ModbusAppGenerator.Core.Models
{
    public class User: IdentityUser
    { 
        public User()
        {
            Projects = new List<Project>();
        }

        public string FirstName { set; get; }

        public string LastName { set; get; }

        public virtual List<Project> Projects { set; get; }
    }
}
