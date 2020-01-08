<template>
	<div id="login">
		<div class="panel">
			<div class="panel-body">
				<div class="col">
					<div class="input-group">
						<label for="username">Username*</label>
						<input id="username" type="text" v-model="username">
					</div>
					<div class="input-group">
						<label for="password">Password*</label>
						<input id="password" type="password" v-model="password">
					</div>

					<button v-on:click="login">Log in</button>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
import $ from '../shared/shared.js'

export default {
	name: 'login',
	components: {
	},
	data: function () {
		return {
			username: '',
			password: ''
		}
	},
	beforeMount () {
		if (sessionStorage.getItem('token')) {
			this.$router.replace({ name: 'home' })
		}
	},
	methods: {
		login: function (event) {
			const app = this
			event.preventDefault()
			// get auth token from api
			$.ajax({
				method: 'POST',
				url: '/api/user/login',
				payload: JSON.stringify({
					username: this.username,
					password: this.password
				}),
				callback: function (xhr) {
					let response = JSON.parse(xhr.responseText)
					sessionStorage.setItem('token', response.token)
					app.$router.replace({ name: 'home' })
				}
			})
		}
	}
}
</script>

<style scoped>
#login {
	width: 100%;
	height: calc(100vh - var(--header-height));
}

.panel {
	display: flex;
	width: 70vw;
	height: 100%;
	margin: 0 auto;
	justify-content: center;
	align-items: center;
}

.panel-body {
	margin: 0 auto;
	padding: 5% 10%;
	box-shadow: 0 1px 10px 1px rgba(0, 0, 0, .15);
}

.col {
	display: flex;
	align-items: center;
	flex-direction: column;
}

.input-group {
	width: 100%;
	display: flex;
	flex-direction: column;
	align-items: flex-start;
	font-size: 16px;
}

.input-group + .input-group {
	margin-top: 1em;
}

.input-group input, .input-group label {
	font-size: inherit;
	width: 100%;
	text-align: left;
}

button {
	margin: 1em 2em;
}
</style>
