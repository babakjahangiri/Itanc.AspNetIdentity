using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Itanc.AspNetIdentity.Entities;

namespace Itanc.AspNetIdentity
{
    public class IdentityRole : IRole
    {
        /// <summary>
        /// Default constructor for Role 
        /// </summary>
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        public IdentityRole(string name) : this()
        {
            Name = name;
        }


        public IdentityRole(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public string Id { get; }
        public string Name { get; set; }
    }
}