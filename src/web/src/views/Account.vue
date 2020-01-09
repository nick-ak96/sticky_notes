<template>
	<div>
		<div class="menu">
			<ul>
				<li>
					<router-link :to="{ path: `/account/${id}` }">My notes</router-link>
				</li>
				<li>
					<router-link :to="{ path: `/account/${id}/shared` }">Shared notes</router-link>
				</li>
				<li>
					<router-link :to="{ path: `/account/${id}/org` }">Organizations</router-link>
				</li>
				<li>
					<router-link :to="{ path: `/account/${id}/edit` }">Account settings</router-link>
				</li>
			</ul>
		</div>
		<div>
			<router-view />
		</div>
	</div>
</template>

<script>
export default {
	props: [
		'id'
	]
}
</script>

<style scoped>
.menu {
}

ul, li {
	list-style: none;
}

li {
	display: inline-block;
	margin: 3em 3em;
}

li a {
	text-decoration: none;
	color: #000;
	padding: 1em 2em;
	border-radius: 10px;
}

li a.router-link-exact-active {
	box-shadow: 1px 0px 5px 1px rgba(0, 0, 0, .2);
}
</style>
