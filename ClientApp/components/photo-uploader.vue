<template>
  <div id="app">

    <file-pond name="test"
               ref="pond"
               class-name="my-pond"
               label-idle="Drop files here..."
               allow-multiple="true"
               accepted-file-types="image/jpeg, image/png"
               v-bind:files="myFiles"
               v-on:init="handleFilePondInit" />

  </div>
</template>

<script>
  // Import FilePond
  import vueFilePond from 'vue-filepond';

  // Import plugins
  import FilePondPluginFileValidateType from 'filepond-plugin-file-validate-type/dist/filepond-plugin-file-validate-type.esm.js';
  import FilePondPluginImagePreview from 'filepond-plugin-image-preview/dist/filepond-plugin-image-preview.esm.js';

  // Import styles
  import 'filepond/dist/filepond.min.css';
  import 'filepond-plugin-image-preview/dist/filepond-plugin-image-preview.min.css';

  // Create FilePond component
  const FilePond = vueFilePond(FilePondPluginFileValidateType, FilePondPluginImagePreview);

  export default {
    name: 'app',
    data: function () {
      return { myFiles: ['index.html'] };
    },
    methods: {
      handleFilePondInit: function () {
        console.log('FilePond has initialized');

        // example of instance method call on pond reference
        this.$refs.pond.getFiles();
      }
    },
    components: {
      FilePond
    }
  };
</script>
