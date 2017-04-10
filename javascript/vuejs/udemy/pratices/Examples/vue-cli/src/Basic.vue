<template>
	<div>
		<p>[Basic component - single .Vue file] Server Status: {{ status }}</p>
		<app-servers></app-servers>
	</div>
</template>

<script>
	export default {
		data: function () {
			return {
				status: 'Critical'
			};
		}
	}
</script>

<style>
	
</style>