<template>
    <div class="container">
        <app-header></app-header>
        <hr>
        <div class="row">
            <servers></servers>
            <button @click="changeName">Change my name</button>
            <p>Original name: {{ name }}</p> 
            <app-server-details v-bind:myName="name" v-on:nameWasReset="name = $event"></app-server-details>
            
        </div>
        <hr>
        <app-footer></app-footer>
    </div>
<!-- 
    In the last lecture I introduced one possible folder structure. It's a great structure in small and medium sized projects.
    Whilst it also might work fine in bigger projects, there's also an alternative you might want to consider.
    Instead of having your components in a components/ folder (and storing other shared files in other folders - e.g. shared/), you can also group your files by feature.
    This could look like this:
    - main.js
    - users/
    - - account/
    - - analytics/
    - shop
    - - main/
    - - checkout/

    Module Resources & Useful Links
    Section 7, Lecture 102
    The Code used in this Module can be found attached to this Lecture!

    If you want to learn more about VueJS Components, you may want to have a look at this Article from the official Docs (important: It does cover Topics which will be taught in the next Course Modules!): http://vuejs.org/guide/components.html

    Resources for this lecture
    Section Code (Before better Structure).zip
    Section Code (After better Structure).zip
-->
</template>

<script>
    import Header from './components/Shared/Header.vue';
    import Footer from './components/Shared/Footer.vue';
    import Servers from './components/Server/Servers.vue';
    import ServerDetails from './components/Server/ServerDetails.vue';

    export default {
        data: function() {
            return {
                name: 'Jonatan'
            }
        },
        methods: {
            changeName() {
                this.name = "Joe";
            }
        },
        components: {
            appHeader: Header,
            Servers,
            'app-server-details': ServerDetails,
            'app-footer': Footer
        }
    }
</script>

<style>

</style>
