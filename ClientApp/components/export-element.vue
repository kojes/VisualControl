<template>
   <v-container>
        <v-layout row>
            <v-flex lg3>
                <v-text-field   v-model="operation.number" 
                                :error-messages="$v.$dirty && !$v.operation.number.required ? [operation.errorMessage] : []" 
                                @change="validateElement()" label="Номер операции:"  outlined>
                </v-text-field>             
            </v-flex>
            <v-flex lg3>
                <v-text-field   v-model="element.name" 
                                :error-messages="$v.$dirty && !$v.element.name.required ? [element.errorMessage] : []" 
                                @change="validateElement()" label="Название элемента:" outlined>
                </v-text-field>          
            </v-flex>
             <v-flex lg3>
                <v-btn v-if="!isElementReady" large block outlined color="pink">Элемент заполнен некорректно</v-btn>
                <v-btn v-else large block outlined color="green" >Элемент заполнен корректно</v-btn>       
            </v-flex>
             <v-flex lg2 offset-lg1>
                <v-switch
                    v-model="element.isAddedToCommonWorksheet"
                    color='primary'
                    :label="element.isAddedToCommonWorksheet ? `Элемент будет добавлен в сводную таблицу` : `Элемент не будет добавлен в сводную таблицу`"
                ></v-switch>  
            </v-flex>   
               
         </v-layout>
         <v-layout row>
            <v-flex lg12>
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
                            <v-divider :key="index + 1"></v-divider>
                        </template>
                    </v-stepper-header>
                    <v-stepper-items>
                        <v-stepper-content
                            v-for="(parameter, index) in parameters"
                            :key="`${parameter}-content`"
                            :step="index + 1">
                            <div>
                                
                                    <v-layout class="pa-4" row>
                                        <v-flex lg3>
                                            <v-text-field class="pt-4" v-model="parameter.parameterName.value" 
                                                            :error-messages="parameter.parameterName.isValidDirty 
                                                                             &&!parameter.parameterName.isValid 
                                                                             ? defaultRequiredMessage 
                                                                             : []" 
                                                            @change="validateParameter(parameter)" outlined label="Буквенное обозначение:">
                                            </v-text-field>
                                        </v-flex>
                                        <v-flex lg8 offset-lg1>
                                            <v-text-field  class="pt-4"  v-model="parameter.russianParameterName.value" 
                                                            :error-messages="parameter.russianParameterName.isValidDirty 
                                                                             &&!parameter.russianParameterName.isValid 
                                                                             ? defaultRequiredMessage 
                                                                             : []" 
                                                            @change="validateParameter(parameter)" outlined label="Наименование:">
                                            </v-text-field>
                                        </v-flex>           
                                    </v-layout>
                                    <v-layout class="pa-4" row v-if="!parameter.shortLink.success">
                                        <v-flex lg3>
                                            <v-text-field   v-model="parameter.shortLink.value" 
                                                            :error-messages="$v.$dirty && !parameter.shortLink.success ? [parameter.shortLink.errorMessage] : []" 
                                                            outlined label="Короткая ссылка:" @change="shortLinkHandler($event, index)">
                                            </v-text-field>
                                        </v-flex>                                               
                                    </v-layout>
                                    <v-layout class="pa-4" row v-else>
                                        <v-flex lg3>
                                            <p>Номер пластины: {{parameter.waferId}}</p>
                                            <p>Название операции: {{parameter.measurementRecording.name}}</p>
                                        </v-flex>
                                        <v-flex lg8 offset-lg1>
                                            <v-select   v-model="parameter.selectedStatParameter"
                                                        :items="parameter.statParameterArray"
                                                        no-data-text="Нет данных"
                                                        outlined
                                                        v-on:change="changeDividerId(parameter)"
                                                        label="Выберите параметр:">
                                            </v-select>
                                        </v-flex>
                                    </v-layout>
                                    <v-layout class="pa-4" row v-if="parameter.shortLink.success">
                                        <v-flex lg3>
                                           
                                        </v-flex>
                                         <v-flex lg3 offset-lg1>
                                            <v-select   v-model="parameter.dividerId"
                                                        :items="dividers"
                                                        no-data-text="Нет данных"
                                                        item-value="id"
                                                        item-text="name"
                                                        outlined
                                                        v-on:change="changeDividerId(parameter)"
                                                        label="Выберите приведение:">
                                            </v-select>
                                            <p v-if="parameter.dividerId">Коэффициент приведения: {{parameter.divider}}</p>
                                        </v-flex>
                                        <v-flex lg2 offset-lg1>
                                            <v-text-field   v-model="parameter.bounds.lower.value" 
                                                            :error-messages="parameter.bounds.lower.isValidDirty 
                                                                             &&!parameter.bounds.lower.isValid 
                                                                             ? parameter.bounds.lower.errorMessages[0] 
                                                                             : []" 
                                                            @change="validateParameter(parameter)" outlined label="Нижняя граница:">
                                            </v-text-field>
                                        </v-flex>
                                        <v-flex lg2>
                                            <v-text-field   v-model="parameter.bounds.upper.value" 
                                                            :error-messages="parameter.bounds.upper.isValidDirty 
                                                                             &&!parameter.bounds.upper.isValid 
                                                                             ? parameter.bounds.upper.errorMessages[0] 
                                                                             : []" 
                                                            @change="validateParameter(parameter)" outlined label="Верхняя граница:">
                                            </v-text-field>
                                        </v-flex>
                                          
                                    </v-layout>
                                
                            
                            <v-tooltip v-if="index > 0" bottom>
                                <template v-slot:activator="{ on }">
                                    <v-btn fab dark small color="indigo" v-on="on" @click="prevStep(index + 1)">
                                        <v-icon dark color="primary">skip_previous</v-icon>
                                    </v-btn>
                                </template>
                                <span>Предыдущий параметр</span>
                            </v-tooltip>                        
                            <v-tooltip v-if="index < parameters.length - 1" bottom>
                                <template v-slot:activator="{ on }">
                                    <v-btn fab dark small color="indigo" v-on="on" @click="nextStep(index + 1)">
                                        <v-icon dark color="primary">skip_next</v-icon>
                                    </v-btn>
                                </template>
                                <span>Следующий параметр</span>
                            </v-tooltip>    
                          </div>                         
                        </v-stepper-content>
                    </v-stepper-items> 
                <div class="parameter-actions"> 
                    <v-tooltip bottom>
                        <template v-slot:activator="{ on }">
                            <v-btn fab dark small color="indigo" v-on="on" @click="createParameter()">
                                <v-icon dark color="primary">add</v-icon>
                            </v-btn>
                        </template>
                        <span>Добавить параметр</span>
                    </v-tooltip>
                    <v-tooltip bottom>
                        <template v-slot:activator="{ on }">
                            <v-btn v-if="parameters.length > 0" fab dark small color="indigo" v-on="on" @click="deleteParameterDialog = true">
                                <v-icon dark color="primary">delete</v-icon>
                            </v-btn>
                        </template>
                        <span>Удалить параметр</span>
                    </v-tooltip>               
               </div>
            </v-stepper>
          </v-flex>
         </v-layout>        
        <v-layout row>
            <v-dialog v-model="deleteParameterDialog" persistent max-width="400">
               <v-card>
                    <v-card-title class="headline">Удаление</v-card-title>
                     <v-card-text>Вы действительно хотите удалить текущий параметр?</v-card-text>
                    <v-spacer></v-spacer>
                    <v-card-actions>
                        <v-btn color="pink" text @click="deleteParameter()">Удалить</v-btn>
                        <v-btn color="indigo" text @click="deleteParameterDialog = false">Отмена</v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
        </v-layout>
    </v-container>
</template>

<script>
import { required } from 'vuelidate/lib/validators'
export default {

    data() {
        return {      
          freakDividerParameters: ["r<sub>DS(on)</sub> (сопротивление открытого канала при Uси = 0.02В)", "R<sub>ds(on)</sub> (сопротивление открытого канала)"],
          defaultRequiredMessage : "Введите значение",
          deleteParameterDialog: false,
          e1: 0
      }    
    },

    props: ['waferId', 'key', 'parameters', 'dividers', 'operation', 'element'],  

    computed: {
        isElementReady: function() { return (this.parameters.length > 0 && this.validateElement()) }        
    },

    methods: {        
        createParameter() {
            const parameter = {
                parameterName: {value: "", isValidDirty: false, isValid: true},
                russianParameterName: {value: "", isValidDirty: false, isValid: true},
                waferId: "",
                measurementRecording: {id: "0", name: "Неизвестно"},
                selectedStatParameter: "",
                bounds: {lower: {value: "", isValidDirty: false, isValid: true, errorMessages: []}, 
                        upper: {value: "", isValidDirty: false, isValid: true, errorMessages: []}},     
                dividerId: 1,
                divider: "1.0",         
                statParameterArray: [],
                shortLink: {value: "", success: "", errorMessage: ""},
                done: false
            }
            this.parameters.push(parameter)
            this.e1++
        },
        deleteParameter() {
            this.parameters.splice(this.e1 - 1, 1)
            this.deleteParameterDialog = false
            this.e1--            
        },        
        validateElement() {     
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
        validateParameter(currentParameter) {          
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
                } ``
                if(lowerBound.value && upperBound.value && lowerBound.isValid && upperBound.isValid && parseFloat(lowerBound.value) > parseFloat(upperBound.value)) {
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
        async shortLinkHandler(shortLink, index) {
            const generatedId = shortLink.split('=')[1]
            let parameter = this.parameters[index]       
            await this.$http.get(`api/shortlink/${generatedId}/element-export`)
                .then((response) => {
                    let data = response.data;              
                    if(response.status === 200) {                               
                        parameter.statParameterArray = data.statisticNameList
                        if(!parameter.selectedStatParameter)
                            parameter.selectedStatParameter = parameter.statParameterArray[0]
                        parameter.waferId = data.waferId
                        parameter.measurementRecording.id = data.measurementRecordingId
                        parameter.shortLink.success = true
                        parameter.shortLink.errorMessage = ""
                        this.$http
                            .get(`api/measurementrecording/${parameter.measurementRecording.id}`)
                            .then(response => {parameter.measurementRecording.name = response.data.name.split('.')[1]})
                            .catch(error => {});
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
        changeDividerId(parameter) {
            parameter.divider = this.freakDividerParameters.includes(parameter.selectedStatParameter) 
                                ? (1/(this.dividers.find(_ => _.id == parameter.dividerId).dividerK)).toFixed(3) 
                                : (+this.dividers.find(_ => _.id == parameter.dividerId).dividerK).toFixed(3)
        },
        nextStep (n) {       
            if (n === this.parameters.length) {
                this.e1 = 1
            } 
            else {
                this.e1 = n + 1
            }        
        },
        prevStep (n) {              
            if (n === 1) {
                this.e1 = this.parameters.length
            }    
            else {
                this.e1 = n - 1
            }            
        }
    },    
    watch: {
        isElementReady: {
           handler: function(newValue) {  
                this.$store.commit("exportkurb/updateElementsReady", {key: this.key, ready: newValue});
            },
            immediate: true          
        }
    },    
    validations: {        
        operation: {
            number: {
                required
            }            
        },
        element: {
            name: {
                required
            }         
        }   
    }  
}
</script>

<style>
    .parameter-actions {
        margin-top: 10px;
        border-width: 0.5px;
        border-style: outset;
        border-color: #ffcc00
    } 
</style>