<template>
	<header>
		<div class="logo-container">
			<div class="logo" v-on:click="goHome">
			</div>
			<div v-if="user != null" class="buttons">
				<div class="button-group">
					<router-link :to="{ path: `account/${user.id}` }" tag="button">Account</router-link>
					<button v-on:click="logout">Log out</button>
				</div>
			</div>
			<div v-else class="buttons">
				<div class="button-group">
					<router-link :to="{ name: 'login' }" tag="button" v-if="$route.path !== '/login'">Log in</router-link>
					<router-link :to="{ name: 'register' }" tag="button" v-if="$route.path !== '/register'">Register</router-link>
				</div>
			</div>
		</div>
	</header>
</template>

<script>
import $ from '../shared/shared.js'

export default {
	methods: {
		goHome () {
			if (this.$router.history.current.name !== 'home') {
				this.$router.push({ name: 'home' })
			}
		},
		logout () {
			sessionStorage.removeItem('token')
			this.$router.go()
		},
		loadUser () {
			let app = this
			$.ajax({
				method: 'GET',
				url: '/api/user',
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 200) {
						app.user = JSON.parse(xhr.responseText)
						console.log(xhr.responseText)
					} else if (xhr.status === 401) {
						app.token = ''
						sessionStorage.removeItem('token')
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		}
	},
	data () {
		return {
			user: null
		}
	},
	beforeMount () {
		this.user = null
		if (sessionStorage.token) {
			this.loadUser()
		}
	}
}
</script>

<style>
header {
	position: fixed;
	width: 100%;
	height: var(--header-height);
	margin: 0;
	padding: 0;
	box-shadow: 0 1px 10px 1px rgba(0, 0, 0, 0.15);
	background: #fff;
	z-index: 100;
}

header .logo-container {
	display: flex;
	justify-content: center;
}

header .logo {
	width: var(--header-height);
	height: var(--header-height);
	background: url('../assets/pageLogo.svg') no-repeat center;
	background-size: contain;
	cursor: pointer;
	z-index: 101;
}

header .buttons {
	position: fixed;
	width: 100%;
	height: var(--header-height);
	display: flex;
	justify-content: flex-end;
}

.button-group {
	margin: auto 10%;
	z-index: 102;
}

</style>
