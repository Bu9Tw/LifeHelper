<template>
  <div class="container">
    <a class="btn btn-info" target="_blank" :href="sourceUrl">條件連結</a>
    <div class="row">
      <div class="col-12">
        <div class="block-header"></div>
        <div class="body">
          <Block
            v-for="jobInfo in jobInfos"
            :key="jobInfo.no"
            :job="jobInfo"
            @ReadedCallBack="UpdateJobInfos"
          />
        </div>
        <Pagination @updatePageInfo="ReloadHistoryJobInfo" :totalPageCount="totalPageCount" />
      </div>
    </div>
  </div>
</template>

<script>
import $ from "jquery";
import Pagination from "../../components/Pagination";
import Block from "../OneOFour/Block";

export default {
  name: "JobInfos",
  components: {
    Pagination,
    Block,
  },
  data() {
    return {
      userType: "",
      sourceUrl: "",
      pageRow: 20,
      //hostUrl: "https://localhost:44331",
      hostUrl: "",
      jobInfos: [],
      totalPageCount: 1,
      curPageNumber: 1,
    };
  },
  created() {
    let searchParams = new URLSearchParams(window.location.search);
    this.userType = searchParams.has("type") ? searchParams.get("type") : "1";
    this.GetSourceUrl();
    this.GetJobInfo(this.totalPageCount);
  },
  methods: {
    //取得搜尋來源的html
    GetSourceUrl() {
      $.get(
        `${this.hostUrl}/api/OneOFour/GetSourceUrl?userType=${this.userType}`
      ).done((sourceUrl) => {
        this.sourceUrl = sourceUrl;
      });
    },
    //Pagination換頁時Reload的Method
    ReloadHistoryJobInfo(num) {
      this.curPageNumber = num;
      this.GetJobInfo(num);
      this.$goToPageTop();
    },
    //取得工作資訊
    GetJobInfo(page) {
      $.get(
        `${this.hostUrl}/api/OneOFour/GetJobInfo?userType=${this.userType}&page=${page}&pageRow=${this.pageRow}`
      ).done((data) => {
        this.jobInfos = data.jobInfo;
        this.totalPageCount = data.totalPage;
      });
    },
    UpdateJobInfos(jobNo) {
      let job = $.grep(this.jobInfos, function (item) {
        return item.no === jobNo;
      })[0];
      let delIndex = this.jobInfos.indexOf(job);

      this.jobInfos.splice(delIndex, 1);

      $.post(`${this.hostUrl}/api/OneOFour/UpdateToReaded`, {
        UserType: this.userType,
        JobNo: jobNo,
        CurPageNumber: this.curPageNumber,
        PageRow: this.pageRow,
      })
        .done((data) => {
          let existData = this.jobInfos.find((x) => x.no === data.no);
          if (data && !existData) this.jobInfos.push(data);
        })
        .fail((data) => {
          this.$toast.error(`${job.no}-${job.name} 更新錯誤!`);
          console.log(data);
        });
    },
  },
};
</script>
