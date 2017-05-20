<template>
    <div class="container">
        <form>
            <div class="row">
                <div class="col-xs-12 col-sm-8 col-sm-offset-2 col-md-6 col-md-offset-3">
                    <!-- Exercise 1 -->
                    <!-- Create a Signup Form where you retrieve the following Information -->
                    <!-- Full Name (First Name + Last Name) -->
                    <!-- Mail -->
                    <!-- Password -->
                    <!-- Store Data? Yes/No -->
                    <h1>Form</h1>
                    <hr>
                    <!--
                                         <div class="form-group">
                                            <label for="fullname">Fullname</label>
                                            <input type="text"
                                                   id="fullname"
                                                   class="form-control"
                                                   v-model="fullName">
                                        </div>
                                    -->
                    <app-fullname v-model="fullName"></app-fullname>
    
                    <div class="form-group">
                        <label for="email">Mail</label>
                        <input type="text"
                               id="email"
                               class="form-control"
                               v-model="email">
                    </div>
                    <div class="form-group">
                        <label for="password">Password</label>
                        <input type="password"
                               id="password"
                               class="form-control"
                               v-model="password">
                    </div>
                    <div class="form-group">
                        <label for="store">
                            <input type="checkbox"
                                   id="store"
                                   v-model="store"> Store
                        </label>
                    </div>
    
                    <!-- Exercise 2 -->
                    <!-- Only display the Form if it has NOT been submitted -->
                    <!-- Display the Data Summary ONCE the Form HAS been submitted -->
    
                    <!-- Exercise 3 -->
                    <!-- Edit the Example from above and create a custom "Full Name" Control -->
                    <!-- which still holds the First Name and Last Name Input Field -->
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-8 col-sm-offset-2 col-md-6 col-md-offset-3">
                    <button @click.prevent="submitForm"
                            class="btn btn-primary">Submit!
                    </button>
                </div>
            </div>
        </form>
        <hr>
        <div class="row"
             v-if="isSubmitted">
            <div class="col-xs-12 col-sm-8 col-sm-offset-2 col-md-6 col-md-offset-3">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4>Your Data</h4>
                    </div>
                    <div class="panel-body">
                        <p>Full Name: {{ fullName }} </p>
                        <p>Mail: {{email}} </p>
                        <p>Password: {{password}} </p>
                        <p>Store in Database?: {{store}} </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import Fullname from './FullName.vue'

export default {
    data() {
        return {
            fullName: 'teste',
            email: 'teste',
            password: 'padrao',
            store: true,
            isSubmitted: false
        }
    },
    methods: {
        submitForm() {
            this.isSubmitted = true;
        }
    },
    components: {
        'appFullname': Fullname
    }
}
</script>

<style>

</style>
