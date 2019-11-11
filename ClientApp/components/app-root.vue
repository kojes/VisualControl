<template>
  <div id="main__app">
    <v-app dark>

      <v-navigation-drawer v-if="auth" fixed
                           v-model="drawer"
                           app>
        <v-toolbar flat class="transparent">
          <v-list>
            <v-list-item avatar>
              <v-list-item-avatar>
                <avatar :username="username"
                        :size="40">
                </avatar>
              </v-list-item-avatar>

              <v-list-item-content>
                <v-list-item-title>{{username}}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>

          </v-list>

        </v-toolbar>
        <v-divider class="pt-0"></v-divider>
        <v-list v-for="(route, index) in routes.filter(x=>x.nav === true)" :key="index">

          <v-list-item ripple :to="route.path">
            <v-list-item-action>
              <v-icon>{{route.icon}}</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{route.display}}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

        </v-list>
      </v-navigation-drawer>
      <v-toolbar v-if="auth" color="indigo" fixed app>
        <v-toolbar-side-icon v-if="auth" @click.stop="drawer = !drawer"><v-icon>drag_indicator</v-icon></v-toolbar-side-icon>
        <v-toolbar-title>Система контроля за измерениями 2.0</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn to="/login" dark outline>Выйти из системы </v-btn>
      </v-toolbar>
      <v-content>

        <v-container fluid>

          <v-layout justify-start
                    align-center>
            <router-view>
            </router-view>
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

     computed:
     {
       auth()
       {
         if (this.$route.name === "login" || this.$route.name === "registration")
         {
           return false;
         }
         else
         {
           this.drawer = true;
           return true;
         }
       },

       username()
       {
         if (this.$store.state.authentication.user !== null)
         {
           return this.$store.state.authentication.user.firstName + " " + this.$store.state.authentication.user.surname;
         }
         else
         {
           return "";
         }
           
       }
       
     },

     methods:
     {
       
     },

     watch: {
       $route(to, from) {
          this.$store.dispatch('alert/clear');
       }
     },

      data () {
        return {
          routes,
          drawer: false,
          
        }
      }
    }
</script>

<style>

</style>
