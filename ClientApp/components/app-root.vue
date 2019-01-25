<template>
  <div id="main__app">
    <v-app dark>
      <v-navigation-drawer fixed dark
                           v-model="drawer"
                           app>
        <v-list v-for="(route, index) in routes.filter(x=>x.nav === true)" :key="index" >
         
            <v-list-tile :to="route.path">
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

    export default {
     

      data () {
        return {
          routes,
          drawer: null
        }
      }
    }
</script>

<style>

</style>
