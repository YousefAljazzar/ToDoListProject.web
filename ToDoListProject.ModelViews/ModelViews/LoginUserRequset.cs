using System.ComponentModel.DataAnnotations;

namespace ToDoListProject.ModelViews.ModelViews
{
    public class LoginUserRequset
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
