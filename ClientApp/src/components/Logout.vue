<template>
    <div id="app">
        <v-app id="inspire">
            <v-content>
                <v-container
                        fluid
                        fill-height
                >
                    <v-layout
                            align-center
                            justify-center
                    >
                        <v-flex
                                xs12
                                sm8
                                md4
                        >
                            <v-card class="elevation-12">
                                <v-toolbar
                                        color="red"
                                        dark
                                        flat
                                >
                                    <v-toolbar-title justify="center">Are you sure you want to log out?</v-toolbar-title>
                                    <v-spacer/>
                                </v-toolbar>
                                <v-card-actions>
                                    <v-spacer/>
                                    <v-row>
                                        <v-col justify="end"><v-btn class="ma-2" dark color="green" @click.local="logout">Yes</v-btn></v-col>
                                        <v-col justify="start"><v-btn class="ma-2" dark color="red" @click.local="back">No</v-btn></v-col>
                                    </v-row>
                                </v-card-actions>
                            </v-card>
                        </v-flex>
                    </v-layout>
                </v-container>
            </v-content>
        </v-app>
    </div>
</template>



<script lang = "ts">
import { mapGetters, mapActions } from 'vuex';
import router from '@/router';
import {Action, Getter} from 'vuex-class';
import { Component, Vue} from 'vue-property-decorator';
import {user} from '@/store/user';
import axios from 'axios';
import {Token} from '@/models/Token';

const namespace: string = 'user';


@Component
export default class Logout extends Vue {
    @Getter('currentJWT', { namespace })
    private currentJWT!: string;
    @Action('fetchJWT', { namespace })
    private fetchJWT: any;

    public data() {
        return {
            show1: false,
            showAlert: false,
        };
    }

    private logout() {
        localStorage.removeItem('token');
        localStorage.removeItem('role');
        localStorage.setItem('loggedIn', 'false');
        router.push('/');
        window.location.reload();
    }

    private back() {
        router.push('/');
    }






}
</script>