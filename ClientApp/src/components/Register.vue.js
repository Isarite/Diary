import { __decorate } from "tslib";
import { Component, Vue } from 'vue-property-decorator';
import axios from "axios";
const namespace = 'user';
let Register = class Register extends Vue {
    data() {
        return {
            username: "",
            password: "",
            show1: false,
            showAlert: false,
            showSuccess: false,
        };
    }
    register() {
        //this.$store.dispatch('user/fetchJWT', {username: "admin", password: "12345"});
        axios
            .post('api/Users/' + this.$data.username, this.$data.password, { headers: { "Content-Type": "application/json" } }).
            then((response) => {
            if (response.status == 201) {
                //TODO Registered alert
                this.$data.showAlert = false;
                this.$data.showSuccess = true;
            }
        }).catch(error => {
            console.log(error.response);
            this.$data.showAlert = true;
            this.$data.showSuccess = false;
        });
    }
};
Register = __decorate([
    Component
], Register);
export default Register;
//# sourceMappingURL=Register.vue.js.map