<template>
	<div>
		<p>[Home.vue] Server Status: {{ status }} </p>
		<hr>
		<button @click="changeStatus">Change Status</button>
	</div>
</template>

<script>
	export default {
		data: function () {
			return {
				status: 'Critical'
			};
		},
		methods: {
			changeStatus() {
				this.status = 'Normal';
			}
		}
	}
</script>