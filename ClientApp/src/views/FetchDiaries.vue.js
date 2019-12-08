import { __decorate } from "tslib";
import { Component, Vue } from 'vue-property-decorator';
import axios from 'axios';
let FetchDiariesView = class FetchDiariesView extends Vue {
    constructor() {
        super(...arguments);
        this.loading = true;
        this.showError = false;
        this.errorMessage = 'Error while loading diaries.';
    }
    data() {
        return {
            diaries: [],
            selected: [],
            headers: [
                { text: 'Id', value: 'id' },
                { text: 'Name', value: 'name' },
                { text: 'Created', value: 'created' },
                { text: 'Edited', value: 'edited' }
            ],
        };
    }
    created() {
        this.fetchDiaries();
    }
    fetchDiaries() {
        axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
        axios
            .get('api/Diaries/')
            .then((response) => {
            this.$data.diaries = response.data;
        })
            .catch((e) => {
            this.showError = true;
            this.errorMessage = `Error while loading diaries: ${e.message}.`;
        })
            .finally(() => (this.loading = false));
    }
};
FetchDiariesView = __decorate([
    Component({})
], FetchDiariesView);
export default FetchDiariesView;
//# sourceMappingURL=FetchDiaries.vue.js.map