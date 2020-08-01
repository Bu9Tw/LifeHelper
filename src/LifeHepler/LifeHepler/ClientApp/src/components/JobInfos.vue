<template>
  <div class="row">
    <div class="container">
      <div class="job-block" v-for="jobBlock in jobInfoDatas" :key="jobBlock">
        <div class="header">
          <h2 class="time-block">{{jobBlock.synchronizeDate}}</h2>
        </div>
        <div class="body">
          <ul v-for="job in jobBlock.oneOFourHtmlJobInfos" :key="job">
            <li>{{job.companyName}}</li>
            <li>{{job.name}}</li>
            <li>
              <a class="btn btn-info" target="_blank" :href="job.detailLink">104資訊</a>
            </li>
          </ul>
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
