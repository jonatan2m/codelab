<template>
    <div>
        <slot name="title">        
        </slot>
        <hr>   
        <slot name="description">        
        </slot>   
        <button @click="counter++;" >Increment!</button>
        {{ counter }}
    </div>
</template>

<script>
    export default {
        data: function() {
            return {
                counter: 0
            }
        }
    }
</script>
<style scoped>
    div {
        border: 1px solid red;
        background-color: lightcoral;
        padding: 30px;
        margin: 20px auto;
        text-align: center
    }
</style>