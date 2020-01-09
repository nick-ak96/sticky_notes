<template>
	<div>
			<h2>STICKY NOTES</h2>
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

<style scoped>
.buttons {
	text-align: right;
	padding: 0 10%;
}
</style>
