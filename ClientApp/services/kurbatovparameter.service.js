
import store from '../store/index'
import { newGuid } from '../services/guid.service.js'
export const kurbatovParameterService = {
    restoreKpListFromSmpVm, createKurbatovParameter
}

export const restoreKpListFromSmpVm = (guid, kpVmList) => kpVmList.forEach(kpVm => {createKurbatovParameter(guid, restoreKurbatovParameterFromViewModel(kpVm))})

export const createKurbatovParameter = (guid, kp = createInitialKurbatovParameter()) => store.dispatch("smpstorage/addToKpList",  {guid, kp})

const restoreKurbatovParameterFromViewModel = (kpVm) => {
    let kp = createInitialKurbatovParameter()
    kp.bounds = {lower: kpVm.kurbatovParameterBorders.lower, upper: kpVm.kurbatovParameterBorders.upper}
    kp.withBounds.value = Boolean(kpVm.kurbatovParameterBorders.id)
    kp.validationRules.parameterRq = true
    kp.standartParameter = {...kpVm.standartParameter}
    return kp
}

const createInitialKurbatovParameter = () => ({
    standartParameter: {parameterName: "", russianParameterName: "", parameterNameStat: "", specialRon: false, dividerNeed: false},
    bounds: {lower: "", upper: ""},
    withBounds: {value: false},
    validationRules: {boundsRq: true, lowerBoundIsNumeric: true, upperBoundIsNumeric: true, parameterRq: false, lowerBoundLowerThanUpperBound: true},
    key: newGuid()
})