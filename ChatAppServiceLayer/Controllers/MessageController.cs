using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ChatAppDataAccessLayer;

namespace ChatAppServiceLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ChatAppRepository _chatAppRepository;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(ChatAppRepository chatAppRepository, IHubContext<ChatHub> hubContext)
        {
            _chatAppRepository = chatAppRepository;
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatAppServiceLayer.Models.Message m)
        {
            var message = new ChatAppDataAccessLayer.Models.Message
            {
                Content = m.Content,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Timestamp = DateTime.UtcNow
            };

            try
            {
                bool result = await _chatAppRepository.SendMessagesAsync(message);

                if (result)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);

                    return Ok(true);
                }
                return BadRequest("Message could not be sent.");
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("messageslist")]
        public async Task<IActionResult> MessagesList([FromQuery] int senderId, [FromQuery] int receiverId)
        {
            try
            {
                var messages = await _chatAppRepository.MessagesListAsync(senderId, receiverId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
