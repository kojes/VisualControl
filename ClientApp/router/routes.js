import HomePage from 'components/home-page'
import SelectBasic from 'components/select-basic'
import DefectCard from 'components/defect-card'
import DefectSingle from 'components/defect-single'
import DefectVue from 'components/defect-mainvue'


export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Начальный экран', nav: true},
  { name: 'basic', path: '/select-basic', component: SelectBasic, display: 'Просмотр измерений', nav: true},
  { name: 'defectcard', path: '/defect-card', component: DefectCard, display: 'Defect card', nav: true },
  { path: '/defect/:defectid', component: DefectCard },
  { name: 'adddefect', path: '/defect-single', component: DefectSingle, display: 'Добавление дефекта', nav: true},
  { name: 'defects', path: '/defects', component: DefectVue, display: 'Просмотр дефектов', nav: true },
  { name: 'defectsbywafer', path: '/defects/:selectedWafer', display: 'Просмотр дефектов', component: DefectVue, props: true, children: [{ path: 'diecode/:selectedDies', component: DefectVue }] }
];
