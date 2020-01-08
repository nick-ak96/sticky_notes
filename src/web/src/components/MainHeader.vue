<template>
	<header>
		<div class="logo-container">
			<div class="logo" v-on:click="goHome">
			</div>
		</div>
	</header>
</template>

<script>
export default {
	methods: {
		goHome () {
			if (this.$router.history.current.name !== 'home') {
				this.$router.push({ name: 'home' })
			}
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
}
</style>
