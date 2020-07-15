using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Crawler
{
    public class JobDetailInfo
    {
        public Data data { get; set; }
        public class CorpImageTop
        {
            public string imageUrl { get; set; }
            public string link { get; set; }
        }

        public class Header
        {
            public CorpImageTop corpImageTop { get; set; }
            public string jobName { get; set; }
            public string appearDate { get; set; }
            public string custName { get; set; }
            public string custUrl { get; set; }
            public string applyDate { get; set; }
            public int analysisType { get; set; }
            public string analysisUrl { get; set; }
            public bool isSaved { get; set; }
            public bool isApplied { get; set; }
        }

        public class Contact
        {
            public string hrName { get; set; }
            public string email { get; set; }
            public string visit { get; set; }
            public string phone { get; set; }
            public string other { get; set; }
            public string reply { get; set; }
        }

        public class Role
        {
            public int code { get; set; }
            public string description { get; set; }
        }

        public class DisRole
        {
            public bool needHandicapCompendium { get; set; }
            public IList<object> disability { get; set; }
        }

        public class AcceptRole
        {
            public IList<Role> role { get; set; }
            public DisRole disRole { get; set; }
        }

        public class Condition
        {
            public AcceptRole acceptRole { get; set; }
            public string workExp { get; set; }
            public string edu { get; set; }
            public IList<object> major { get; set; }
            public IList<object> language { get; set; }
            public IList<object> localLanguage { get; set; }
            public IList<JobSpecialty> specialty { get; set; }
            public IList<object> skill { get; set; }
            public IList<object> certificate { get; set; }
            public IList<object> driverLicense { get; set; }
            public string other { get; set; }
        }

        public class JobSpecialty
        {
            public string code { get; set; }

            public string description { get; set; }
        }

        public class Welfare
        {
            public IList<object> tag { get; set; }
            public string welfare { get; set; }
        }

        public class JobCategory
        {
            public string code { get; set; }
            public string description { get; set; }
        }

        public class JobDetail
        {
            public string jobDescription { get; set; }
            public IList<JobCategory> jobCategory { get; set; }
            public string salary { get; set; }
            public int salaryMin { get; set; }
            public int salaryMax { get; set; }
            public int salaryType { get; set; }
            public int jobType { get; set; }
            public IList<object> workType { get; set; }
            public string addressRegion { get; set; }
            public string addressDetail { get; set; }
            public string industryArea { get; set; }
            public string longitude { get; set; }
            public string latitude { get; set; }
            public string manageResp { get; set; }
            public string businessTrip { get; set; }
            public string workPeriod { get; set; }
            public string vacationPolicy { get; set; }
            public string startWorkingDay { get; set; }
            public int hireType { get; set; }
            public string delegatedRecruit { get; set; }
            public string needEmp { get; set; }
        }

        public class Data
        {
            public Header header { get; set; }
            public Contact contact { get; set; }
            public Condition condition { get; set; }
            public Welfare welfare { get; set; }
            public JobDetail jobDetail { get; set; }
            public string custLogo { get; set; }
            public string postalCode { get; set; }
            public string closeDate { get; set; }
            public string industry { get; set; }
            public string custNo { get; set; }
            public string reportUrl { get; set; }
        }

    }

}
