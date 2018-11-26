<template>
  <div id="app">

    <file-pond ref="pond"
               class-name="my-pond"
               label-idle="Загрузите фото..."
               labelFileProcessing = "Загрузка"
               labelFileProcessingComplete = "Загрузка завершена"
               labelTapToCancel ="Нажмите для отмены"
               labelTapToUndo ="Нажмите для удаления"
               allow-multiple="true"
               accepted-file-types="image/jpeg, image/png"
               v-on:init="handleFilePondInit"
               v-bind:server="server"/>
               
  </div>
</template>

<script>
  // Import FilePond
  import vueFilePond from 'vue-filepond';

  // Import plugins
  import FilePondPluginImagePreview from 'filepond-plugin-image-preview/dist/filepond-plugin-image-preview.esm.js';

  // Import styles
  import 'filepond/dist/filepond.min.css';
  import 'filepond-plugin-image-preview/dist/filepond-plugin-image-preview.min.css';

  // Create FilePond component
  const FilePond = vueFilePond(FilePondPluginImagePreview);

  
  
  export default {
    data: function () {
      return {
        server: {
          process: './api/photouploading/saveimage',
          fetch: null,
          revert: './api/photouploading/revertimage'
        }
      };
    },
    methods: {
      handleFilePondInit: function () {
        console.log('FilePond has initialized');
       
        
       
      }
    },
    components: {
      FilePond
    }
    
  };
</script>
