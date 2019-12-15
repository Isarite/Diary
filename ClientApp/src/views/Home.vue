<template>
  <v-container fluid>
    <v-layout column align-center>
      <v-icon color="blue" x-large >book</v-icon>
    </v-layout>
    <v-slide-y-transition mode="out-in">
      <v-row>        
        <v-col align-self="center">
          <h1 align="center" class="headline">Welcome to your diary app!</h1>
          <br>
          <br>
          <ul>
            <v-label>
              On the left menu are your activites, and on the right there are your account options.
            </v-label>
          </ul>
        </v-col>
      </v-row>
    </v-slide-y-transition>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import HelloWorld from '@/components/HelloWorld.vue'; // @ is an alias to /src

@Component({
  components: {
    HelloWorld,
  },
})
export default class Home extends Vue {}
</script>
