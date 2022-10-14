using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListProject.ModelViews.ModelViews
{
    public class UserResult
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DefaultValue("")]
        public string Image { get; set; }

        public string Email { get; set; }
    }
}
