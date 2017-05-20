<template>
    <div class="container">
        <div class="row">
            <div class="col-xs-12 col-sm-8 col-sm-offset-2 col-md-6 col-md-offset-3">
                <h1>Filters & Mixins</h1>
                <!-- Exercise 1) -->
                <!-- Build a local Filter which reverses the Text it is applied on -->
                <input type="text"
                       v-model="myText" />
                <p>{{ myText | reverse }}</p>
    
                <!-- Exercise 2 -->
                <!-- Build a global Filter which counts the length of a word and it appends it -->
                <!-- Like this: "Test" => Gets Filtered to => "Test (4)" -->
                <p>{{ myText | count-char}}</p>
    
                <!-- Exercise 3 -->
                <!-- Do the same as in Exercises 1 & 2, now with Computed Properties -->
                <hr>
                <p>{{ reverseText }} </p>
                <p>{{ countCharsText }}</p>
    
                <!-- Exercise 4 -->
                <!-- Share the Computed Property rebuilding Exercise 2 via a Mixin -->
                <hr>
    
                <p>{{ countChar }}</p>
    
            </div>
        </div>
    </div>
</template>

<script>
import { countCharMixin } from './countCharMixin'

export default {
    mixins: [countCharMixin],
    data() {
        return {
            myText: 'Default'
        }
    },
    computed: {
        reverseText() {
            return this.myText.split("").reverse().join("");
        },
        countCharsText() {
            return `${this.myText} (${this.myText.length})`;
        }
    },
    filters: {
        reverse(value) {
            return value.split("").reverse().join("");
        }
    }
}
</script>

<style>

</style>
