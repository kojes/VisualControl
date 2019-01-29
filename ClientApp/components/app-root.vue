<template>
  <div id="main__app">
    <v-app dark>
      <v-navigation-drawer fixed
                          
                           v-model="drawer"
                           app>
        <v-toolbar flat class="transparent">
          <v-list>
            <v-list-tile avatar>
              <v-list-tile-avatar>
                <avatar :username="username"
                        :size="40">
                </avatar>
              </v-list-tile-avatar>

              <v-list-tile-content>
                <v-list-tile-title>{{username}}</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>

          </v-list>
        
        </v-toolbar>
        <v-divider class="pt-0"></v-divider>
        <v-list v-for="(route, index) in routes.filter(x=>x.nav === true)" :key="index" >
         
            <v-list-tile ripple :to="route.path">
              <v-list-tile-action>
                <v-icon>{{route.icon}}</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>{{route.display}}</v-list-tile-title>
               </v-list-tile-content>
            </v-list-tile>
         
        </v-list>
      </v-navigation-drawer>
      <v-toolbar color="indigo" fixed app>
        <v-toolbar-side-icon @click.stop="drawer = !drawer"><v-icon>drag_indicator</v-icon></v-toolbar-side-icon>
        <v-toolbar-title>Система визуального контроля</v-toolbar-title>
      </v-toolbar>
      <v-content>
       
          <v-container fluid>
            
              <v-layout justify-start
                        align-center>
                <router-view></router-view>
              </v-layout>
       
</v-container>
         
</v-content>

    </v-app>
  </div>
</template>

<script>
   import { routes } from '../router/routes'
  import Avatar from 'vue-avatar'
    export default {


     components:
     {
        Avatar
     },

      data () {
        return {
          routes,
          drawer: null,
          username: "Sergei Strelnikov"
        }
      }
    }
</script>

<style>

</style>
