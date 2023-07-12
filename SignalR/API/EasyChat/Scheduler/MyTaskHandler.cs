using Common.Helper;
using EasyChat.Hubs;
using FreeScheduler;
using Microsoft.AspNetCore.SignalR;
using static Common.Helper.ApiResponseInfo;

namespace EasyChat
{
    public class MyTaskHandler : FreeScheduler.TaskHandlers.TestHandler
    {
        private readonly IHubContext<ChatHub> _hub;
        public MyTaskHandler(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }
        public override async void OnExecuting(Scheduler scheduler, TaskInfo task)
        {
            //todo..
            Console.WriteLine(task.Body);
            await _hub.Clients.All.SendAsync("PublicMessageReceived", new ApiResponseInfo
            {
                Code = ResponseCode.SUCCESS,
                Data = new
                {
                    DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Message = task.Body
                }
            });

        }
    }
}
