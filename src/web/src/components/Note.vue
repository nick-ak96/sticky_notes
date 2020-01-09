<template>
	<div class="note" v-bind:style="{ background: noteColor }">
		<div class="note-edit-buttons" v-if="noteEdited">
			<span class="content-edit fas fa-check" v-on:click="updateContent"></span>
			<span class="content-edit fas fa-times" v-on:click="discardChanges"></span>
		</div>
		<div class="note-edit-buttons" v-if="colorPaletteOpen">
			<span class="color-item"
				v-for="color in noteColors"
				v-bind:key="color.id"
				v-bind:style="{ background: color.value }"
				v-on:click="updateColor(color.id)"
				></span>
		</div>
		<div>
			<textarea
			v-bind:readonly="!editable"
			v-model="note.content"
			rows=7
			>
			</textarea>
		</div>
		<div>
			<div class="note-info">
				<span class="fas fa-user">
					<span class="tooltip">
						@{{ note.username }}
					</span>
				</span>
				<span class="fas fa-clock">
					<span class="tooltip">
						{{ new Date(note.lastModified).toLocaleString() }}
					</span>
				</span>
			</div>
			<div class="note-tools" v-if="editable">
				<div class="share-menu" v-if="shareMenuOpen">
					<span class="fas fa-times close-button" v-on:click="shareMenuOpen = false"></span>
					<div class="box-scroll">
						<div class="box">
							<div>
								<h4>Public</h4>
								<div class="input-group">
									<input id="sh-public" type="checkbox"
										v-model="isPublic"
										v-on:change="sharePublic"
									>
								</div>
							</div>
							<div>
								<h4>Users</h4>
								<div class="sharing-list">
									<ul>
										<li
											v-for="(user, index) in sharingInfo.users"
											v-bind:key="index"
											>
											{{ user.userDetails.name + ' ' + user.userDetails.surname }}
											<span class="fas fa-trash" v-on:click="removeUser(user.userId, index)"></span>
										</li>
									</ul>
									<div class="input-group">
										<input type="number" v-model="newUserToShare" placeholder="Enter a user ID for sharing">
										<button v-on:click="addUser">Share</button>
									</div>
								</div>
							</div>
							<div>
								<h4>Organizations</h4>
								<div class="sharing-list">
									<ul>
										<li
											v-for="(org, index) in sharingInfo.organizations"
											v-bind:key="index"
											>
											{{ org.organizationDetails.name }}
											<span class="fas fa-trash" v-on:click="removeOrg(org.organizationId, index)"></span>
										</li>
									</ul>
									<div class="input-group">
										<input type="number" v-model="newOrgToShare" placeholder="Enter an ID of organization for sharing">
										<button v-on:click="addOrg">Share</button>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
				<span class="fas fa-trash" v-on:click="deleteNote"></span>
				<span class="fas fa-share-alt" v-on:click="shareMenuOpen = true"></span>
				<span class="fas fa-palette" v-on:click="colorPaletteOpen = !colorPaletteOpen"></span>
			</div>
		</div>
	</div>
</template>

<script>
import $ from '../shared/shared.js'

export default {
	name: 'note',
	components: {
	},
	props: [
		'note', 'editable'
	],
	data () {
		return {
			noteColors: [
				{ id: 1, value: '#33BFF4' },
				{ id: 2, value: '#A4A4A7' },
				{ id: 3, value: '#78E078' },
				{ id: 4, value: '#FFA85C' },
				{ id: 5, value: '#D890E1' },
				{ id: 6, value: '#FF5F5F' },
				{ id: 7, value: '#FFD46A' }
			],
			noteColor: '',
			originalNoteContent: '',
			noteEdited: false,
			colorPaletteOpen: false,
			shareMenuOpen: false,
			sharingInfo: null,
			isPublic: false,
			toUsers: false,
			toOrganizations: false,
			newUserToShare: '',
			newOrgToShare: ''
		}
	},
	watch: {
		'note.content' (n) {
			this.noteEdited = n !== '' && n !== this.originalNoteContent
		}
	},
	mounted () {
		this.setColor(this.note.color)
		this.originalNoteContent = this.note.content
		if (this.editable) {
			this.loadSharingData()
		}
	},
	methods: {
		setColor (colorId) {
			const colorIdx = this.noteColors.findIndex(c => c.id === colorId)
			this.noteColor = this.noteColors[colorIdx].value
		},
		updateContent () {
			this.updateNote({
				content: this.note.content
			})
		},
		updateColor (colorId) {
			this.setColor(colorId)
			this.updateNote({
				color: colorId
			})
		},
		updateNote (payload) {
			let app = this
			$.ajax({
				method: 'PUT',
				url: `/api/user/note/${app.note.id}`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				payload: JSON.stringify(payload),
				callback (xhr) {
					if (xhr.status === 200) {
						let response = JSON.parse(xhr.responseText)
						app.$emit('note-update', response)
						app.noteEdited = false
						app.originalNoteContent = response.content
						console.log(xhr.responseText)
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		discardChanges () {
			this.note.content = this.originalNoteContent
		},
		deleteNote () {
			let app = this
			$.ajax({
				method: 'DELETE',
				url: `/api/user/note/${app.note.id}`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 204) {
						app.$emit('note-delete')
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		loadSharingData () {
			let app = this
			$.ajax({
				method: 'GET',
				url: `/api/user/note/${app.note.id}/sharing`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 200) {
						let response = JSON.parse(xhr.responseText)
						app.sharingInfo = response
						app.isPublic = app.sharingInfo.isPublic
						app.toUsers = app.sharingInfo.users.length > 0
						app.toOrganizations = app.sharingInfo.organizations.length > 0
						console.log(xhr.responseText)
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		sharePublic () {
			let app = this
			$.ajax({
				method: app.isPublic ? 'POST' : 'DELETE',
				url: `/api/user/note/${app.note.id}/share/public`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 204) {
						app.loadSharingData()
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		addUser () {
			let app = this
			if (app.sharingInfo.users.findIndex(u => u.userId === parseInt(app.newUserToShare)) !== -1) {
				alert('The user is already is the sharing list')
				return
			}
			$.ajax({
				method: 'POST',
				url: `/api/user/note/${app.note.id}/share/user/${app.newUserToShare}`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 204) {
						app.loadSharingData()
						app.newUserToShare = ''
					} else if (xhr.status === 409 || xhr.status === 403) {
						alert(JSON.parse(xhr.responseText).detail)
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		removeUser (userId, index) {
			let app = this
			$.ajax({
				method: 'DELETE',
				url: `/api/user/note/${app.note.id}/share/user/${userId}`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 204) {
						app.sharingInfo.users.splice(index)
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		addOrg () {
			let app = this
			if (app.sharingInfo.organizations.findIndex(org => org.organizationId === parseInt(app.newOrgToShare)) !== -1) {
				alert('The organization is already is the sharing list')
				return
			}
			$.ajax({
				method: 'POST',
				url: `/api/user/note/${app.note.id}/share/org/${app.newOrgToShare}`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 204) {
						app.loadSharingData()
						app.newOrgToShare = ''
					} else if (xhr.status === 409 || xhr.status === 403) {
						alert(JSON.parse(xhr.responseText).detail)
					} else {
						console.error(xhr.responseText)
					}
				}
			})
		},
		removeOrg (orgId, index) {
			let app = this
			$.ajax({
				method: 'DELETE',
				url: `/api/user/note/${app.note.id}/share/org/${orgId}`,
				headers: [
					{ name: 'Authorization', value: `Bearer ${sessionStorage.token}` }
				],
				callback (xhr) {
					if (xhr.status === 204) {
						app.sharingInfo.organizations.splice(index)
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
.note {
	height: 10em;
	width: 10em;
	padding: 1em;
	margin: 1.5em 1em;
	position: relative;
	box-shadow: 1px 1px 10px 1px rgba(0, 0, 0, .15);
	display: inline-block;
}

.note textarea {
	resize: none;
	font-size: 100%;
	font-weight: normal;
	text-align: left;
	display: inline-block;
	width: 100%;
	background: none;
	border: none;
	height: 100%;
}

.note-edit-buttons {
	position: absolute;
	top: -1.7em;
	left: 10%;
	width: 80%;
	background-color: #fff;
	border-radius: 6px;
	box-shadow: 1px 0px 3px 1px rgba(0, 0, 0, .2);
	padding: .1em 0;
}

.note-edit-buttons span, .note-tools span {
	cursor: pointer;
}

.note-edit-buttons span.content-edit {
	margin: 0 1em;
}

.note-edit-buttons span.content-edit.fa-check {
	color: #3ABB48;
}

.note-edit-buttons span.content-edit.fa-times {
	color: #FF0550;
}

.note-edit-buttons span.color-item {
	display: inline-block;
	width: 1em;
	height: 1em;
	margin: 0 .1em;
	border-radius: 10px;
}

.note textarea[readonly] {
	outline: none;
}

.note-info, .note-tools {
	position: absolute;
	bottom: 1em;
}

.note-info {
	left: 1em;
}

.note-info > span {
	float: left;
}

.note-tools {
	right: 1em;
}

.note-tools > span {
	float: right;
}

.note-tools > span, .note-info > span {
	position: relative;
	margin: 0 .2em;
}

.tooltip {
	visibility: hidden;
	width: 120px;
	background-color: #000;
	font-size: 12px;
	font-weight: normal;
	color: #fff;
	text-align: center;
	border-radius: 6px;
	padding: .5em 0;
	position: absolute;
	z-index: 200;
	bottom: 150%;
	left: 50%;
	margin-left: -60px;
	opacity: 0;
	transition: opacity 0.2s;
}

.tooltip::after {
	content: '';
	position: absolute;
	top: 100%;
	left: 50%;
	margin-left: -5px;
	border-width: 5px;
	border-style: solid;
	border-color: #000 transparent transparent transparent;
}

.note-info > span:hover .tooltip {
	visibility: visible;
	opacity: .8;
}

.share-menu {
	position: fixed;
	z-index: 200;
	top: 20%;
	left: 30%;
	width: 40%;
	height: 60%;
	background: #fff;
	border-radius: 6px;
	box-shadow: 1px 0px 3px 1px rgba(0, 0, 0, .2);
	overflow: auto;
}

.close-button {
	position: absolute;
	right: 10px;
	top: 10px;
	cursor: pointer;
}

.box {
	height: 100%;
	width: 70%;
	overflow: auto;
	text-align: center;
	margin: 0 auto;
	padding: 2em 0;
}

.input-group {
	text-align: right;
}

.box h4 {
	text-align: left;
}

.input-group input:not([type="checkbox"]) {
	width: 60%;
	margin-left: 5px;
}

.sharing-list ul, .sharing-list li {
	list-style: none;
	text-align: right;
}

.sharing-list li {
	margin: .5em 0;
	padding: 5px 10px;
	box-shadow: 1px 0px 3px 1px rgba(0, 0, 0, .2);
}
</style>
