(function(t){function e(e){for(var o,s,c=e[0],i=e[1],l=e[2],p=0,f=[];p<c.length;p++)s=c[p],Object.prototype.hasOwnProperty.call(a,s)&&a[s]&&f.push(a[s][0]),a[s]=0;for(o in i)Object.prototype.hasOwnProperty.call(i,o)&&(t[o]=i[o]);u&&u(e);while(f.length)f.shift()();return r.push.apply(r,l||[]),n()}function n(){for(var t,e=0;e<r.length;e++){for(var n=r[e],o=!0,c=1;c<n.length;c++){var i=n[c];0!==a[i]&&(o=!1)}o&&(r.splice(e--,1),t=s(s.s=n[0]))}return t}var o={},a={app:0},r=[];function s(e){if(o[e])return o[e].exports;var n=o[e]={i:e,l:!1,exports:{}};return t[e].call(n.exports,n,n.exports,s),n.l=!0,n.exports}s.m=t,s.c=o,s.d=function(t,e,n){s.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:n})},s.r=function(t){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},s.t=function(t,e){if(1&e&&(t=s(t)),8&e)return t;if(4&e&&"object"===typeof t&&t&&t.__esModule)return t;var n=Object.create(null);if(s.r(n),Object.defineProperty(n,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var o in t)s.d(n,o,function(e){return t[e]}.bind(null,o));return n},s.n=function(t){var e=t&&t.__esModule?function(){return t["default"]}:function(){return t};return s.d(e,"a",e),e},s.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},s.p="/";var c=window["webpackJsonp"]=window["webpackJsonp"]||[],i=c.push.bind(c);c.push=e,c=c.slice();for(var l=0;l<c.length;l++)e(c[l]);var u=i;r.push([0,"chunk-vendors"]),n()})({0:function(t,e,n){t.exports=n("56d7")},"56d7":function(t,e,n){"use strict";n.r(e);n("e260"),n("e6cf"),n("cca6"),n("a79d");var o=n("2b0e"),a=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{attrs:{id:"app"}},[n("JobInfos")],1)},r=[],s=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{staticClass:"container"},[n("a",{staticClass:"btn btn-info",attrs:{target:"_blank",href:t.sourceUrl}},[t._v("條件連結")]),n("div",{staticClass:"row"},[n("div",{staticClass:"col-12"},[n("div",{staticClass:"block-header"}),n("div",{staticClass:"body"},t._l(t.jobInfos,(function(e){return n("Block",{key:e.no,attrs:{job:e},on:{ReadedCallBack:t.UpdateJobInfos}})})),1),n("Pagination",{attrs:{totalPageCount:t.totalPageCount},on:{updatePageInfo:t.ReloadHistoryJobInfo}})],1)])])},c=[],i=(n("99af"),n("c975"),n("a434"),n("b0c0"),n("d3b7"),n("ac1f"),n("3ca3"),n("841c"),n("ddb0"),n("2b3d"),n("1157")),l=n.n(i),u=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("Paginate",{attrs:{"page-count":t.totalPageCount,"page-range":3,"margin-pages":1,"click-handler":t.clickCallback,"prev-text":"Prev","next-text":"Next","container-class":"pagination","page-class":"page-item","page-link-class":"page-link","prev-class":"page-item","next-class":"page-item","prev-link-class":"page-link","next-link-class":"page-link","active-class":"active"}})},p=[],f=(n("a9e3"),n("8832")),d=n.n(f),b={name:"Pagination",components:{Paginate:d.a},data:function(){return{}},props:{totalPageCount:{type:Number}},methods:{clickCallback:function(t){this.$emit("updatePageInfo",t)}}},h=b,g=n("2877"),v=Object(g["a"])(h,u,p,!1,null,null,null),m=v.exports,y=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("article",[n("div",{staticClass:"article-header"},[n("h1",{staticClass:"header-title"},[t._v(t._s(t.job.name))]),n("span",{staticClass:"article-tag"},[t._v(t._s(t.job.pay))]),n("span",{staticClass:"article-tag"},[t._v(t._s(t.job.workTime))])]),n("section",{staticClass:"article-body"},[n("h4",[t._v(t._s(t.job.companyName))]),n("h5",[t._v(t._s(t.job.workPlace))])]),n("div",{staticClass:"row"},[n("div",{staticClass:"col-5"},[n("a",{staticClass:"btn btn-info",attrs:{target:"_blank",href:t.job.detailLink}},[t._v("104 資訊")])]),n("div",{staticClass:"col-5"},[n("button",{staticClass:"btn btn-warning",on:{click:t.Readed}},[t._v("已讀")])])])])},j=[],k={name:"Block",components:{},data:function(){return{}},props:{job:{no:"",name:"",pay:"",workTime:"",companyName:"",workPlace:"",detailLink:""}},created:function(){},methods:{Readed:function(){this.$emit("ReadedCallBack",this.job.no)}}},_=k,P=Object(g["a"])(_,y,j,!1,null,null,null),C=P.exports,w={name:"JobInfos",components:{Pagination:m,Block:C},data:function(){return{userType:"",sourceUrl:"",pageRow:20,hostUrl:"https://localhost:44331",jobInfos:[],totalPageCount:1}},created:function(){var t=new URLSearchParams(window.location.search);this.userType=t.has("type")?t.get("type"):"1",this.GetSourceUrl(),this.GetJobInfo(this.totalPageCount)},methods:{GetSourceUrl:function(){var t=this;l.a.get("".concat(this.hostUrl,"/api/OneOFour/GetSourceUrl?userType=").concat(this.userType)).done((function(e){t.sourceUrl=e}))},ReloadHistoryJobInfo:function(t){console.log(t),this.GetJobInfo(t),this.$goToPageTop()},GetJobInfo:function(t){var e=this;l.a.get("".concat(this.hostUrl,"/api/OneOFour/GetJobInfo?userType=").concat(this.userType,"&page=").concat(t,"&pageRow=").concat(this.pageRow)).done((function(t){e.jobInfos=t.jobInfo,e.totalPageCount=t.totalPage}))},UpdateJobInfos:function(t){var e=l.a.grep(this.jobInfos,(function(e){return e.no===t}))[0],n=this.jobInfos.indexOf(e);this.jobInfos.splice(n,1),l.a.post("".concat(this.hostUrl,"/api/OneOFour/UpdateToReaded"),{UserType:this.userType,JobNo:t}).fail((function(t){this.$toast.error("".concat(e.no,"-").concat(e.name," 更新錯誤!")),console.log(t)})),console.log("jobinfo : "+t)}}},O=w,I=Object(g["a"])(O,s,c,!1,null,null,null),T=I.exports,x=(n("4989"),n("ab8b"),n("ad0b"),{name:"App",components:{JobInfos:T}}),U=x,J=Object(g["a"])(U,a,r,!1,null,null,null),R=J.exports,$=n("6c42");n("da96");o["a"].config.productionTip=!1;var S={};o["a"].use($["a"],S),o["a"].prototype.$goToPageTop=function(){return l()("html,body").animate({scrollTop:0},"fast"),!1},new o["a"]({render:function(t){return t(R)}}).$mount("#app")},ad0b:function(t,e,n){}});
//# sourceMappingURL=app.78ea5f52.js.map