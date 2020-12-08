using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class AvailableCandidates : List<AvailableCandidate>
    {
        public DataTable DtAvailableCandidates { get; set; }
        public string CnnStr { get; set; }

        public AvailableCandidates(int requestid)
        {
            this.CnnStr = ConfigurationManager.ConnectionStrings["BIT"].ConnectionString;

            DataAccessLayer myDAL = new DataAccessLayer(this.CnnStr);

            myDAL.AddParameter("@RequestId", Convert.ToInt32(requestid), DataAccessLayer.GenericDataType.genInteger);

            DtAvailableCandidates = myDAL.RunSPDataTable("usp_AvailableCandidates");

            foreach (DataRow dr in DtAvailableCandidates.Rows)
            {
                AvailableCandidate candidate = new AvailableCandidate(dr);
                this.Add(candidate);
            }


            //string sqlStr = "declare @startDate date declare @endDate date select @startDate = r.EarliestStartDate, @endDate = r.DueDate from REQUEST r where r.RequestId = '" + requestid + "' ;with dateRange as (select[Date] = dateadd(dd, 0, @startDate) where dateadd(dd, 1, @startDate) <= @endDate union all select dateadd(dd, 1, [Date]) from dateRange where dateadd(dd, 1, [Date]) <= @endDate) select r.RequestId, a.WeekDayName, [Date], ps.SkillName, psu.Suburb, c.ContractorId, b.FirstName, b.LastName from REQUEST r, PREFERRED_SUBURB psu, BIT_USER b, PREFERRED_SKILL ps, CONTRACTOR c, AVAILABILITY a, LOCATION l, dateRange where r.RequestId = '" + requestid + "' AND l.LocationId = r.LocationId AND c.ContractorId = a.ContractorId AND b.UserId = c.ContractorId AND c.ContractorId = ps.ContractorId AND c.ContractorId = psu.ContractorId AND psu.Suburb = l.Suburb AND ps.SkillName = r.SkillName AND a.WeekDayName = DATENAME(dw, [Date]) AND r.Status = 'Pending' AND (c.Active = 'Available' OR c.Active = 'In Progress') AND [Date] NOT IN (Select s.StartDate From SCHEDULED s Where s.ContractorId = c.ContractorId AND s.Status != 'Rejected')";

            //SQLHelper objHelper = new SQLHelper("BIT");
            //DataTable candidatesTable = objHelper.ExecuteSQL(sqlStr);

            //foreach (DataRow dr in candidatesTable.Rows)
            //{
            //    AvailableCandidate candidate = new AvailableCandidate(dr);
            //    this.Add(candidate);
            //}
        }
    }
}
