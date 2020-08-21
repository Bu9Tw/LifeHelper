<template>
  <div class="container">
    <a class="btn btn-info" target="_blank" :href="sourceUrl">條件連結</a>
    <div class="row">
      <UnReadJobInfo :hostUrl="hostUrl" :userType="userType" />
    </div>
  </div>
</template>

<script>
// import Vue from "vue";
import $ from "jquery";
import UnReadJobInfo from "../components/UnReadJobInfo.vue";

export default {
  name: "JobInfos",
  components: {
    UnReadJobInfo,
  },
  data() {
    return {
      userType: "",
      sourceUrl: "",
      pageRow: 20,
      // hostUrl: "https://localhost:44331",
      hostUrl: "",
    };
  },
  created() {
    let searchParams = new URLSearchParams(window.location.search);
    this.userType = searchParams.has("type") ? searchParams.get("type") : "1";
    this.GetSourceUrl();
  },
  methods: {
    GetSourceUrl() {
      $.get(
        `${this.hostUrl}/api/OneOFour/GetSourceUrl?userType=${this.userType}`
      ).done((sourceUrl) => {
        this.sourceUrl = sourceUrl;
      });
    },
  },
};
</script>
