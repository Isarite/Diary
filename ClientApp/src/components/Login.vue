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
                                        color="primary"
                                        dark
                                        flat
                                >
                                    <v-toolbar-title>Login</v-toolbar-title>
                                    <v-spacer/>
                                </v-toolbar>
                                <v-card-text>
                                    <v-form>
                                        <p><v-text-field v-model = "username" prepend-icon="person" placeholder="User name"/></p>

                                        <v-text-field v-model = "password":append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
                                                      :type="show1 ? 'text' : 'password'"
                                                      @click:append="show1 = !show1"
                                                      prepend-icon="lock"
                                                      placeholder="Password"/>
                                    </v-form>
                                </v-card-text>
                                <v-card-actions>
                                    <v-spacer/>
                                    <v-btn class="ma-2" color="primary" @click.local="fetch">Login</v-btn>
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
    import router from "@/router";
    import {Action, Getter} from "vuex-class";
    import { Component, Vue } from 'vue-property-decorator';
    import {user} from "@/store/user";
    import axios from "axios";
    import {Token} from "@/models/Token";

    const namespace: string = 'user';


    @Component
    export default class Login extends Vue{
        @Getter('currentJWT', { namespace })
        private currentJWT!: string;
        @Action('fetchJWT', { namespace })
        private fetchJWT: any;

        data() {
            return {
                username: "",
                password: "",
                show1: false,
                token: localStorage.getItem('token'),
            };
        }

        private fetch() {
            //this.$store.dispatch('user/fetchJWT', {username: "admin", password: "12345"});
            axios
                .put<Token>('api/Users/RequestToken/' + this.$data.username,
                    this.$data.password,
                    {headers: {"Content-Type": "application/json"}}
                ).
            then((response) => {
                if(response.status == 200) {
                    localStorage.setItem('token', response.data.token); // store the token in localstorage
                    axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
                    router.push('/');
                }else{
                    localStorage.removeItem('token'); // store the token in localstorage
                    axios.defaults.headers.common['Authorization'] = `Bearer ${null}`;
                }
            });
        }

    }
</script>