<template>
  <div class="col-6">
    <div v-for="jobBlock in unReadJob" :key="jobBlock.synchronizeDate">
      <div class="block-header">
        <h2 class="time-block">{{jobBlock.synchronizeDate}}</h2>
      </div>
      <div class="body">
        <article v-for="job in jobBlock.oneOFourHtmlJobInfos" :key="job.no">
          <div class="article-header">
            <h1 class="header-title">{{job.name}}</h1>
            <span class="artice-tag">{{job.pay}}</span>
            <span class="artice-tag">{{job.workTime}}</span>
          </div>
          <section class="article-body">
            <h4>{{job.companyName}}</h4>
            <h5>{{job.workPlace}}</h5>
            <a class="btn btn-info" target="_blank" :href="job.detailLink">104資訊</a>
          </section>
        </article>
      </div>
    </div>
    <Pagination @updatePageInfo="ReloadHistoryJobInfo" :totalPageCount="totalPageCount" />
  </div>
</template>
<script>
import Pagination from "./Pagination.vue";
import $ from "jquery";

export default {
  name: "UnReadJobInfo",
  components: {
    Pagination,
  },
  data() {
    return {
      unReadJob: [],
      totalPageCount: 1,
    };
  },
  props: {
    hostUrl: {
      type: String,
    },
    userType: {
      type: String,
    },
  },
  created() {
    this.GetUnReadJobInfo();
  },
  methods: {
    ReloadHistoryJobInfo(num) {
      console.log(num);
    },
    GetUnReadJobInfo() {
      $.get(
        `${this.hostUrl}/api/OneOFour/GetJobInfo?type=${this.userType}`
      ).done((data) => {
        this.unReadJob = data;
      });
    },
  },
};
</script>