import Vue from 'vue'
import App from './App.vue'
import axios from 'axios'
import VueAxios from 'vue-axios'
import $ from "jquery";

Vue.config.productionTip = false
Vue.use(VueAxios, axios);

Vue.prototype.$goToPageTop = function() {
    $('html,body').animate({ scrollTop: 0 }, 'fast'); /* 返回到最頂上 */
    return false;
}

new Vue({
    render: h => h(App),
}).$mount('#app')