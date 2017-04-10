import Vue from 'vue'
import App from './App.vue'
import BasicComponentSingleVue from './Basic.vue'
import Home from './Home.vue'

//quando se usa um objeto externo dentro do componente, todos os
//componentes usam a mesma instancia desse objeto.
//var data = { status: 'Critical'	};

Vue.component('my-cmp', {
	data: function () {
		return { status: 'Critical'	};
	},
	template: '<p>[Global component] Server Status: {{ status }} (<button @click="changeStatus">Change</button>)</p>',
	methods: {
		changeStatus: function () {
			this.status = 'Normal';
		}
	}
})

//Global Component
Vue.component('app-servers', Home);

var myCmpLocal = {
	data: function () {
		return { status: 'Critical'	};
	},
	template: '<p>[Local component] Server Status: {{ status }} (<button @click="changeStatus">Change</button>)</p>',
	methods: {
		changeStatus: function () {
			this.status = 'Normal';
		}
	}
};

new Vue({
  el: '#app',
  render: h => h(App)
})

new Vue({
  el: '#app2',
  components: {
  	'my-cmp2': myCmpLocal
  }
})

new Vue({
  el: '#app3',
  render: h => h(BasicComponentSingleVue)
})