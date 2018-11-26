import HomePage from 'components/home-page'
import SelectBasic from 'components/select-basic'
import PhotoUploader from 'components/photo-uploader'
import DefectSimple from 'components/defect-single'

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Начальный экран', icon: 'home' },
  { name: 'basic', path: '/select-basic', component: SelectBasic, display: 'Просмотр измерений' },
  { name: 'photo', path: '/photo-uploader', component: PhotoUploader, display: 'Фото' },
  { name: 'adddefect', path: '/defect-single', component: DefectSimple, display: 'Добавление дефекта' }
];
