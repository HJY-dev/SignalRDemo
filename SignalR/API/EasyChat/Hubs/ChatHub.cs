using Common.Helper;
using EasyChat.Extend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Numerics;
using static Common.Helper.ApiResponseInfo;

namespace EasyChat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        /// <summary>
        /// 客户端连接服务端
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public override Task OnConnectedAsync()
        {
            BigInteger.Parse("1111111111111111");
            var id = Context.ConnectionId;
            UserStore.Ids.Add(id);
            return base.OnConnectedAsync();
        }
        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var id = Context.ConnectionId;
            UserStore.Ids.Remove(id);
            GroupStore.UnConnection(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        #region 群发
        public async Task SendPublicMessage(string message)
        {
            string connId = this.Context.ConnectionId;
            //服务器端群发所有客户端
            await Clients.All.SendAsync("PublicMessageReceived", new ApiResponseInfo
            {
                Code = ResponseCode.SUCCESS,
                Data = new
                {
                    ConnId = connId,
                    DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Message = message
                }
            });
        }

        #endregion

        #region 群组
        public Task SendMyselfMessage(string message)
        {
            return Clients.Caller.SendAsync("MyselfMessageReceived", message);
        }

        public Task SendOthersMessage(string message)
        {
            return Clients.Others.SendAsync("OthersMessageReceived", message);
        }
        /// <summary>
        /// 添加用户到某个组
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task AddUserToGroups(string groupName)
        {
            await Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync($"{Context.ConnectionId} has joined the group {groupName}.");
            GroupStore.Add(groupName, Context.ConnectionId);
        }
        /// <summary>
        /// 发送组消息
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendGroupsMessage(string groupName, string message)
        {
            return Clients.Group(groupName).SendAsync("GroupsMessageReceived", message);
        }

        public Task SendOthersInGroupMessage(string groupName, string message)
        {
            return Clients.OthersInGroup(groupName).SendAsync("OthersInGroupMessageReceived", message);
        }
        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            // 按照组来发送消息
            await Clients.Group(groupName).SendAsync($"{Context.ConnectionId} has left the group {groupName}.");
            GroupStore.Remove(groupName, Context.ConnectionId);
        }
        #endregion

        #region 私聊

        //对应ClaimTypes.NameIdentifier的Claim 需认证用户
        public Task SendPrivateMessage(string toUserName,string message)
        {
            return Clients.User(toUserName).SendAsync("PrivateMessageReceived", message);
        }


        #endregion

    }
}
