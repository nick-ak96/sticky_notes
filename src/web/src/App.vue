<template>
	<div id="app">
		<MainHeader></MainHeader>
		<main>
			<router-view/>
		</main>
		<!--footer>Some footer</footer-->
	</div>
</template>

<script>
import MainHeader from './components/MainHeader.vue'

export default {
	components: {
		MainHeader
	}
}
</script>

<style>
#app {
	font-family: Helvetica, Arial, sans-serif;
	-webkit-font-smoothing: antialiased;
	-moz-osx-font-smoothing: grayscale;
	text-align: center;
	color: #000;
}
:root {
	--header-height: 50px
}
html, body {
	margin: 0;
	padding: 0;
}
main {
	padding-top: var(--header-height);
}

.notes-group {
	padding: 1em;
}

.notes-search-box {
	margin: 1em 0;
}
</style>
