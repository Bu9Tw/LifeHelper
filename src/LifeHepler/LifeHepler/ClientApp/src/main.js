import Vue from 'vue'
import App from './App.vue'
import $ from "jquery";
import Toast from "vue-toastification"
import "vue-toastification/dist/index.css"

Vue.config.productionTip = false
const toastOption = {}
Vue.use(Toast, toastOption)

Vue.prototype.$goToPageTop = function() {
    $('html,body').animate({ scrollTop: 0 }, 'fast'); /* 返回到最頂上 */
    return false;
}

new Vue({
    render: h => h(App),
}).$mount('#app')