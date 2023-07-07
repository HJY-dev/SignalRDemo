using EasyChat.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EasyChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hub;
        public LoginController(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        [HttpPost]
        public string Login ()
        {
            return "";
        }



    }
}
