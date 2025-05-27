using ChatAppDataAccessLayer;
using ChatAppDataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using ChatAppServiceLayer.Models;
using User = ChatAppDataAccessLayer.Models.User;
using Microsoft.AspNetCore.Cors;

namespace ChatAppServiceLayer.Controllers
{
    [EnableCors("AllowFrontend")]

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
            User res = new User();
            try
            {
                res = chatAppRepository.VerifyUser(user.Username,user.Password);
            }
            catch (Exception)
            {
                res = null;
            }
            return Json(res);
        }
        //[HttpPost("uploadimage")]

        //public async Task<JsonResult> UploadImage(IFormFile image, string userId)
        //{
        //    bool result;
        //    try
        //    {
        //        result = await chatAppRepository.UploadImage(image, userId);
        //    }
        //    catch (Exception)
        //    {
        //        result = false;
        //    }
        //    return Json(result);
        //}

        //[HttpGet("getimage")]
        //public async Task<JsonResult> GetImage(int userId)
        //{
        //    string imageUrl = "";
        //    try
        //    {
        //        imageUrl =  chatAppRepository.getImageUrl(userId);
        //    }
        //    catch (Exception)
        //    {
        //        imageUrl = null;
        //    }
        //    return Json(imageUrl);
        //}

    }
}
