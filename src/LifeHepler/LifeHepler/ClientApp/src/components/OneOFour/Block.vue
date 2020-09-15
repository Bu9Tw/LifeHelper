<template>
  <article>
    <div class="article-header">
      <h1 class="header-title">{{job.name}}</h1>
      <span class="article-tag">{{job.pay}}</span>
      <span class="article-tag">{{job.workTime}}</span>
    </div>
    <section class="article-body">
      <h4>{{job.companyName}}</h4>
      <h5>{{job.workPlace}}</h5>
    </section>
    <div class="row">
      <div class="col-5">
        <a class="btn btn-info" target="_blank" :href="job.detailLink">104 資訊</a>
      </div>
      <div class="col-5">
        <button class="btn btn-warning" @click="Readed">已讀</button>
      </div>
    </div>
  </article>
</template>
<script>
export default {
  name: "Block",
  components: {},
  data() {
    return {};
  },
  props: {
    job: {
      no: "",
      name: "",
      pay: "",
      workTime: "",
      companyName: "",
      workPlace: "",
      detailLink: "",
    },
  },
  created() {},
  methods: {
    Readed() {
      this.$emit("ReadedCallBack", this.job.no);
    },
  },
};
</script>