import { __decorate } from "tslib";
import { Component, Vue } from 'vue-property-decorator';
import axios from 'axios';
let FetchUserDataView = class FetchUserDataView extends Vue {
    constructor() {
        super(...arguments);
        this.loading = true;
        this.showError = false;
        this.errorMessage = 'Error while loading weather forecast.';
    }
    data() {
        return {
            users: [],
            selected: [],
            headers: [
                { text: 'Id', value: 'id' },
                { text: 'User name', value: 'name' },
                { text: 'User role', value: 'role' },
                { text: 'Selected', value: 'selected' }
            ],
        };
    }
    created() {
        this.fetchUsers();
    }
    fetchUsers() {
        axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
        axios
            .get('api/Users/')
            .then((response) => {
            this.$data.users = response.data;
        })
            .catch((e) => {
            this.showError = true;
            this.errorMessage = `Error while loading users: ${e.message}.`;
        })
            .finally(() => (this.loading = false));
        let users = this.$data.users;
        this.$data.users.forEach(function (item, i) {
            users[i].selected = true;
        });
        this.$data.users = users;
        this.$data.users.map((u) => u.selected = false);
    }
};
FetchUserDataView = __decorate([
    Component({})
], FetchUserDataView);
export default FetchUserDataView;
//# sourceMappingURL=FetchUserData.vue.js.map