<template>
    <v-container> 
      <div class="d-flex flex-row justify-center align-center ma-4">
        <div>
          <v-card
            min-height="250"
            min-width="500"
            color="indigo"
            shaped
            @click="$router.push({ name: 'pwafer'})"
            class="d-flex left-card ma-2">
             <v-card-text class="headline font-weight-bold">
                    Просмотр измерений
                </v-card-text>
            <v-card-actions>
            </v-card-actions>
          </v-card>
        </div>
        <div>
          <div>
            <div>
              <v-card
                min-height="150"
                shaped
                color="indigo"
                @click="$router.push({ name: 'testing'})"
                class="d-flex ma-1">
                  <v-card-text class="headline font-weight-bold">
                    Результаты испытаний
                  </v-card-text>
                <v-card-actions>
                </v-card-actions>
              </v-card>
              <v-card
                min-height="150"
                shaped
                color="indigo"
                @click="$router.push({ name: 'control-charts'})"
                class="d-flex ma-1">
                  <v-card-text class="headline font-weight-bold">
                    Контрольные карты
                  </v-card-text>
                <v-card-actions>
                </v-card-actions>
              </v-card>
            </div>
            <div class="d-flex flex-row justify-space-around">
              <v-card
                min-height="100"
                shaped
                color="indigo"
                 @click="$router.push({ name: 'device'})"
                class="d-flex ma-1">
                <v-card-text class="headline font-weight-bold">
                    Приборы
                </v-card-text>
                <v-card-actions>
                </v-card-actions>
              </v-card>
              <v-card
                min-height="100"
                @click=""
                color="indigo"
                class="d-flex ma-1">
                <v-card-text class="headline font-weight-bold">
                    Измеряемые устройства
                </v-card-text>
                <v-card-actions>
                </v-card-actions>
              </v-card>
              <v-card
                min-height="100"
                shaped
                color="indigo"
                @click=""
                class="d-flex ma-1">
                <v-card-text class="headline font-weight-bold">
                  Измерения
                </v-card-text>
                <v-card-actions>
                </v-card-actions>
              </v-card>
            </div>
          </div>
        </div>
      </div>
      <div class="d-flex flex-row justify-center align-center ma-4">
        <div>
          <v-card
            min-height="300"
            min-width="500"
            shaped
            color="indigo"
            @click="$router.push({ name: 'uploader'})"
            class="d-flex ma-2 left-card">
              <v-card-text class="headline font-weight-bold">
                Загрузка измерений
              </v-card-text>
              <v-card-actions>
              </v-card-actions>
          </v-card>
        </div>
        <div class="d-flex flex-column">
          <v-card
            min-height="100"
            shaped
            color="indigo"
            @click="$router.push({ name: 'idmrvocstart'})"
            class="d-flex ma-1">
             <v-card-text class="headline font-weight-bold">
                Редактирование измерений
              </v-card-text>
              <v-card-actions>
              </v-card-actions>
          </v-card>
          <v-card
            min-height="100"
            @click="$router.push({ name: 'dietypesettings'})"
            color="indigo"
            class="d-flex ma-1">
              <v-card-text class="headline font-weight-bold">
                Настройка элементов
              </v-card-text>
              <v-card-actions>
              </v-card-actions>
          </v-card>
          <v-card
            min-height="100"
            shaped
            @click="$router.push({ name: 'uploader-fg'})"
            color="indigo"
            class="d-flex ma-1">
             <v-card-text class="headline font-weight-bold">
                Типы измерений
              </v-card-text>
              <v-card-actions>
              </v-card-actions>
          </v-card>
        </div>
      </div>
      <div class="d-flex flex-row justify-center align-center">
        <div>
           <v-card
            min-height="200"
            min-width="500"
            shaped
            color="indigo"
            @click="$router.push({ name: 'export'})"
            class="d-flex ma-2 left-card">
              <v-card-text class="headline font-weight-bold">
               Экспорт
              </v-card-text>
              <v-card-actions>
              </v-card-actions>
          </v-card>
        </div>
        <div class="d-flex flex-column">
           <v-card
            min-height="100"
            shaped
            color="indigo"
            @click=""
            class="d-flex ma-1"
            >
             <v-card-text class="headline font-weight-bold">
               Создание шаблонов
              </v-card-text>
              <v-card-actions>
              </v-card-actions>
          </v-card>
           <v-card
            min-height="100"
            shaped
            color="indigo"
            @click=""
            class="d-flex ma-1"
            >
             <v-card-text class="headline font-weight-bold">
               Типы элементов
              </v-card-text>
              <v-card-actions>
              </v-card-actions>
          </v-card>
        </div>
      </div>
    </v-container>
</template>

<script>
export default {
  data () {
    return {}
  }
}
</script>

<style scoped>
  .left-card {
    border-color: blueviolet;
    border-width: 5px;
  }
</style>
