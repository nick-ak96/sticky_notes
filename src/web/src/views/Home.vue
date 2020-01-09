<template>
	<div>
		<h2>STICKY NOTES</h2>
		<section>
			<publicNotes></publicNotes>
		</section>
	</div>
</template>

<script>
import publicNotes from '../components/PublicNotes.vue'

export default {
	name: 'home',
	components: {
		publicNotes
	}
}
</script>

<style scoped>
</style>
