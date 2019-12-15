<template class="change-font">
    <div id="app">
        <v-expand-transition>
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
                                        class="change-font"
                                >
                                    <v-toolbar-title>Login</v-toolbar-title>
                                    <v-spacer/>
                                </v-toolbar>
                                <v-card-text class="change-font">
                                    <v-form>
                                        <v-alert type="error" v-model ="showAlert">
                                            Login failed. Check your username and password.
                                        </v-alert>
                                        <p><v-text-field v-model = "username" prepend-icon="person" placeholder="User name"/></p>

                                        <v-text-field v-model = "password":append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
                                                      :type="show1 ? 'text' : 'password'"
                                                      @click:append="show1 = !show1"
                                                      prepend-icon="lock"
                                                      placeholder="Password"/>
                                    </v-form>
                                </v-card-text>
                                <v-card-actions class="change-font">
                                    <v-spacer/>
                                    <v-btn class="ma-2" color="primary" @click.local="fetch">Login</v-btn>
                                </v-card-actions>
                            </v-card>
                        </v-flex>
                    </v-layout>
                </v-container>
            </v-content>
        </v-app>
    </v-expand-transition>
    </div>
</template>



<script lang = "ts">
    import { mapGetters, mapActions } from 'vuex';
    import router from "@/router";
    import {Action, Getter} from "vuex-class";
    import { Component,Vue} from 'vue-property-decorator';
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
                showAlert: false,
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
                    localStorage.setItem('token', response.data.token); // store the token in localstorage
                localStorage.setItem('token', response.data.token); // store the token in localstorage
                localStorage.setItem('loggedIn', "true"); // store the token in localstorage
                axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
                    this.$data.showAlert = false;
                this.$route.meta.admin = true;
                this.$route.meta.authorized = true;
                window.location.reload();
                router.push("/")
            }).catch( error =>
            {console.log(error.response);
                localStorage.removeItem('token');
                localStorage.setItem('loggedIn', "false"); // store the token in localstorage
                this.$data.showAlert = true;
            });

        }

    }
</script>

<style scoped>
    .change-font {
        font-family: "Comic Sans MS","Fira Code","Consolas",serif;
    }
</style>