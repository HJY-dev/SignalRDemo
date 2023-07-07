<template>
  <div class="hello">
    <input
      type="text"
      v-model="state.userMsg"
      v-on:keypress="txtMsgOnkeypress"
    />
    <div>
      <ul>
        <li v-for="(msg, index) in state.message" :key="index">
          <span class="msg">{{ msg }}</span>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup>
import { reactive, onMounted, getCurrentInstance } from "vue";

let internalInstance = getCurrentInstance();
let $signalr = internalInstance.appContext.config.globalProperties.$signalr;
let $axios = internalInstance.appContext.config.globalProperties.$http;

const state = reactive({ userMsg: "", message: [], connectionstatus: "init" });

const getToken = async () => {
  const name = "bob";

  await $axios
    .get(`generatetoken?user=${name}`)
    .then(function (response) {
      // onClose的定义
      if (state.connectionstatus == "init") {
        $signalr.onclose(() => {
          state.connectionstatus = "close";
          signal.stop();
          console.log("连接已关闭");
          e.retryConnection();
        });
      }
      // 处理成功情况
      $signalr.connection._options.accessTokenFactory = () => response.data;
      // 然后我们请求连接signalr
      $signalr.start().then(() => {
        console.log("连接成功");
      });
      console.log($signalr.state+"=============================")
    })
    .catch(function (error) {
      // 处理错误情况
      console.log(error);
    });
};
getToken();
console.log($signalr.state+"=============================")

const retryConnection = () => {
  if (state.connectionstatus == "init" || state.connectionstatus == "start") {
    return;
  } else if (this.connectionstatus == "close") {
    console.log("正在重试连接...");
    this.timer = setInterval(() => {
      getToken();
    }, 10000);
    return;
  }
};

const txtMsgOnkeypress = async function (e) {
  if (e.keyCode != 13) return;
  try {
    await $signalr.invoke("SendPublicMessage", state.userMsg);
  } catch (err) {
    console.error(err);
  }
  state.userMsg = "";
};

onMounted(async () => {
  $signalr.on("PublicMessageReceived", (res) => {
    state.message.push(res);
    console.log(res, "PublicMessageReceived收到消息");
  });
  $signalr.on("ClientMessageReceived", (res) => {
    console.log(res, "ClientMessageReceived收到消息");
  });
});
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
ul li {
  display: block;
  margin-top: 8px;
}
.msg {
  color: red;
}
</style>
