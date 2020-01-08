import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'

Vue.use(VueRouter)

const routes = [
	{
		path: '/',
		name: 'home',
		component: Home
	},
	{
		path: '/login',
		name: 'login',
		component: () => import('../views/Login.vue')
	},
	{
		path: '/register',
		name: 'register',
		component: () => import('../views/Register.vue')
	},
	{
		path: '/account',
		name: 'account',
		component: () => import('../views/Account.vue'),
		children: [
			{
				path: '',
				name: 'userNotes',
				component: () => import('../components/UserNotes.vue')
			},
			{
				path: 'shared',
				name: 'sharedNotes',
				component: () => import('../components/SharedNotes.vue')
			},
			{
				path: 'organizations',
				name: 'userOrganizations',
				component: () => import('../components/Organizations.vue')
			},
			{
				path: 'edit',
				name: 'editAccount',
				component: () => import('../components/EditAccount.vue')
			}
		]
	}
]

const router = new VueRouter({
	mode: 'history',
	base: process.env.BASE_URL,
	routes
})
export default router
