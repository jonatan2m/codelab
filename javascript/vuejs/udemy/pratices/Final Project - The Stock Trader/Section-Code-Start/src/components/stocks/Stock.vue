<template>
    <div class="col-sm-6 col-md-4">
        <div class="panel panel-success">
            <div class="panel-heading">
                <h3 class="panel-title">{{stock.name}} <small>(price: {{stock.price}})</small></h3>
            </div>
            <div class="panel-body">
                <div class="pull-left">
                    <input type="number" v-model="quantity" :class="{danger: insufficentFunds}" class="form-control" placeholder="Quantity">
                </div>
                <div class="pull-right">
                    <button class="btn btn-success" :disabled="insufficentFunds || quantity <= 0" @click="buyStock">
                        {{ insufficentFunds ? "insufficient Funds": "Buy" }}
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.danger {
    border: 1px solid red;
}
</style>

<script>
export default {
    props: ['stock'],
    data() {
        return {
            quantity: 0
        }
    },
    computed: {
        funds() {
            return this.$store.getters.funds;
        },
        insufficentFunds() {
            return this.quantity * this.stock.price > this.funds
        }
    },
    methods: {
        buyStock() {
            const order = {
                stockId: this.stock.id,
                stockPrice: this.stock.price,
                quantity: this.quantity
            };
            this.$store.dispatch('buyStocks', order);
            this.quantity = 0;
        }
    }
}    
</script>