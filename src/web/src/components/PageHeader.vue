<template>
	<div>
		<div v-if="user != null">
			<h2>{{ `Hello, ${user.name} ${user.surname}` }}</h2>
			<router-link to="/account" tag="button">Account</router-link>
			<button v-on:click="logout">Log out</button>
		</div>
		<div v-else>
			<h3>Welcome to</h3>
			<h2>STICKY NOTES</h2>
			<div class="buttons">
				<router-link :to="{ name: 'login' }" tag="button">Log in</router-link>
				<router-link :to="{ name: 'register' }" tag="button">Register</router-link>
			</div>
		</div>
	</div>
</template>

<script>
export default {
	name: 'page-header',
	props: [
		'user'
	],
	data () {
		return {
		}
	},
	methods: {
		logout () {
			sessionStorage.removeItem('token')
			this.$router.go()
		}
	}
}
</script>

<style>
</style>
