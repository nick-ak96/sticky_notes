<template>
	<div id="register">
		<div class="panel">
			<div class="panel-body">
				<div class="row">
					<div class="col">
						<div class="input-group">
							<label for="username">Username*</label>
							<input id="username" type="text" v-model="user.username">
						</div>

						<div class="input-group">
							<label for="password">Password*</label>
							<input id="password" type="password" v-model="user.password">
						</div>

						<div class="input-group">
							<label for="repassword">Repeat password*</label>
							<input id="repassword" type="password" v-model="repassword">
						</div>
					</div>
					<div class="col">
						<div class="input-group">
							<label for="name">Name*</label>
							<input id="name" type="text" v-model="user.name">
						</div>

						<div class="input-group">
							<label for="surname">Surname*</label>
							<input id="surname" type="text" v-model="user.surname">
						</div>

						<div class="input-group">
							<label for="language">Language</label>
							<select id="language" v-model="user.language">
								<option v-for="lang in languageOptions" v-bind:key="lang">
									{{ lang }}
								</option>
							</select>
						</div>
					</div>
				</div>
				<button v-on:click="register">Register</button>
			</div>
		</div>
	</div>
</template>

<script>
import $ from '../shared/shared.js'

export default {
	name: 'register',
	components: {
	},
	data () {
		return {
			user: {},
			repassword: '',
			languageOptions: [
				'EN',
				'DE'
			],
			defaultLanguage: 'EN'
		}
	},
	beforeMount () {
		if (sessionStorage.token) {
			this.$router.replace({ name: 'home' })
		}
	},
	methods: {
		register: function (event) {
			const app = this
			event.preventDefault()
			// check data
			for (const prop in this.user) {
				if (prop === 'language') {
					continue
				}
				if (this.user[prop] === '') {
					alert('Please, fill out all the required fields, marked with a start ("*")')
					return
				}
			}
			if (this.user.password !== this.repassword) {
				alert('Passwords do not match')
				return
			} else if (!this.user.language) {
				this.user.language = this.defaultLanguage
			}

			$.ajax({
				method: 'POST',
				url: '/api/user',
				payload: JSON.stringify(app.user),
				callback: function (xhr) {
					if (xhr.status === 201) {
						alert('User account created successfully. You will now be redireced to the home page, where you can log in')
						app.$router.replace({ name: 'home' })
					} else if (xhr.status === 409) {
						alert('An account with the same username already exist. Please, use othet username')
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		}
	}
}
</script>

<style scoped>
#register {
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

.row {
	display: flex;
	align-items: flex-start;
	flex-direction: row;
}

.col {
	display: flex;
	align-items: center;
	flex-direction: column;
	margin: 0 2em;
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

.input-group input, .input-group label, .input-group select {
	font-size: inherit;
	width: 100%;
	text-align: left;
}

button {
	margin: 1em 2em;
}
</style>
