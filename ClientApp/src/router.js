import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';
Vue.use(Router);
export default new Router({
    mode: 'history',
    base: process.env.BASE_URL,
    routes: [
        {
            path: '/',
            name: 'home',
            component: Home,
        },
        {
            path: '/counter',
            name: 'counter',
            // route level code-splitting
            // this generates a separate chunk (about.[hash].js) for this route
            // which is lazy-loaded when the route is visited.
            component: () => import(/* webpackChunkName: "counter" */ './views/Counter.vue'),
        },
        {
            path: '/fetch-data',
            name: 'fetch-data',
            component: () => import(/* webpackChunkName: "fetch-data" */ './views/FetchData.vue'),
        },
        {
            path: '/fetch-users',
            name: 'fetch-users',
            component: () => import(/* webpackChunkName: "fetch-users" */ './views/FetchUserData.vue'),
        },
        {
            path: '/login',
            name: 'login',
            component: () => import(/* webpackChunkName: "fetch-users" */ './views/Login.vue'),
        },
        {
            path: '/register',
            name: 'register',
            component: () => import(/* webpackChunkName: "fetch-users" */ './views/Register.vue'),
        },
        {
            path: '/fetch-diaries',
            name: 'fetch-diaries',
            component: () => import(/* webpackChunkName: "fetch-users" */ './views/FetchDiaries.vue'),
        },
    ],
});
//# sourceMappingURL=router.js.map