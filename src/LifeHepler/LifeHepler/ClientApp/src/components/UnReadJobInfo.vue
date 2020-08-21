<template>
  <div class="col-12">
    <div class="block-header">
      <!-- <h2 class="time-block">{{jobBlock.synchronizeDate}}</h2> -->
    </div>
    <div class="body">
      <article v-for="job in unReadJob" :key="job.no">
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
    pageRow: {
      type: Number,
    },
  },
  created() {
    this.GetUnReadJobInfo(1);
    this.SynJobData();
  },
  methods: {
    ReloadHistoryJobInfo(num) {
      console.log(num);
      this.GetUnReadJobInfo(num);
      this.$goToPageTop();
    },
    GetUnReadJobInfo(page) {
      $.get(
        `${this.hostUrl}/api/OneOFour/GetJobInfo?userType=${this.userType}&page=${page}&pageRow=${this.pageRow}`
      ).done((data) => {
        this.unReadJob = data.jobInfo;
        this.totalPageCount = data.totalPage;
      });
    },
    SynJobData() {
      $.post(`${this.hostUrl}/api/OneOFour/SynJobData`, {
        UserType: this.userType,
        PageRow: this.pageRow,
      }).done((data) => {
        this.totalPageCount = data;
        if (this.totalPageCount === 1) location.reload();
      });
    },
  },
};
</script>