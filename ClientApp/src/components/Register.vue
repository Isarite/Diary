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
                                        class="change-font"
                                >
                                    <v-toolbar-title>Register</v-toolbar-title>
                                    <v-spacer/>
                                </v-toolbar>
                                <v-card-text class="change-font">
                                    <v-form>
                                        <v-alert type="error" v-model ="showAlert">
                                            Register failed. Likely the account with this name already exists.
                                        </v-alert>
                                        <v-alert type="success" v-model ="showSuccess">
                                            Register was successful! You can log in now.
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
                                    <v-btn class="ma-2" color="primary" @click.local="register">Register</v-btn>
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
import { Component, Vue } from 'vue-property-decorator';
import {user} from '@/store/user';
import axios from 'axios';
import {Token} from '@/models/Token';

const namespace: string = 'user';


@Component
export default class Register extends Vue {

    public data() {
        return {
            username: '',
            password: '',
            show1: false,
            showAlert: false,
            showSuccess: false,
        };
    }

    private register() {
        // this.$store.dispatch('user/fetchJWT', {username: "admin", password: "12345"});
        axios
            .post('api/Users/' + this.$data.username,
                this.$data.password,
                {headers: {'Content-Type': 'application/json'}},
            ).
        then((response) => {
            if (response.status == 201 ) {
                // TODO Registered alert
                this.$data.showAlert = false;
                this.$data.showSuccess = true;
            }
        }).catch( (error) => {console.log(error.response);
         this.$data.showAlert = true;
         this.$data.showSuccess = false;
        },
    );
    }

}
</script>

<style scoped>
    .change-font {
        font-family: "Comic Sans MS","Fira Code","Consolas",serif;
    }
</style>