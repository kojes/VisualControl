<template>

  <v-container grid-list-lg>
    
    <v-layout align-start wrap>

      <v-flex lg4 offset-lg4>
        <v-text-field v-model="username" outline label="Логин:" :error-messages="usernameErrors" @input="$v.username.$touch()" @blur="$v.username.$touch()">
        </v-text-field>
      </v-flex>

      <v-flex lg4 offset-lg4>
        <v-text-field v-model="password" outline label="Пароль:" type="password" :error-messages="passwordErrors" @input="$v.password.$touch()" @blur="$v.password.$touch()">
        </v-text-field>
      </v-flex>
      
      <v-flex lg4 offset-lg4>
        <v-btn block outline :disabled="loggingIn" @click="handleSubmit">Войти</v-btn>        
      </v-flex>

      <v-flex lg4 offset-lg4>
        <v-btn block @click="goToRegistry">Регистрация</v-btn>
      </v-flex>

    </v-layout>


    <v-snackbar v-model="errorSnackbar"                           
                top>
      {{ snackbarText }}
      <v-btn color="pink"
             flat
             @click="errorSnackbar = false">
        Закрыть
      </v-btn>
    </v-snackbar>



  </v-container>

</template>

<script>
import { required } from 'vuelidate/lib/validators'
export default {
    data() {
      return {
        username: "Molchanov",
        password: "123456",
        errorSnackbar: false,
        submitted: false
      }
    },
    computed: {
      loggingIn() {
        return this.$store.state.authentication.status.loggingIn;
      },

      snackbarText()
      {
         let request = this.$store.state.alert.message;
         if (request)
         {
           this.errorSnackbar = true;
           return request.response.data.message;
         }
         this.errorSnackbar = false;
         return ""; 
      },

      passwordErrors() {
        const errors = []
        if (!this.$v.password.$error) return errors
        !this.$v.password.required && errors.push('Введите пароль')
       
        return errors
      },

      usernameErrors() {
        const errors = []
        if (!this.$v.username.$error) return errors
        !this.$v.username.required && errors.push('Введите логин')

        return errors
      },


    },
    created() {
      
      this.$store.dispatch('authentication/logout');
    },
    methods: {
      handleSubmit() {
        this.$v.$touch();
        if (!this.$v.$invalid) {
          this.submitted = true;
          const { username, password } = this;
          const { dispatch } = this.$store;
          if (username && password) {
            dispatch('authentication/login', { username, password });
            
          }
        }
      },

      goToRegistry()
      {
        this.$router.push({ name: 'registration' });
      }
    },
    validations: {
      password: {
        required,
      },
      username: {
        required
      }

    }
  };

</script>



<style>

</style>
