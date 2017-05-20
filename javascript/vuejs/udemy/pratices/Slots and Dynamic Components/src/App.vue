<template>
    <div class="container">
        <div class="row">
            <div class="col-xs-12">
                <br>
                <button 
                    @click="selectedTemplate = 'appBlue'"
                    class="btn btn-primary">Load Blue Template</button>
                
                <button 
                    @click="selectedTemplate = 'appGreen'"
                    class="btn btn-success">Load Green Template</button>
                
                <button 
                    @click="selectedTemplate = 'appRed'"
                    class="btn btn-danger">Load Red Template</button>
                <hr>
                <keep-alive>
                <component :is="selectedTemplate">
                    <h3 slot="title">
                            Título 
                        </h3>
                        <p slot="description">
                            Descrição 
                            
                        </p> 
                        
                </component>
                </keep-alive>
                
            </div>
        </div>
    </div>
</template>

<script>
    import Blue from './components/Blue.vue';
    import Green from './components/Green.vue';
    import Red from './components/Red.vue';

    export default {
        data: function () {
            return {
                selectedTemplate: 'appBlue'
            }
        },
        components: {
            appBlue: Blue,
            appGreen: Green,
            appRed: Red
        }
    }
</script>

<style>
</style>
