<template>
   <v-container>
        <v-layout row>
            <v-flex lg3 offset-lg1>
                <v-text-field v-model="operation.number" :error-messages="$v.$dirty && !$v.operation.number.required ? [operation.errorMessage] : []" @change="validateElement()" label="Номер операции:"  outline>
                </v-text-field>
             
            </v-flex>
            <v-flex lg3 offset-lg1>
                <v-text-field v-model="element.name" :error-messages="$v.$dirty && !$v.element.name.required ? [element.errorMessage] : []" @change="validateElement()" label="Название элемента:" outline>
                </v-text-field>
          
            </v-flex>           
         </v-layout>
         <v-layout row>
          <v-flex lg7 offset-lg1>
            <v-stepper v-model="e1">
                <v-stepper-header>
                    <template v-for="(parameter, index) in parameters">
                    <v-stepper-step 
                        :key="`${parameter}-step`"
                        :complete="parameter.done"
                        :step="index + 1"
                        color="indigo"
                        editable>
                        {{parameter.parameterName.value}}
                    </v-stepper-step>
                    <v-divider
                        :key="index + 1"
                    ></v-divider>
                    </template>
                </v-stepper-header>

                <v-stepper-items>
                    <v-stepper-content
                        v-for="(parameter, index) in parameters"
                        :key="`${parameter}-content`"
                        :step="index + 1">
                        <v-card>
                            <v-card-text>
                                <v-layout row>
                                    <v-flex lg4>
                                        <v-text-field v-model="parameter.parameterName.value" :error-messages="parameter.parameterName.isValidDirty && !parameter.parameterName.isValid ? defaultRequiredMessage: []" @change="validateParameter(parameter)" outline label="Буквенное обозначение:">
                                        </v-text-field>
                                    </v-flex>
                                    <v-flex lg7 offset-lg1>
                                        <v-text-field v-model="parameter.russianParameterName.value" :error-messages="parameter.russianParameterName.isValidDirty &&!parameter.russianParameterName.isValid ? defaultRequiredMessage: []" @change="validateParameter(parameter)" outline label="Наименование:">
                                        </v-text-field>
                                    </v-flex>           
                                </v-layout>

                                <v-layout row v-if="!parameter.shortLink.success">
                                    <v-flex lg4 >
                                        <v-text-field v-model="parameter.shortLink.value" :error-messages="$v.$dirty && !parameter.shortLink.success ? [parameter.shortLink.errorMessage] : []" outline label="Короткая ссылка:" @change="shortLinkHandler($event, index)">
                                        </v-text-field>
                                    </v-flex>                                               
                                 </v-layout>

                                <v-layout row v-else>
                                    <v-flex lg4>
                                        <p>Номер пластины: {{parameter.waferId}}</p>
                                        <p>ID: {{parameter. measurementRecordingId}}</p>
                                    </v-flex>
                                    <v-flex lg7 offset-lg1>
                                        <v-select v-model="parameter.selectedStatParameter"
                                                    :items="parameter.statParameterArray"
                                                    no-data-text="Нет данных"
                                                    outline
                                                    label="Выберите параметр:">
                                        </v-select>
                                    </v-flex>
                                </v-layout>

                                <v-layout row v-if="parameter.shortLink.success">
                                    <v-flex lg4>
                                    </v-flex>
                                    <v-flex lg3 offset-lg1>
                                        <v-text-field v-model="parameter.bounds.lower.value" :error-messages="parameter.bounds.lower.isValidDirty && !parameter.bounds.lower.isValid ? parameter.bounds.lower.errorMessages[0]: []" @change="validateParameter(parameter)" outline label="Нижняя граница:">
                                        </v-text-field>
                                    </v-flex>
                                    <v-flex lg3 offset-lg1>
                                        <v-text-field v-model="parameter.bounds.upper.value" :error-messages="parameter.bounds.upper.isValidDirty && !parameter.bounds.upper.isValid ? parameter.bounds.upper.errorMessages[0]: []" @change="validateParameter(parameter)" outline label="Верхняя граница:">
                                        </v-text-field>
                                    </v-flex>
                                </v-layout>

                            </v-card-text>
                        </v-card>
                        <v-btn
                            color="indigo"
                            @click="nextStep(index + 1)">
                            Следующий параметр
                        </v-btn>
                       
                    </v-stepper-content>
                    <v-btn
                        color="indigo"
                        @click="addParameter()">
                        Добавить параметр
                    </v-btn>
                    <v-chip v-if="!isElementCorrect" color="red" text-color="white">Элемент заполнен некорректно</v-chip>
                    <v-chip v-else color="green" text-color="white">Элемент заполнен корректно</v-chip>
                </v-stepper-items>
            </v-stepper>
          </v-flex>
         </v-layout>
    </v-container>
</template>

<script>
import { required } from 'vuelidate/lib/validators'
import { create } from '@amcharts/amcharts4/core';
export default {

    data() {
        return {
          operation: {number: "180", errorMessage: "Введите номер операции"},
          element: {name: "TC1", errorMessage: "Введите название элемента"},
          parameters: [],
          defaultRequiredMessage : "Введите значение",
          e1: 0
      }
    
    },

    computed: {
        isElementCorrect: function() { return (this.parameters.length > 0 && this.validateElement()) }        
    },

    methods: {
     
      addParameter()
      {      
          if(this.validateElement()) {
              this.createParameter()
          }
      },

    
      validateElement()
      {     
            this.$v.$touch();     
            if(!this.$v.operation.number.required|| !this.$v.element.name.required)
                return false;
            if(this.parameters.length === 0) {
                return true
            }           
            for (let i = 0; i < this.parameters.length; i++) {
                const currentParameter = this.parameters[i];
                if(!this.validateParameter(currentParameter)) {
                    return false
                }
                                
            }      
        
        return true          
      },


      validateParameter(currentParameter)
      {
          
        currentParameter.parameterName.isValidDirty = true
        currentParameter.russianParameterName.isValidDirty = true
        currentParameter.parameterName.isValid = true
        currentParameter.russianParameterName.isValid = true

        if(!currentParameter.parameterName.value) {
            currentParameter.parameterName.isValid = false;
        }
        if(!currentParameter.russianParameterName.value) {
            currentParameter.russianParameterName.isValid = false;
        }

        if(currentParameter.shortLink.success) {     
            const isNumber = (n) => !isNaN(parseFloat(n)) && !isNaN(n - 0)               
            let lowerBound = currentParameter.bounds.lower
            let upperBound = currentParameter.bounds.upper
            lowerBound.errorMessages = []
            upperBound.errorMessages = []
            lowerBound.isValidDirty = true
            upperBound.isValidDirty = true
            lowerBound.isValid = true
            upperBound.isValid = true
            if(lowerBound.value && !isNumber(lowerBound.value)) {
                lowerBound.isValid = false
                lowerBound.errorMessages.push("Введите значение в правильном формате")
            }                   
            if(upperBound.value && !isNumber(upperBound.value)) {
                upperBound.isValid = false
                upperBound.errorMessages.push("Введите значение в правильном формате")
            }
            if(!lowerBound.value && !upperBound.value) {
                lowerBound.isValid = false
                upperBound.isValid = false
                lowerBound.errorMessages.push("Должна быть установлена хотя бы одна граница")
                upperBound.errorMessages.push("Должна быть установлена хотя бы одна граница")
            } 
            if(lowerBound.value && upperBound.value && lowerBound.isValid && upperBound.isValid && lowerBound.value > upperBound.value) {
                upperBound.isValid = false;
                upperBound.errorMessages.push("Верхняя граница должна быть больше нижней")
            }
        }                                          

        if(currentParameter.russianParameterName.isValid && currentParameter.parameterName.isValid 
                && currentParameter.shortLink.success && currentParameter.bounds.lower.isValid && currentParameter.bounds.upper.isValid) {
            return true
        }  
        return false
      },

      createParameter()
      {
           let parameter = {
                    parameterName: {value: "", isValidDirty: false, isValid: true},
                    russianParameterName: {value: "", isValidDirty: false, isValid: true},
                    waferId: "",
                    measurementRecordingId: "",
                    selectedStatParameter: "",
                    bounds: {lower: {value: "", isValidDirty: false, isValid: true, errorMessages: []}, upper: {value: "", isValidDirty: false, isValid: true, errorMessages: []}},     
                    divider: "",         
                    statParameterArray: [],
                    shortLink: {value: "", success: "", errorMessage: ""},
                    done: false

                }
                this.parameters.push(parameter)
                this.e1 = this.e1 + 1
      },
      async shortLinkHandler(shortLink, index)
      {
        let generatedId = shortLink.split('=')[1];
        let parameter = this.parameters[index];       
        await this.$http.get(`api/shortlink/${generatedId}/element-export`)
            .then((response) => {
                let data = response.data;              
                if(response.status == 200) {                               
                    parameter.statParameterArray = data.statisticNameList
                    parameter.selectedStatParameter = parameter.statParameterArray[0]
                    parameter.waferId = data.waferId
                    parameter.measurementRecordingId = data.measurementRecordingId
                    parameter.shortLink.success = true
                    parameter.shortLink.errorMessage = ""
                }
                else {
                     parameter.shortLink.success = false;
                     parameter.shortLink.errorMessage = data.reduce((r,c) => r + "/n" + c.message);
                }
            })
            .catch((error) => {
                parameter.shortLink.success = false;
                parameter.shortLink.errorMessage = "Не удалось обработать ссылку";
            });
      
        
      },
      nextStep (n) {
        if(this.validateParameter(this.parameters[this.e1 - 1])) {        
            if (n === this.parameters.length) {
                this.e1 = 1
            } 
            else {
                this.e1 = n + 1
            }
        }
      }
    },
    validations: {
        
        operation: {
            number: {
                required
            }
            
        },
        element: {
            name:{
                required
            }         
        }
      

    }

  
}

</script>

<style>

</style>