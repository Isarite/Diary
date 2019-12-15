<template>
    <v-container fluid>
        <v-slide-y-transition mode="out-in">
            <Logout/>
        </v-slide-y-transition>
    </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import Logout from '@/components/Logout.vue';
import router from '@/router';
@Component({
    components: { Logout },
})

export default class LogoutView extends Vue {
    public data() {
        return {
            loggedIn : (localStorage.getItem('loggedIn') === 'false'), // store the token in localstorage,
        };
    }
    public created() {
        const authorized: string = localStorage.getItem('loggedIn') || '';
        if ('true' != authorized) {
            router.push('/');
            return;
        }
    }
}
</script>


