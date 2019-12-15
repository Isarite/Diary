<template>
  <v-app>

    <v-navigation-drawer persistent :mini-variant="miniVariant" :clipped="clipped" v-model="drawer" enable-resize-watcher fixed app>
      <v-list :key="key">
        <v-list-item value="true" v-for="(item, i) in items" :key="i" :to="item.link">
          <v-list-item-action>
            <v-icon v-html="item.icon"></v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title v-text="item.title"></v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar app :clipped-left="clipped" color="info" dark>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-btn class="d-none d-lg-flex" icon @click.stop="miniVariant = !miniVariant">
        <v-icon v-html="miniVariant ? 'chevron_right' : 'chevron_left'"></v-icon>
      </v-btn>
      <v-btn class="d-none d-lg-flex" icon @click.stop="clipped = !clipped">
        <v-icon>web</v-icon>
      </v-btn>
      <v-toolbar-title v-text="title"></v-toolbar-title>
      <v-spacer></v-spacer>
      <v-menu
              bottom
              left
              color="info" light
      >
        <template v-slot:activator="{ on }">
          <v-btn
                  icon
                  color="yellow"
                  v-on="on"
          >
            <v-icon>mdi-dots-vertical</v-icon>
          </v-btn>
        </template>

        <v-card v-if="$data.loggedIn"
                class="mx-auto"
                max-width="300"
                tile
        >
          <v-img
                  height="50%"
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
                    <v-list-item-title  class="title">{{$data.username}}</v-list-item-title>
                    <v-list-item-subtitle>{{$data.role}}</v-list-item-subtitle>
                  </v-list-item-content>
                </v-list-item>
              </v-col>
            </v-row>
          </v-img>
        </v-card>
        <v-list>
          <v-list-item
                  v-list-item value="true" v-for="(item, i) in loggedIn2" :key="i" :to="item.link"
          >
            <v-list-item-title>{{ item.title }}</v-list-item-title>
            <v-list-item-action>
              <v-icon v-html="item.icon"></v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title v-text="item.title"></v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>
    



    <v-content>
      <router-view/>
    </v-content>

    <v-footer app>
      <span>&nbsp;Domas Jurevičius, IFF-6/4, 2019</span>
    </v-footer>

  </v-app>
</template>

<script lang="ts">
import HelloWorld from '@/components/HelloWorld.vue';
import { Component, Vue } from 'vue-property-decorator';
import axios from "axios";
import JwtDecode from "jwt-decode";
import router from "@/router";
import {Token} from "@/models/Token";
import Timeout = NodeJS.Timeout;
import {User} from "@/models/User";
axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
axios.defaults.baseURL = `https://diaryappapp.azurewebsites.net/`;
if(localStorage.getItem('loggedIn') ===null)
  localStorage.setItem('loggedIn', "false");
var token:string = localStorage.getItem('token') || "";

try{var jwt_decode:any =  JwtDecode(token);
localStorage.setItem('role', jwt_decode['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']);
console.log(jwt_decode);
localStorage.setItem('refresh',"-1");
}
catch{}
@Component({
  components: { HelloWorld },
})
export default class App extends Vue {
  data() {
    return {
      key: 0,
      refresh: null,
      role: "Guest",
      username: "No username",
    };
  }
  private clipped: boolean = true;
  private clipped2: boolean = true;
  private drawer: boolean = true;
  private drawer2: boolean = true;
  private miniVariant: boolean = false;
  private right: boolean = true;
  private title: string = 'Diary App';

  private items1 = [
    { title: 'Home', icon: 'home', link: '/' },
  ];
  private role:string = localStorage.getItem('role') || "";
  private isAdmin:boolean = this.role == "Administrator";
  private logdIn:string = localStorage.getItem('loggedIn')||"";
  private refresh = setInterval(function (){});
  private a = this.logdIn  == "true" ? this.ready() : clearInterval(this.refresh);
  private items2 = (localStorage.getItem('loggedIn') == "false") ? [] : [{ title: 'Your Diaries', icon: 'book', link: '/fetch-diaries' },
    ];
  private items3 = this.isAdmin ?
          this.items2.concat([{ title: 'Users', icon: 'supervisor_account', link: '/fetch-users' }]) 
          : this.items2.concat([]);
  private items = this.items1.concat(this.items3);
  private loggedIn2 = (localStorage.getItem('loggedIn') == "false") ? [{ title: 'Login', icon: 'supervisor_account', link: '/login'},    
            { title: 'Register', icon: 'supervisor_account', link: '/register'}] : 
          [{title: 'Log Out', icon: 'supervisor_account', link: '/logout'}];

  created(){
    this.$route.meta.authorized = false;
    this.$route.meta.admin = false;
    this.getUser();
  }
  
  ready() {
    var self = this;
    var number = setInterval(function () {
      self.refreshToken();
    }.bind(this), 30 * 60 * 1000);//refreshes every 30 min
    localStorage.setItem('refresh', '1');
  }
  
  getUser(){
    if(localStorage.getItem('loggedIn')!= "true"){
      this.$data.loggedIn = false;
      return;
    }
    this.$data.loggedIn = true;
    axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
    axios
            .get<User>('api/Users/GetUser')
            .then((response) => {
              this.$data.username = response.data.name;
              this.$data.role = response.data.role;
            });
  }
  
  public refreshToken(){
    axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
    axios
            .put<Token>('api/Users/RefreshToken/'
            ).
    then((response) => {
      localStorage.setItem('token', response.data.token); // store the token in localstorage
      localStorage.setItem('loggedIn', "true"); // store the token in localstorage
      axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
      this.$data.showAlert = false;
      this.$route.meta.admin = true;
      this.$route.meta.authorized = true;
      console.log("success");
    }).catch( error =>
    {console.log(error.response);
      localStorage.removeItem('token');
      localStorage.setItem('loggedIn', "false"); // store the token in localstorage
      axios.defaults.headers.common['Authorization'] = ``;
      this.$data.showAlert = true;
      console.log("failure");
    });
  }
  
  
  
}
</script>
