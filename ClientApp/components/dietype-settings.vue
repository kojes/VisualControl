<template>
     <v-container>
      <v-layout>
        <v-flex lg10 lg-offset1>
            <v-tabs dark fixed-tabs>
                <v-tab key="create">
                    Создание нового монитора
                </v-tab>
                <v-tab-item key="create">
                    <v-card>
                        <create></create>
                    </v-card>
                </v-tab-item>
                <v-tab key="edit">
                    Редактирование монитора
                </v-tab>
                <v-tab-item key="edit">
                    <v-card>
                        <update></update>
                    </v-card>
                </v-tab-item>
            </v-tabs>
        </v-flex>
      </v-layout>
  </v-container>
</template>

<script>
    import DieTypeCreate from './dietype-create.vue';
    import DieTypeUpdate from './dietype-update.vue';
  
    export default {
        data() {
            return {

            }
        },

        components:
        {
            "create": DieTypeCreate,
            "update": DieTypeUpdate           
        }
    }
</script>