<template>
	<div>
		<h3>Organizations</h3>
		<div>
			<div>
				<org
					v-for="(o, index) in orgs"
					v-bind:key="o.id"
					v-bind:org="o"
					v-on:org-delete="orgs.splice(index, 1)"
				></org>
			</div>
			<button v-on:click="createOrg">Create organization</button>
		</div>
	</div>
</template>

<script>
import org from './Organization.vue'
import $ from '../shared/shared.js'

export default {
	components: {
		org
	},
	data () {
		return {
			orgs: [],
			filter: '',
			userLanguage: 'en'
		}
	},
	mounted () {
		this.loadOrganizations()
	},
	methods: {
		loadOrganizations () {
			let app = this
			$.ajax({
				method: 'GET',
				url: `/api/user/orgs`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 200) {
						let response = JSON.parse(xhr.responseText)
						app.orgs = response
						console.log(xhr.responseText)
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		createOrg () {
		}
	}
}

</script>

<style>
</style>
