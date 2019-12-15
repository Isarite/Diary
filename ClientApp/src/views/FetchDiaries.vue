<template>
    <v-container>
        <div id="create" v-if="createDiary">
                    <v-content>
                        <v-container
                                fill-height
                        >
                            <v-layout
                                    align-start
                                    justify-center
                            >
                                <v-flex
                                        xs2
                                        sm6
                                        md6
                                >
                                    <v-card class="elevation-12">
                                        <v-toolbar
                                                color="primary"
                                                dark
                                                flat
                                        >
                                            <v-toolbar-title>Create new diary</v-toolbar-title>
                                            <v-spacer/>
                                            <v-btn icon @click.local="showCreationDialog">
                                                <v-icon>mdi-close</v-icon>
                                            </v-btn>
                                        </v-toolbar>
                                        <v-card-text>
                                            <v-form>
                                                <v-alert type="error" v-model ="showAlert">
                                                    Login failed. Check your username and password.
                                                </v-alert>
                                                <v-text-field v-model = "name" prepend-icon="book" placeholder="Name of your diary"/>
                                            </v-form>
                                        </v-card-text>
                                        <v-row justify="center" align ="center">
                                        <v-card-actions>
                                            <v-btn class="ma-2" color="primary" @click.local="create">Create</v-btn>
                                        </v-card-actions>
                                        </v-row>
                                    </v-card>
                                </v-flex>
                            </v-layout>
                        </v-container>
                    </v-content>
            </div>
            <v-row v-if="!createDiary">
                <v-col>
                    <h1>Your Diaries</h1>
                    <v-btn v-if="showCreateButton" class="ma-2" color="primary" @click.local="showCreationDialog">Create a new diary</v-btn>
                    <v-data-table
                            :headers="headers"
                            :items="diaries"
                            hide-default-footer
                            :loading="loading"
                            class="elevation-1"
                            v-model="selected"
                            item-key="id"
                            show-select
                    >
                        <v-progress-linear v-slot:progress color="blue" indeterminate></v-progress-linear>

                        <template v-slot:item.data-table-select="{ isSelected, select }">
                            <v-simple-checkbox color="green" :value="isSelected" @input="select($event)"></v-simple-checkbox>
                        </template>
                        <template v-slot:item.name="{ item }">
                            <v-btn class="ma-2" color="primary" @click.local="fetchDiary(item.id)">{{item.name}}</v-btn>
                        </template>
                        <template v-slot:item.created="{ item }">
                            {{item.created.toString().substring(0,10) + " " + item.created.toString().substring(11,16)}}
                        </template>
                        <template v-slot:item.edited="{ item }">
                            {{item.edited.toString().substring(0,10) + " " + item.edited.toString().substring(11,16)}}
                        </template>
                    </v-data-table>
                </v-col>
            </v-row>
        <v-btn v-if="!createDiary" class="ma-2" color="primary" @click.local="deleteDiaries">Delete</v-btn>
        <v-alert
                :value="showError"
                type="error"
                v-text="errorMessage"
        >
            This is an error alert.
        </v-alert>

        <v-alert
                :value="showError"
                type="warning"
        >
            Are you sure you're using ASP.NET Core endpoint? (default at <a href="http://localhost:5000/fetch-data">http://localhost:5000</a>)<br>
            API call would fail with status code 404 when calling from Vue app (default at <a href="http://localhost:8080/fetch-data">http://localhost:8080</a>) without devServer proxy settings in vue.config.js file.
        </v-alert>

    </v-container>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import { Diary } from '@/models/Diary';
    import axios from 'axios';
    import {User} from "@/models/User";
    import router from "@/router";
    import Login from "@/components/Login.vue";
    
    @Component({})
    export default class FetchDiariesView extends Vue {
        private loading: boolean = true;
        private showError: boolean = false;
        private errorMessage: string = 'Error while loading diaries.';
        data() {
            return {
                diaries: [],
                selected: [],
                headers : [
                    { text: 'Name', value: 'name' },
                    { text: 'Created', value: 'created' },
                    { text: 'Edited', value: 'edited'},
                ],
                createDiary: false,
                showCreateButton: true,
                showAlert: false,
                name: "",
            };
        }


        private created() {
            let authorized:string = localStorage.getItem('loggedIn') || "";
            if("true" != authorized){
                router.push('/login');
                return;
            }
            this.fetchDiaries();
        }
        
        private showCreationDialog(){
            this.$data.createDiary = !this.$data.createDiary;
            this.$data.showCreateButton = !this.$data.showCreateButton;
        }

        private fetchDiaries() {
            axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
            axios
                .get<Diary[]>('api/Diaries/GetDiaries')
                .then((response) => {
                    this.$data.diaries = response.data;
                })
                .catch((e) => {
                    this.showError = true;
                    console.log(e.response);
                    this.errorMessage = `Error while loading diaries: ${e.message} + ${e.response.data}.`;
                })
                .finally(() => (this.loading = false));
        }
        
        private async create(){
            axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
            await axios
                .post<Diary>('api/Diaries',
                    "\""+this.$data.name+"\"",
                    {headers: {"Content-Type": "application/json"}})
                .catch((e) => {
                    this.showError = true;
                    this.errorMessage = `Error while creating diary ${this.$data.name}: ${e.message + e.response.data[0]}.`;
                    console.log(e.response.data);
                })
                .finally(() => (this.loading = false));
            this.fetchDiaries();
            this.showCreationDialog();
        }

        private async deleteDiaries() {
            let diaries = this.$data.diaries;
            this.$data.msg = this.$data.selected[0].name;
            await this.$data.selected.forEach( function (item:Diary) {
                axios
                    .delete<Diary[]>('api/Diaries/' + item.id)
                    .then((response) => {
                    })
            });
            this.fetchDiaries();
        }
        
        
        private fetchDiary(id:any){
            router.push("/fetch-diary/"+id);
        }
    }
</script>
