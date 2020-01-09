<template>
	<div>
		<div class="orgs">
			<div class="org-list">
				<ul>
					<li
						v-for="o in orgs"
						v-bind:key="o.id"
						>
						<router-link
							:to="{ path: `/account/${$route.params.id}/org/${o.id}` }"
							>
							{{ o.name }}
						</router-link>
						<span class="fas fa-trash" v-if="id === o.createdBy" v-on:click="deleteOrg(o.id)"></span>
					</li>
				</ul>
			</div>
			<button v-on:click="createOrg">Create organization</button>
		</div>
	</div>
</template>

<script>
import $ from '../shared/shared.js'

export default {
	data () {
		return {
			orgs: []
		}
	},
	props: [
		'id'
	],
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
			let app = this
			$.ajax({
				method: 'POST',
				url: `/api/org`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				payload: JSON.stringify({ name: 'New organization' }),
				callback (xhr) {
					if (xhr.status === 201) {
						const newOrg = JSON.parse(xhr.responseText)
						app.$router.replace({ path: `/account/${app.id}/org/${newOrg.id}` })
						console.log(xhr.responseText)
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		deleteOrg (orgId) {
			let app = this
			$.ajax({
				method: 'DELETE',
				url: `/api/org/${orgId}`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 204) {
						app.loadOrganizations()
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
.orgs {
	padding: 1em;
	text-align: right;
	width: 50%;
	margin: 0 auto;
}

.orgs span {
	cursor: pointer;
}

.org-list ul, .org-list li {
	list-style: none;
	padding: 0;
	margin: 0;
}

.org-list li {
	margin: 2em 0;
	padding: 1em;
	display: block;
	box-shadow: 1px 0px 10px 1px rgba(0, 0, 0, .2);
}

li a {
	padding: 1em;
	color: #000;
	text-decoration: none;
	display: inline-block;
	width: 80%;
}

.org-list {
}
</style>
