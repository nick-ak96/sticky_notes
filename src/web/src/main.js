import Vue from 'vue'
import App from './App.vue'
import router from './router'

Vue.config.productionTip = false

router.beforeEach((to, from, next) => {
	if (to.matched.some(record => record.meta.conditionalRoute)) {
		if (sessionStorage.token) {
			next()
		} else {
			next({ path: '/' })
		}
	} else {
		next()
	}
})

new Vue({
	router,
	render: h => h(App)
}).$mount('#app')
