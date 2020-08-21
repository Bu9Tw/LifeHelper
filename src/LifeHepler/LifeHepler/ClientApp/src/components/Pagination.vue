<template>
  <Paginate
    :page-count="totalPageCount"
    :page-range="3"
    :margin-pages="1"
    :click-handler="clickCallback"
    :prev-text="'Prev'"
    :next-text="'Next'"
    :container-class="'pagination'"
    :page-class="'page-item'"
    :page-link-class="'page-link'"
    :prev-class="'page-item'"
    :next-class="'page-item'"
    :prev-link-class="'page-link'"
    :next-link-class="'page-link'"
    :active-class="'active'"
  ></Paginate>
</template>
<script>
import Paginate from "vuejs-paginate";
export default {
  name: "Pagination",
  components: {
    Paginate,
  },
  data() {
    return {};
  },
  props: {
    totalPageCount: {
      type: Number,
    },
  },
  methods: {
    clickCallback(curPageNumber) {
      this.$emit("updatePageInfo", curPageNumber);
    },
  },
};
</script>