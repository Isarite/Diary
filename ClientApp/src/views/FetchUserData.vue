<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-row>
        <v-col>
          <h1>Users</h1>
          <v-data-table
            :headers="headers"
            :items="users"
            hide-default-footer
            :loading="loading"
            class="elevation-1"
            v-model="selected"
            item-key="id"
            show-select
          >
            <v-progress-linear v-slot:progress color="blue" indeterminate></v-progress-linear>
            <template v-slot:items="props">
              <v-checkbox
                      v-model="props.selected"
                      :disabled="!props.selected && selected.length != 0"
                      :indeterminate="!props.selected && selected.length != 0"
              />
              <td>{{ props.item.id }}</td>
              <td>{{ props.item.username}}</td>
              <td>{{ props.item.role }}</td>
            </template>
          </v-data-table>
        </v-col>
      </v-row>
    </v-slide-y-transition>
    <v-spacer/>
    <v-btn class="ma-2" color="primary" @click.local="deleteUsers">Delete</v-btn>
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
import { User } from '@/models/User';
import axios from 'axios';
import router from "@/router";

@Component({})
export default class FetchUserDataView extends Vue {
  private loading: boolean = true;
  private showError: boolean = false;
  private errorMessage: string = 'Error while loading users.';
  data() {
    return {
      users: [],
      selected: [],
      msg: "",
      headers : [
      { text: 'Id', value: 'id' },
      { text: 'User name', value: 'name' },
      { text: 'User role', value: 'role' },
    ],
    };
  }


  private created() {
    let authorized:string = localStorage.getItem('loggedIn') || "";
    let role:string = localStorage.getItem('role') || "";
    if("true" != authorized){
      router.push('/login');
      return;
    }else if("Administrator" != role){
      router.push('/');
      return;
    }
    this.fetchUsers();
  }

  private fetchUsers() {
    axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
    axios
      .get<User[]>('api/Users/')
      .then((response) => {
        this.$data.users = response.data;
      })
      .catch((e) => {
        this.showError = true;
        this.errorMessage = `Error while loading users: ${e.message}.`;
      })
      .finally(() => (this.loading = false));
  }

  private deleteUsers() {
    let users = this.$data.users;
    this.$data.msg = this.$data.selected[0].name;
    this.$data.selected.forEach( function (item:User) {
      axios
              .delete<User[]>('api/Users/' + item.id)
              .then((response) => {
              })
    });
    this.fetchUsers();
  }
  
  private deleteUser(id:string){
    axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
    axios
            .delete<User[]>('api/Users/' + id)
            .then((response) => {
            })
            .catch((e) => {
              this.showError = true;
              this.errorMessage = `Error while loading users: ${e.message}.`;
            });
  }
}
</script>
