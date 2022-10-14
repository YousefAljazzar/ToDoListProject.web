using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListProject.DbModel.Models;

namespace ToDoListProject.ModelViews.ModelViews
{
    public class ToDoListModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public bool IsRead { get; set; }
        public string Image { get; set; }
        public int AssignedBy { get; set; }
        public int UserId { get; set; }
     //   public virtual User User { get; set; }
    }
}
