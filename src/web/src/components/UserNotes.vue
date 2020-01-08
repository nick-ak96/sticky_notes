<template>
	<div>
		<div>
			<h3>User notes</h3>
		</div>
		<div>
			<input v-model="filter" type="text" />
			<button v-on:click="loadUserNotes">Search</button>
		</div>
		<div>
			<div>
				<note
				v-for="(note, index) in notes"
				v-bind:key="note.id"
				v-bind:note="note"
				v-bind:editable="true"
				v-on:note-update="updateNote"
				v-on:note-delete="notes.splice(index, 1)"
				></note>
			</div>
			<button v-on:click="createUserNote">Create new note</button>
		</div>
	</div>
</template>

<script>
import note from './Note.vue'
import $ from '../shared/shared.js'

export default {
	components: {
		note
	},
	data () {
		return {
			token: '',
			notes: [],
			filter: '',
			userLanguage: 'en'
		}
	},
	mounted () {
		if (sessionStorage.token) {
			this.token = sessionStorage.token
		}
		this.loadUserNotes()
	},
	methods: {
		loadUserNotes () {
			let app = this
			$.ajax({
				method: 'GET',
				url: `/api/user/note?filter=${app.filter}`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${app.token}` }
				],
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
		},
		createUserNote () {
			let app = this
			$.ajax({
				method: 'POST',
				url: '/api/user/note',
				headers: [
					{ name: 'Authorization', value: `Bearer ${app.token}` }
				],
				payload: JSON.stringify({
					content: '',
					color: 1,
					language: app.userLanguage
				}),
				callback (xhr) {
					if (xhr.status === 201) {
						app.loadUserNotes()
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		updateNote (note) {
			const idx = this.notes.findIndex(n => n.id === note.id)
			this.$set(this.notes, idx, note)
		}
	}
}
</script>

<style>
</style>
