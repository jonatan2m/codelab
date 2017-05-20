import Vue from 'vue'
import App from './App.vue'

//is possible centralize the common data here.
export const eventBus = new Vue({
  methods: {
    changeAge(age) {
      this.$emit('ageWasEdited', age);
    }
  }
});

new Vue({
  el: '#app',
  render: h => h(App)
})

