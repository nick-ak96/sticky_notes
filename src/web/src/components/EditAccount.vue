<template>
	<div class="account">
		<h4>@{{ user.username }}</h4>
		<div class="input-group">
			<label for="uid">ID</label>
			<input id="uid" type="text" v-model="user.id" readonly>
		</div>
		<div class="input-group">
			<label for="name">Name</label>
			<input id="name" type="text" v-model="user.name">
		</div>
		<div class="input-group">
			<label for="surname">Surname</label>
			<input id="surname" type="text" v-model="user.surname">
		</div>
		<div class="input-group">
			<button v-on:click="passwordChanging = !passwordChanging">Change password</button>
		</div>
		<div class="pass-change" v-if="passwordChanging">
			<div class="input-group">
				<label for="password">New password</label>
				<input id="password" type="password" v-model="newPassword">
			</div>
			<div class="input-group">
				<label for="repassword">Re-enter password</label>
				<input id="repassword" type="password" v-model="newRePassword">
			</div>
		</div>
		<div class="input-group">
			<button v-if="edited || passwordChanging" v-on:click="updateUser">Save</button>
			<button v-if="edited || passwordChanging" v-on:click="discardChanges">Discard</button>
		</div>
	</div>
</template>

<script>
import $ from '../shared/shared.js'

export default {
	props: [
		'id'
	],
	data () {
		return {
			user: {},
			oldName: '',
			oldSurname: '',
			passwordChanging: false,
			newPassword: '',
			newRePassword: '',
			edited: false
		}
	},
	mounted () {
		this.loadUser()
	},
	watch: {
		'user.name' (n) {
			this.edited = n !== '' && n !== this.oldName
		},
		'user.surname' (n) {
			this.edited = n !== '' && n !== this.oldSurname
		}
	},
	methods: {
		loadUser () {
			let app = this
			$.ajax({
				method: 'GET',
				url: `/api/user`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 200) {
						app.user = JSON.parse(xhr.responseText)
						app.oldName = app.user.name
						app.oldSurname = app.user.surname
						console.log(xhr.responseText)
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		discardChanges () {
			this.user.name = this.oldName
			this.user.surname = this.oldSurname
			this.newPassword = ''
			this.newRePassword = ''
			this.passwordChanging = false
		},
		updateUser () {
			let payload = this.user
			if (this.passwordChanging) {
				if (this.newPassword === '') {
					alert('Password cannot be empty')
					return
				}
				if (this.newPassword !== this.newRePassword) {
					alert('Passwords do not match')
					return
				}
				payload.password = this.newPassword
			}
			let app = this
			$.ajax({
				method: 'PUT',
				url: `/api/user`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				payload: JSON.stringify(payload),
				callback (xhr) {
					if (xhr.status === 200) {
						app.user = JSON.parse(xhr.responseText)
						app.oldName = app.user.name
						app.oldSurname = app.user.surname
						app.passwordChanging = false
						console.log(xhr.responseText)
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		}
	}
}
</script>

<style>
.account {
	width: 50%;
	margin: 1em auto;
	text-align: left;
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
	margin: 1em 0;
}
.input-group label {
	display: inline-block;
	width: 30%;
}
.input-group input {
	display: inline-block;
	width: 65%;
}
</style>
