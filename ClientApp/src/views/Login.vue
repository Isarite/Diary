<template>
    <v-container fluid>
        <v-slide-y-transition v-if ="loggedIn"  mode="out-in">
            <Login/>
        </v-slide-y-transition>
    </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import Login from '@/components/Login.vue';
import router from '@/router';
@Component({
    components: { Login },
})

export default class LoginView extends Vue {
    public data() {
        return {
            loggedIn : (localStorage.getItem('loggedIn') === 'false'), // store the token in localstorage,
        };
    }
    public created() {
        const authorized: string = localStorage.getItem('loggedIn') || '';
        if ('true' == authorized) {
            router.push('/');
            return;
        }
    }
}
</script>


