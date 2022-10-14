using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListProject.ModelViews.ReadViews
{
    public class ToDoReadResponse
    {
        public int Id { get; set; }

        public bool IsRead { get; set; }

        public DateTime updatedDate = DateTime.Now;
}
}
