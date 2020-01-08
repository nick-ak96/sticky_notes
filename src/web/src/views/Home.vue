<template>
	<div>
		<!--aside class="side-bar">
			<div class="navigation">
				<nav>
					<ul>
						<li v-if="token !== ''">
							<router-link :to="{ name: 'account' }">My notes</router-link>
						</li>
						<li><a href="#hero">Home</a></li>
						<li><a href="#public-notes">Public notes</a></li>
						<li><a href="#contacts">Contact</a></li>
					</ul>
				</nav>
			</div>
		</aside-->
		<section>
			<pageHeader
				v-bind:user="user"
				>
			</pageHeader>
		</section>
		<section>
			<publicNotes></publicNotes>
		</section>
	</div>
</template>

<script>
import $ from '../shared/shared.js'
import pageHeader from '../components/PageHeader.vue'
import publicNotes from '../components/PublicNotes.vue'

export default {
	name: 'home',
	components: {
		pageHeader, publicNotes
	},
	data () {
		return {
			token: '',
			user: null
		}
	},
	beforeMount () {
		this.token = ''
		if (sessionStorage.token) {
			this.token = sessionStorage.token
			this.loadUser()
		}
	},
	methods: {
		loadUser () {
			let app = this
			$.ajax({
				method: 'GET',
				url: '/api/user',
				headers: [
					{ name: 'Authorization', value: `Bearer ${app.token}` }
				],
				callback (xhr) {
					if (xhr.status === 200) {
						let response = JSON.parse(xhr.responseText)
						app.user = response
						console.log(xhr.responseText)
					} else if (xhr.status === 401) {
						app.token = ''
						sessionStorage.removeItem('token')
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
.side-bar {
	position: fixed;
	top: var(--header-height);
	left: 0;
	width: 10%;
	height: calc(100vh - var(--header-height));
	background: #f4f4f4;
	z-index: 1;
	display: flex;
	flex-direction: column;
	justify-content: flex-start;
	padding: 5% .5em 0 0;
}

ul {
	margin: 0;
	padding: 0 0;
}

ul li {
	list-style: none;
	text-align: end;
}

li + li {
	margin-top: .3em;
}

li a {
	display: inline-block;
	width: 100%;
	font-size: 16px;
	text-decoration: none;
	text-transform: uppercase;
	font-weight: lighter;
	letter-spacing: 100;
	color: #000;
	padding: .5em 0;
	padding-right: .5em;
}

li a:hover {
	background-color: #ccc;
}
</style>
