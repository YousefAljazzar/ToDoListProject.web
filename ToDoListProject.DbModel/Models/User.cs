using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ToDoListProject.DbModel.Models
{
    public partial class User
    {
        public User()
        {
            ToDos = new HashSet<ToDo>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsArcived { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual ICollection<ToDo> ToDos { get; set; }
    }
}
