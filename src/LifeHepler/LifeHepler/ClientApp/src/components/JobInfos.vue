<template>
  <div class="row">
    <div class="container">
      <div v-for="jobBlock in jobInfoDatas" :key="jobBlock">
        <div class="block-header">
          <h2 class="time-block">{{jobBlock.synchronizeDate}}</h2>
        </div>
        <div class="body">
          <article v-for="job in jobBlock.oneOFourHtmlJobInfos" :key="job">
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
    </div>
  </div>
</template>

<script>
import Vue from "vue";
import $ from "jquery";

export default {
  name: "JobInfos",
  data() {
    return {
      jobInfoDatas: [],
    };
  },
  created() {
    let searchParams = new URLSearchParams(window.location.search);
    let url = `/api/oneofour/GetJobInfo?type=${
      searchParams.has("type") ? searchParams.get("type") : "1"
    }`;
    Vue.axios.get(url).then((response) => {
      this.jobInfoDatas = [];
      $.each(response.data, (jobInfosIndex, jobInfos) => {
        this.jobInfoDatas.push(jobInfos);
      });
      console.log(this.jobInfoDatas);
    });
  },
};
</script>
