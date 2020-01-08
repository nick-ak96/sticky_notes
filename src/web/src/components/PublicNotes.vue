<template>
	<div>
		<div>
			<h3>Public notes</h3>
		</div>
		<div>
			<input v-model="filter" type="text" />
			<button v-on:click="loadPublicNotes">Search</button>
		</div>
		<div>
			<div>
				<note
				v-for="note in notes"
				v-bind:key="note.id"
				v-bind:note="note"
				v-bind:editable="false"
				></note>
			</div>
		</div>
	</div>
</template>

<script>
import note from './Note.vue'
import $ from '../shared/shared.js'

export default {
	name: 'publicNotes',
	components: {
		note
	},
	data () {
		return {
			notes: [],
			filter: ''
		}
	},
	mounted () {
		this.loadPublicNotes()
	},
	methods: {
		loadPublicNotes () {
			let app = this
			$.ajax({
				method: 'GET',
				url: `/api/note?filter=${app.filter}`,
				callback (xhr) {
					if (xhr.status === 200) {
						let response = JSON.parse(xhr.responseText)
						app.notes = response
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

<style scoped>
</style>
