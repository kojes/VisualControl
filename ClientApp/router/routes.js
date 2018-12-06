import HomePage from 'components/home-page'
import SelectBasic from 'components/select-basic'
import DefectCard from 'components/defect-card'
import DefectSingle from 'components/defect-single'
import DefectVue from 'components/defect-mainvue'


export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Начальный экран'},
  { name: 'basic', path: '/select-basic', component: SelectBasic, display: 'Просмотр измерений'},
  { name: 'defectcard', path: '/defect-card', component: DefectCard, display: 'Defect card' },
  { name: 'adddefect', path: '/defect-single', component: DefectSingle, display: 'Добавление дефекта', icon: '' },
  { name: 'defect', path: '/defect-mainvue', component: DefectVue, display: 'Просмотр дефектов', icon: '' }
];
