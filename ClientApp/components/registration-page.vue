<template>
  <v-container grid-list-lg>
   
     
        <form>

          <v-layout align-start wrap>
            <v-flex lg4 offset-lg4>

              <v-text-field v-model="login"
                            label="Логин"
                            outline
                            readonly>

              </v-text-field>
             
            </v-flex>
            <v-flex>
              <v-btn outline to="\login" color="teal">Я уже зарегистрирован в системе</v-btn>
            </v-flex>
            <v-flex lg4 offset-lg4>
              <v-text-field v-model="password"
                            type="password"
                            outline
                            label="Пароль"
                            :error-messages="passwordErrors"
                            required
                            @input="$v.password.$touch()"
                            @blur="$v.password.$touch()">
              </v-text-field>
            
            </v-flex>
            <v-flex lg4 offset-lg4>
              <v-text-field v-model="firstname"
                            :error-messages="firstnameErrors"
                            label="Имя"
                            outline
                            required
                            @input="$v.firstname.$touch()"
                            @blur="$v.firstname.$touch()">
              </v-text-field>
            </v-flex>
            <v-flex lg4 offset-lg4>
              <v-text-field v-model="surname"
                            :error-messages="surnameErrors"
                            label="Фамилия"
                            outline
                            required
                            @input="$v.surname.$touch()"
                            @blur="$v.surname.$touch()">
              </v-text-field>
            </v-flex>
            <v-flex lg4 offset-lg4>
              <v-text-field v-model="email"
                            :error-messages="emailErrors"
                            outline
                            label="Адрес электронной почты"
                            required
                            @input="$v.email.$touch()"
                            @blur="$v.email.$touch()">
              </v-text-field>
            </v-flex>
            <v-flex lg4 offset-lg5>
               <v-btn outline @click.native="registerAttempt">Зарегистрироваться в системе</v-btn>
            </v-flex>
            
          </v-layout>
        </form>
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
  import { transliterate as tr } from 'transliteration';
  import { required, minLength, email } from 'vuelidate/lib/validators'
  export default {
    data() {
      return {

        errorSnackbar: false,
        password: '1234556',
        firstname: 'Денис',
        surname: 'Куликов',
        email: 'kulikov@svrost.ru'
       
      }
    },

    methods:
    {
      registerAttempt()
      {
        this.$v.$touch();
        if (!this.$v.$invalid) {
          let user = { login: this.login, password: this.password, firstname: this.firstname, surname: this.surname, email: this.email };
          const { dispatch } = this.$store;
          dispatch('authentication/registry', { user });
          
        }
      }
    },

    computed:
    {
      login()
      {
          return tr(this.surname);
      },

      snackbarText()
      {
        let request = this.$store.state.alert.message;
        if (request) {
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
        !this.$v.password.minLength && errors.push('Пароль должен быть более 6 символов')
        return errors
      },

      firstnameErrors() {
        const errors = []
        if (!this.$v.firstname.$error) return errors
        !this.$v.firstname.required && errors.push('Введите имя')
        return errors
      },

      surnameErrors() {
        const errors = []
        if (!this.$v.surname.$error) return errors
        !this.$v.surname.required && errors.push('Введите фамилию')
        return errors
      },

      emailErrors() {
        const errors = []
        if (!this.$v.email.$error) return errors
        !this.$v.email.required && errors.push('Введите адрес электронной почты')
        !this.$v.email.email && errors.push('Введите существующий адрес электронной почты')
        return errors
      }


    },

    validations: {
      password: {
        required,
        minLength: minLength(6)
      },
      firstname: {
        required
      },
      surname: {
        required
      },
      email:
      {
        required, email
      }

    }
  };
</script>

<style>

</style> 
