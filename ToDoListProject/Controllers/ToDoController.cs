using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDoListProject.Core.Mangers.Interfaces;
using ToDoListProject.ModelViews.ModelViews;
using ToDoListProject.ModelViews.ReadViews;
using ToDoListProject.ModelViews.Requset;
using ToDoListWorker.Attributes;

namespace ToDoListProject.Controllers
{

    [ApiController]
    public class ToDoController : ApiBaseController
    {
        private IToDoListManager _toDoListManager;
        private readonly ILogger<ToDoController> _logger;

        public ToDoController(IToDoListManager toDoListManager, ILogger<ToDoController> logger)
        {
            _logger = logger;
            _toDoListManager = toDoListManager;
        }



        [Route("CreateToDo")]
        [HttpPost]
        [Authorize]
        public IActionResult CreateToDo(ToDoRequset requset)
        {
            var create = _toDoListManager.CreateToDo(LoggedInUser, requset);
            return Ok(create);

        }

        [Route("GetAllToDoLists")]
        [HttpGet]
        [Authorize]
        public IActionResult GetAllToDoLists(int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "")
        {
            var res= _toDoListManager.GetAllToDoLists(page, pageSize, sortColumn, sortDirection, searchText);
            return Ok(res);
        }


        [Route("ReadToDo")]
        [HttpPut]
        [Authorize]
        public IActionResult ReadToDo([FromBody]ToDoReadRequast toDoReadRequast)
        {
            var res = _toDoListManager.ReadToDo(LoggedInUser, toDoReadRequast);

            return Ok(res);
        }
        
        [Route("GetAllReadToDos")]
        [HttpGet]
        [ToDoListAuthrize]
        public IActionResult GetAllReadToDos()
        {
            var res=_toDoListManager.GetAllReadToDos();
            return Ok(res);
        }

        [Route("UpdateToDo")]
        [HttpPut]
        public IActionResult UpdateToDoUpdateToDo( ToDoUpdateModel request)
        {
            var res = _toDoListManager.UpdateToDo(LoggedInUser, request);

            return Ok(res);
        }

        [Route("DeleteToDoList")]
        [HttpDelete]
        public IActionResult DeleteToDoList(int id)
        {
            _toDoListManager.DeleteToDoList(LoggedInUser, id);

            return Ok("Delete Success");
        }
    }
}
