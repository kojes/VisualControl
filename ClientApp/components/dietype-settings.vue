<template>
     <v-container>
      <v-layout>
        <v-flex lg10 lg-offset1>
            <v-tabs dark fixed-tabs @change="tabChange">
                <v-tab key="create">
                    Создание нового монитора
                </v-tab>
                <v-tab-item key="create">                   
                    <create></create>                    
                </v-tab-item>
                <v-tab key="update">
                    Редактирование монитора
                </v-tab>
                <v-tab-item key="update">                  
                    <update ref="updateTab"></update>                    
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
        },

        methods: {
            tabChange(tabIndex) {
                if(tabIndex === 0) {
                    this.$store.commit("elements/clearElements")
                }
                if(tabIndex === 1 && this.$refs.updateTab) {
                    this.$refs.updateTab.dieType = ""
                }
                
            }
        }
    }
</script>