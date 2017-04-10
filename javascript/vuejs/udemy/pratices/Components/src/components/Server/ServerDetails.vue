<template>
    <div class="col-xs-12 col-sm-6">
        <p>Server Details are currently not updated</p>
        <p>Server Name: {{ myName }} ({{ switchName() }}) </p>
        <button @click="resetName">Reset Name</button>
    </div>

</template>

<script>
    export default {
//        props: ['myName'],
        props: {
            myName: String,            
            default: 'Hello John'
        },
        methods: {
            switchName() {
                return this.myName.split("").reverse().join("");
            },
            resetName() {
                this.myName = "Jonatan";
                this.$emit('nameWasReset', this.myName);
            }
        }
    }
</script>

<style scoped>
    div {
        border: 1px solid red;
    }
</style>
