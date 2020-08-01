<template>
  <div id="app">
    <!-- <img alt="Vue logo" src="./assets/logo.png" />
    <HelloWorld msg="Welcome to Your Vue.js App" />-->
    <JobInfos />
  </div>
</template>

<script>
import JobInfos from "./components/JobInfos.vue";
import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "./Content/Site.css";

export default {
  name: "App",
  components: {
    JobInfos,
  },
};
</script>
