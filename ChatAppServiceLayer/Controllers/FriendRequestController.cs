using Microsoft.AspNetCore.Mvc;
using ChatAppDataAccessLayer;
using ChatAppDataAccessLayer.Models;
using ChatAppServiceLayer.Models;

namespace ChatAppServiceLayer.Controllers

{

    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestController : Controller
    {
        ChatAppRepository chatAppRepository;
        public FriendRequestController(ChatAppRepository chatAppRepository)
        {
            this.chatAppRepository = chatAppRepository;
        }


        [HttpPost("request")]
        public async Task<IActionResult> SendRequest(Friendrequest dto)
        {
            var result = await chatAppRepository.SendRequest(dto.RequesterId, dto.AddresseeId);
            return Ok(result);
        }
        [HttpPost("accept")]
        public async Task<IActionResult> AcceptRequest(Friendrequest dto)
        {
            var result = await chatAppRepository.AcceptRequest(dto.RequesterId, dto.AddresseeId);
            return Ok(result);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetFriendsList(int userId)
        {
            var friends = await chatAppRepository.Friends(userId);
            if (friends == null)
            {
                return NotFound("No friends found");
            }

            return Ok(friends);
        }


    }
}
