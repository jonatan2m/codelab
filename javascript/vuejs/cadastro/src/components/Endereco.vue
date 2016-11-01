<template>
	<div class="endereco">
		<h1>{{message}}</h1>

		<div>CEP: <input type="text" v-model="cep" @keyup="buscar" placeholder="CEP"></div>
		<p style="display:none;" v-show="naoEncontrado">CEP não encontrado</p>
		<div>Rua: <input type="text" v-model="endereco.logradouro" placeholder="Endereço"></div>
		<div>Bairro: <input type="text" v-model="endereco.bairro" placeholder="Bairro"></div>
		<div>Número: <input type="text" v-model="endereco.numero" ref="numero" placeholder="Número"></div>
	</div>
</template>
<style>
	.endereco input[type=text] {
		font-size: 21px;
	}
</style>
<script>
	import axios from 'axios'

	export default {		
		data (){
			return {
				message: 'Olá mundo',
				cep: '',
				naoEncontrado: false,
				endereco: {}
			}
		},
		attached (){
			console.log(1);
		},
		methods:{
			buscar: function (){				

				if(/^[0-9]{5}-[0-9]{3}$/.test(this.cep)){

					var self = this;
					self.naoEncontrado = false;
					axios.get('http://viacep.com.br/ws/'+self.cep+'/json')
					.then(function(res){
						var result = res.data;
						if(result.erro){
							self.naoEncontrado = true;	
							return;
						}
						
						self.$refs.numero.focus();						
						self.endereco = res.data;
					})
					.catch(function(error){
						self.naoEncontrado = true;
						console.log(error);		
					})
					
				}
				
			}
		}
	}
</script>