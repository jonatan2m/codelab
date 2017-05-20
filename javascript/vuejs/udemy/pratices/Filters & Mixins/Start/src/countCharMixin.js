export const countCharMixin = {
    computed: {
        countChar() {
            return `${this.myText} (${this.myText.length})`;

        }
    }
}