<template>
    <v-container fluid>
        <v-slide-y-transition mode="out-in">
            <Register/>
        </v-slide-y-transition>
    </v-container>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import Register from '@/components/Register.vue';

    @Component({
        components: { Register },
    })
    export default class RegisterView extends Vue {}
</script>




