using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace ToDoListProject.DbModel.Models
{
    public partial class ToDo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public bool IsArchived { get; set; }
        public bool IsRead { get; set; }

        [DefaultValue("")]
        public string Image { get; set; }
        public int AssignedBy { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
    }
}
