import * as signalR from '@microsoft/signalr'
import axios from 'axios'

const url = "https://localhost:7027/chatHub"


const signal = new signalR.HubConnectionBuilder()
  .withUrl(url, {
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets,
    accessTokenFactory: () => ''
  })
  .withAutomaticReconnect()
  .configureLogging(signalR.LogLevel.Information)
  .build()


signal.onclose((err)=>{
    console.log("连接已经断开 执行函数onclose",err)
})

export default {
  signal
}