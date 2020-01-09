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
		path: '/account/:id/org/:orgId',
		name: 'organization',
		component: () => import('../components/Organization.vue'),
		meta: { conditionalRoute: true },
		props: (route) => ({ id: Number(route.params.id), orgId: Number(route.params.orgId) })
	},
	{
		path: '/account/:id',
		component: () => import('../views/Account.vue'),
		children: [
			{
				path: 'shared',
				name: 'sharedNotes',
				component: () => import('../components/SharedNotes.vue'),
				meta: { conditionalRoute: true },
				props: (route) => ({ id: Number(route.params.id) })
			},
			{
				path: 'org',
				name: 'userOrganizations',
				component: () => import('../components/Organizations.vue'),
				meta: { conditionalRoute: true },
				props: (route) => ({ id: Number(route.params.id) })
			},
			{
				path: 'edit',
				name: 'editAccount',
				component: () => import('../components/EditAccount.vue'),
				meta: { conditionalRoute: true },
				props: (route) => ({ id: Number(route.params.id) })
			},
			{
				path: '',
				name: 'userNotes',
				component: () => import('../components/UserNotes.vue'),
				meta: { conditionalRoute: true },
				props: (route) => ({ id: Number(route.params.id) })
			}
		],
		meta: { conditionalRoute: true },
		props: (route) => ({ id: Number(route.params.id) })
	}
]

const router = new VueRouter({
	mode: 'history',
	base: process.env.BASE_URL,
	routes
})
export default router
