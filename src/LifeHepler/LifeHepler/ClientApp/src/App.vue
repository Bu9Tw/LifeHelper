<template>
  <div id="app">
    <JobInfos />
  </div>
</template>

<script>
import JobInfos from "./components/OneOFour/JobInfos.vue";
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
