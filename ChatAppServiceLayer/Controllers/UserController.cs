using ChatAppDataAccessLayer;
using ChatAppDataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using ChatAppServiceLayer.Models;
using User = ChatAppDataAccessLayer.Models.User;

namespace ChatAppServiceLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        ChatAppRepository chatAppRepository;
        public UserController(ChatAppRepository chatAppRepository)
        {
            this.chatAppRepository = chatAppRepository;
        }

        [HttpGet]
        public JsonResult GetUser()
        {
            {
                List<User> users = new List<User>();
                try
                {
                    users = chatAppRepository.GetAllCategories();
                }
                catch (Exception)
                {
                    users = null;
                }
                return Json(users);
            }
        }
        [HttpPost("register")]
        public JsonResult SaveUsers(ChatAppServiceLayer.Models.User user)
        {
            bool res = false;
            User u = new User();

            u.Username = user.Username;
            u.Email = user.Email;
            u.DisplayName = user.DisplayName;
            u.Password = user.Password;
            try
            {
                res = chatAppRepository.AddUser(u);
            }
            catch (Exception)
            {
                res = false;
            }
            return Json(res);
        }

        [HttpPost("login")]
        public JsonResult VerifyUsers(AuthUser user)
        {
            string res = "";
            try
            {
                res = chatAppRepository.VerifyUser(user.Username,user.Password);
            }
            catch (Exception)
            {
                res = "Doest Not Exists";
            }
            return Json(res);
        }


    }
}
