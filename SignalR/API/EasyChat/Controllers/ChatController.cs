using EasyChat.Extend;
using EasyChat.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static EasyChat.Hubs.ChatHub;

namespace EasyChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hub;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatController(IHubContext<ChatHub> hub, IHttpContextAccessor httpContextAccessor)
        {
            _hub = hub;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllUserIds", Name = "GetAllUserIds")]
        public string[] GetAllUserIds()
        {
            return UserStore.Ids.ToArray();
        }

        /// <summary>
        /// 发送指定的消息给指定的客户端
        /// </summary>
        /// <param name="userConnId"></param>
        /// <param name="msg"></param>
        /// <param name="hubContext"></param>
        /// <returns></returns>
        [HttpGet("SendCustomUserMessage", Name = "SendCustomUserMessage")]
        public async Task<IActionResult> SendCustomUserMessage(
            string userConnId,
            string msg
            )
        {
            await _hub.Clients.Client(userConnId).SendAsync("ClientMessageReceived", msg);
            return Ok("Send Successful!");
        }

    }
}
