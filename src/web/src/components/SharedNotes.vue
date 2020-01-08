<template>
	<div>
		<div>
			<h3>Shared notes</h3>
		</div>
		<div>
			<input v-model="filter" type="text" />
			<button v-on:click="loadSharedNotes">Search</button>
		</div>
		<div>
			<div>
				<note
				v-for="note in userNotes"
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
	components: {
		note
	},
	data () {
		return {
			token: '',
			userNotes: [],
			filter: '',
			userLanguage: 'en'
		}
	},
	mounted () {
		this.loadSharedNotes()
	},
	methods: {
		loadSharedNotes () {
			let app = this
			$.ajax({
				method: 'GET',
				url: `/api/user/note/shared`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 200) {
						let response = JSON.parse(xhr.responseText)
						app.userNotes = response
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
</style>
