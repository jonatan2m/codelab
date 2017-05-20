<template>
    <div>
        <div class="form-group">
            <label for="firstName">firstName</label>
            <input type="text"
                   id="firstName"
                   class="form-control"
                   v-model="firstName"
                   @change="changeFields">
        </div>
        <div class="form-group">
            <label for="lastName">lastName</label>
            <input type="text"
                   id="lastName"
                   class="form-control"
                   v-model="lastName"
                   @change="changeFields">
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            firstName: 'primeiro',
            lastName: 'ultimo'
        }
    },
    methods: {
        changeFields() {
            this.$emit('input', this.firstName + " " + this.lastName);
        }
    },
    created() {
        this.$emit('input', this.firstName + " " + this.lastName);
    }

}
</script>

<style scoped>

</style>