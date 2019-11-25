import HomePage from 'components/home-page'
import DefectCard from 'components/defect-card'
import DefectSingle from 'components/defect-single'
import DefectVue from 'components/defect-mainvue'
import WaferMeas from 'components/wafermap-meas'
import DangerLevel from 'components/dangerlevel-crud'
import DefectType from 'components/defecttype-crud'
import Device from 'components/device-crud'
import WaferMap from 'components/wafermap-full'
import SelectBasic from 'components/select-basic'
import Kurbatov from 'components/export-kurb'
import VerificationSettings from 'components/verification-settings'
import IdmrVoc from 'components/idmr-voc'
import DefectMassiveUploader from 'components/massive-uploader'
import DieTypeSettings from 'components/dietype-settings'
import Uploader from 'components/uploader-ng'
import UploaderFg from 'components/uploader-filegraphic'
// Service components
import LoginPage from 'components/login-page'
import RegistrationPage from 'components/registration-page'
import NotFound from 'components/error-404'

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Начальный экран', nav: true, icon: 'home' },
  {
    name: 'defectuploader', path: '/defectuploader', component: DefectMassiveUploader, display: 'MassiveUploader', nav: true, icon: 'cloud_upload' },
  { name: 'registration', path: '/registration', component: RegistrationPage },
  { name: 'login', path: '/login', component: LoginPage },
  { name: 'wafermap', path: '/wafermap', component: WaferMap, display: 'WaferMap', nav: true, icon: 'blur_circular' },
  { name: 'defecttypeCRUD', path: '/defecttype', component: DefectType, display: 'DefectType', nav: true, icon: 'category' },
  { name: 'device', path: '/devices', component: Device, display: 'Device', nav: true, icon: 'category' },
  { name: 'dangerlevelCRUD', path: '/dangerlevel', component: DangerLevel, display: 'DangerLevel', nav: true, icon: 'report_problem' },
  { name: 'testing', path: '/testing', component: SelectBasic, display: 'Просмотр иcпытаний', nav: true },
  { path: '/defect/:defectid', component: DefectCard },
  { name: 'adddefect', path: '/adddefect', component: DefectSingle, display: 'Добавление дефекта', nav: true },
  { name: 'defects', path: '/defects', component: DefectVue, display: 'Просмотр дефектов', nav: true },
  { name: 'wafermeasurement', path: '/wafermeas', component: WaferMeas, display: 'Измерения', nav: true },
  { name: 'kurbatov', path: '/export-kurb', component: Kurbatov, display: 'Экспорт', nav: true }, 
  { name: 'idmrvoc', path: '/idmr-voc', component: IdmrVoc, display: 'voc', nav: true }, 
  { name: 'dietypesettings', path: '/dts', component: DieTypeSettings, display: 'dts', nav: true }, 
  { name: 'uploader', path: '/uu', component: Uploader, display: 'uu', nav: true },
  { name: 'uploader-fg', path: '/ufg', component: UploaderFg, display: 'ufg', nav: true },
  { name: 'uploader-cp', path: '/uu/:selectedCodeProductFolder', component: Uploader, display: 'uu', props: true }, 
  { name: 'uploader-cpw', path: '/uu/:selectedCodeProductFolder/:selectedWaferFolder', component: Uploader, display: 'uu', props: true }, 
  { name: 'uploader-cpwi', path: '/uu/:selectedCodeProductFolder/:selectedWaferFolder/:mrArray', component: Uploader, display: 'uu', props: true }, 

  //{ name: 'verificationsettings', path: '/vsettings', component: VerificationSettings, display: 'Редактирование параметров испытаний', nav: true }, 

  {
    name: 'defectsbywafer',
    path: '/defects/:selectedWafer',
    display: 'Просмотр дефектов',
    component: DefectVue,
    props: true

  },

  {
    name: 'singlediedefects',
    path: '/defects/:selectedWafer/singledie/:selectedsingledieId/:dangerlevelspec',
    display: 'Просмотр дефектов',
    component: DefectVue,
    props: true

  },

  {
    path: '/404',
    name: '404',
    component: NotFound
  }, {
    path: '*',
    redirect: '/404'
  }
]

