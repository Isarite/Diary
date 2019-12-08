<template>
    <v-container fluid>
        <v-slide-y-transition mode="out-in">
            <Login/>
        </v-slide-y-transition>
    </v-container>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import Login from '@/components/Login.vue';

    @Component({
        components: { Login },
    })
    export default class LoginView extends Vue {}
</script>


