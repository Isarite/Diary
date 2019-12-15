<template>
    <v-row justify="space-around">
        <v-card
                class="mx-auto"
                max-width="434"
                tile
        >
            <v-img
                    height="100%"
                    src="https://cdn.vuetifyjs.com/images/cards/server-room.jpg"
            >
                <v-row
                        align="end"
                        class="fill-height"
                >
                    <v-col
                            align-self="start"
                            class="pa-0"
                            cols="12"
                    >
                        <v-avatar
                                class="profile"
                                color="grey"
                                size="164"
                                tile
                        >
                            <v-img src="http://akcmv.gov.lv/wp-content/uploads/2018/06/blank-profile-picture-973460_1280-2-1024x1024.png"/>
                        </v-avatar>
                    </v-col>
                    <v-col class="py-0">
                        <v-list-item
                                color="rgba(0, 0, 0, .4)"
                                dark
                        >
                            <v-list-item-content>
                                <v-list-item-title class="title">Your Username</v-list-item-title>
                                <v-list-item-subtitle>Your Role</v-list-item-subtitle>
                            </v-list-item-content>
                        </v-list-item>
                    </v-col>
                </v-row>
            </v-img>
        </v-card>
    </v-row>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import Login from '@/components/Login.vue';
import router from '@/router';
@Component({
    components: { Login },
})

export default class UserView extends Vue {
    public data() {
        return {
            loggedIn : (localStorage.getItem('loggedIn') === 'false'), // store the token in localstorage,
        };
    }
    public created() {
        const authorized: string = localStorage.getItem('loggedIn') || '';
        if ('false' == authorized) {
            router.push('/');
            return;
        }
    }
}
</script>


