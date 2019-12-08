<template>
    <v-container fluid>
        <v-slide-y-transition mode="out-in">
            <v-row>
                <v-col>
                    <h1>Your Diaries</h1>
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
                        <template v-slot:items="props">
                            <td><a href="http://example.com">
                                {{ props.item.name}}</a></td>
                            <td>{{ props.item.created }}</td>
                            <td>{{ props.item.edited}}</td>
                        </template>
                    </v-data-table>
                </v-col>
            </v-row>
        </v-slide-y-transition>

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
                    { text: 'Edited', value: 'edited'}
                ],
            };
        }


        private created() {
            this.fetchDiaries();
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
                    this.errorMessage = `Error while loading diaries: ${e.message}.`;
                })
                .finally(() => (this.loading = false));
        }
    }
</script>
