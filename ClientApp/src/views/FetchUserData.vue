<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-row>
        <v-col>
          <h1>Users</h1>
          <p>This component demonstrates fetching data from the server.</p>

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
              <td>{{ props.item.id }}</td>
              <td>{{ props.item.username}}</td>
              <td>{{ props.item.role }}</td>
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
import { User } from '@/models/User';
import axios from 'axios';

@Component({})
export default class FetchUserDataView extends Vue {
  private loading: boolean = true;
  private showError: boolean = false;
  private errorMessage: string = 'Error while loading weather forecast.';
  data() {
    return {
      users: [],
      selected: [],
      headers : [
      { text: 'Id', value: 'id' },
      { text: 'User name', value: 'name' },
      { text: 'User role', value: 'role' },
    ],
    };
  }


  private created() {
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
    let users = this.$data.users;
    this.$data.users.forEach( function(item:User, i:number) {
      users[i].selected = true;
    });
    this.$data.users = users;
    this.$data.users.map((u:User) => u.selected = false);
  }
}
</script>
