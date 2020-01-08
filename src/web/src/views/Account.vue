<template>
	<div>
		<div>
			<button v-on:click="logout">Log out</button>
		</div>
		<div>
			<ul>
				<li>
					<router-link to="/account">My notes</router-link>
				</li>
				<li>
					<router-link to="/account/shared">Shared notes</router-link>
				</li>
				<li>
					<router-link to="/account/organizations">Organizations</router-link>
				</li>
				<li>
					<router-link to="/account/edit">Account settings</router-link>
				</li>
			</ul>
		</div>
		<div>
			<router-view />
		</div>
	</div>
</template>

<script>
export default {
	name: 'account',
	methods: {
		logout () {
			sessionStorage.removeItem('token')
			this.$router.replace({ name: 'home' })
		}
	}
}
</script>

<style>
</style>
