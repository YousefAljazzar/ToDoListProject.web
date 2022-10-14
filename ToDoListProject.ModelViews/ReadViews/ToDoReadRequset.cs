using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListProject.ModelViews.ReadViews
{
    public class ToDoReadRequast
    {
        public int Id { get; set; }

        public bool IsRead { get; set; } = false;

    }
}
