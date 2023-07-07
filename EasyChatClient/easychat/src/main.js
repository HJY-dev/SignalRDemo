import { createApp } from 'vue'
import App from './App.vue'
import signalr from './utils/signalR'
import axios from 'axios'

const app = createApp(App)
axios.defaults.baseURL = "https://localhost:7027"
app.config.globalProperties.$signalr = signalr.signal;
app.config.globalProperties.$http = axios;
app.mount('#app')