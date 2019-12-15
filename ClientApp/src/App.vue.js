import { __decorate } from "tslib";
import HelloWorld from '@/components/HelloWorld.vue';
import { Component, Vue } from 'vue-property-decorator';
import axios from "axios";
axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
axios.defaults.baseURL = `http://localhost:5000`;
let App = class App extends Vue {
    constructor() {
        super(...arguments);
        this.clipped = true;
        this.drawer = true;
        this.miniVariant = false;
        this.right = true;
        this.title = 'Diary App';
        this.items = [
            { title: 'Home', icon: 'home', link: '/' },
            { title: 'Counter', icon: 'touch_app', link: '/counter' },
            { title: 'Fetch data', icon: 'get_app', link: '/fetch-data' },
            { title: 'Users', icon: 'supervisor_account', link: '/fetch-users' },
            { title: 'Login', icon: 'supervisor_account', link: '/login' },
            { title: 'Register', icon: 'supervisor_account', link: '/register' }
        ];
    }
};
App = __decorate([
    Component({
        components: { HelloWorld },
    })
], App);
export default App;
//# sourceMappingURL=App.vue.js.map