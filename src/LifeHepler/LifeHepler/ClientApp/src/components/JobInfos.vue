<template>
  <div class="container">
    <a class="btn btn-info" target="_blank" :href="sourceUrl">條件連結</a>
    <div class="row">
      <div class="col-6">
        <div v-for="jobBlock in existJobInfoDatas" :key="jobBlock">
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
      <div class="col-6">
        <div v-for="jobBlock in latestJobInfoDatas" :key="jobBlock">
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
  </div>
</template>

<script>
// import Vue from "vue";
import $ from "jquery";

export default {
  name: "JobInfos",
  data() {
    return {
      existJobInfoDatas: [],
      latestJobInfoDatas: [],
      sourceUrl: "",
      hostUrl: "",
    };
  },
  created() {
    let searchParams = new URLSearchParams(window.location.search);
    let type = searchParams.has("type") ? searchParams.get("type") : "1";

    $.post(`${this.hostUrl}/api/OneOFour/SynJobData?type=${type}`, {
      type: type,
    }).done((data) => {
      this.latestJobInfoDatas = data;
    });

    $.when(
      $.get(`${this.hostUrl}/api/OneOFour/GetSourceUrl?type=${type}`),
      $.get(`${this.hostUrl}/api/OneOFour/GetJobInfo?type=${type}`)
    ).done((sourceUrl, jobInfo) => {
      this.sourceUrl = sourceUrl[0];
      this.existJobInfoDatas = jobInfo[0];
    });
  },
};
</script>
