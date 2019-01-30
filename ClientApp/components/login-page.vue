<template>
  <v-container>


    <form @submit.prevent="handleSubmit">

      <v-layout>

        <v-flex lg6>
          <v-text-field v-model="username" label="Логин:">
          </v-text-field>
        </v-flex>

      </v-layout>
      <v-layout>
        <v-flex lg6>
          <v-text-field v-model="password"
                        label="Пароль:"
                        readonly>
          </v-text-field>
        </v-flex>


      </v-layout>
      <v-layout>
        <v-flex lg6>
          <v-btn block :disabled="loggingIn">Войти</v-btn>


        </v-flex>
      </v-layout>
    </form>

  </v-container>
</template>

<script>
  
export default {
    data() {
      return {
        username: '',
        password: '',
        submitted: false
      }
    },
    computed: {
      loggingIn() {
        return this.$store.state.authentication.status.loggingIn;
      }
    },
    created() {
      
      this.$store.dispatch('authentication/logout');
    },
    methods: {
      handleSubmit(e) {
        this.submitted = true;
        const { username, password } = this;
        const { dispatch } = this.$store;
        if (username && password) {
          dispatch('authentication/login', { username, password });
        }
      }
    }
  };

</script>



<style>

</style>
