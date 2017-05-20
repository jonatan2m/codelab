<template>
        <div class="row">
            <div class="col-xs-12">
                <footer>
                    <p>All Servers are managed here</p>
                    <p>Age: {{ userAge }}</p>
                     <button @click="editAge">Change Age</button>
                     <button @click="incrementAgeFn()">Change Age 2</button>                     
                </footer>
            </div>
        </div>
</template>

<script>
    import { eventBus } from '../../main';

    export default {
        props: {
            userAge: Number,
            incrementAgeFn: Function
        },
        methods: {
            editAge() {                
                //this.$emit('ageWasEdited', 30);
                //eventBus.$emit('ageWasEdited', 30);                
                eventBus.changeAge(this.userAge);
            }
        },
        created() {
            eventBus.$on('ageWasEdited', (age) => {
                this.userAge = age;                   
            });
        }
    }
</script>

<style>

</style>
