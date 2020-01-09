<template>
	<div class="org">
		<div class="back">
			<router-link :to="{ path: `/account/${id}/org` }" tag="span" class="fas fa-chevron-left fa-2x">
				</router-link>
		</div>
		<div class="row">
			<div class="organization">
				<div class="org-name">
					<div class="org-edit-buttons" v-if="nameEdited">
						<span class="content-edit fas fa-check" v-on:click="updateOrg"></span>
						<span class="content-edit fas fa-times" v-on:click="org.name = originalName"></span>
					</div>
					<input
						type="text"
						maxlength="100"
						v-bind:readonly="org.createdBy !== id"
						v-model="org.name"
						class="org-name-input"
						>
				</div>
				<div class="org-members">
					<ul>
						<li
							v-for="(member, index) in org.members"
							v-bind:key="member.id"
							>
							@{{ member.username }}
							<span
								v-if="org.createdBy === id"
								class="fas fa-trash"
								v-on:click="deleteMember(index)"
								></span>
						</li>
					</ul>
					<div class="input-group" v-if="org.createdBy === id">
						<input type="number" v-model="newMember">
						<button v-on:click="addMember">Add member</button>
					</div>
				</div>
			</div>
			<div class="org-notes">
				<note
				v-for="note in orgNotes"
				v-bind:key="note.id"
				v-bind:note="note"
				v-bind:editable="false"
				></note>
			</div>
		</div>
	</div>
</template>

<script>
import $ from '../shared/shared.js'
import note from './Note.vue'

export default {
	components: {
		note
	},
	data () {
		return {
			org: {},
			orgNotes: [],
			newMember: Number,
			originalName: '',
			nameEdited: false
		}
	},
	props: [
		'id', 'orgId'
	],
	mounted () {
		this.getOrg()
		this.getOrgNotes()
	},
	watch: {
		'org.name' (n) {
			this.nameEdited = n !== '' && n !== this.originalName
		}
	},
	methods: {
		getOrg () {
			let app = this
			$.ajax({
				method: 'GET',
				url: `/api/org/${app.orgId}`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 200) {
						app.org = JSON.parse(xhr.responseText)
						app.originalName = app.org.name
						console.log(xhr.responseText)
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		getOrgNotes () {
			let app = this
			$.ajax({
				method: 'GET',
				url: `/api/org/${app.orgId}/note`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 200) {
						app.orgNotes = JSON.parse(xhr.responseText)
						console.log(xhr.responseText)
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		updateOrg () {
			let app = this
			$.ajax({
				method: 'PUT',
				url: `/api/org/${app.orgId}`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				payload: JSON.stringify({ name: app.org.name }),
				callback (xhr) {
					if (xhr.status === 200) {
						app.org = JSON.parse(xhr.responseText)
						app.originalName = app.org.name
						app.nameEdited = false
						console.log(xhr.responseText)
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		deleteMember (memberIndex) {
			let app = this
			$.ajax({
				method: 'DELETE',
				url: `/api/org/${app.orgId}/user/${app.org.members[memberIndex].id}`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 204) {
						app.getOrg()
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		addMember () {
			let app = this
			$.ajax({
				method: 'POST',
				url: `/api/org/${app.orgId}/user/${app.newMember}`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 204) {
						app.getOrg()
						app.newMember = Number
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
.org {
	margin: 1em 10%;
}

.back {
	text-align: left;
	margin-bottom: 3em;
}

.row {
	display: flex;
	align-items: flex-start;
	flex-direction: row;
}

.organization {
	width: 15%;
	text-align: left;
}

.org-notes {
	width: 80%;
	padding: 1em;
}

.org span {
	cursor: pointer;
}

.org-members {
	text-align: right;
}

.org-members span {
	cursor: pointer;
}

.org-members ul, .org-members li {
	list-style: none;
	padding: 0;
	margin: 0;
}

.org-members li {
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

.org-name {
	position: relative;
}

.org-name-input {
	border: none;
	width: 100%;
	font-size: 1.5rem;
}
.org-edit-buttons {
	position: absolute;
	top: -1.7em;
	left: calc((100% - 6em) / 2);
	width: 6em;
	background-color: #fff;
	border-radius: 6px;
	box-shadow: 1px 0px 3px 1px rgba(0, 0, 0, .2);
	padding: .1em 0;
}

.org-edit-buttons span {
	margin: 0 1em;
}

.org-edit-buttons span.fa-check {
	color: #3ABB48;
}

.org-edit-buttons span.fa-times {
	color: #FF0550;
}
</style>
