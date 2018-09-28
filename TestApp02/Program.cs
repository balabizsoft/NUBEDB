using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DALNUBEV2;

namespace TestApp02
{
    class Program
    {
        static nubev2Entities db = new nubev2Entities();
        static void Main(string[] args)
        {
            MemberInitScanByBankCode(3);
        }


        static void DuplicateMonthEndStatus()
        {
            AppLib.WriteLogFileName = "NUBE_DuplicateMonthEndStatus";

            AppLib.WriteLog("Duplicate Entry on Month End Status");
            AppLib.WriteLog("-----------------------------------");
            AppLib.WriteLog("");
            AppLib.WriteLog("");

            var lstEntry = db.MemberMonthEndStatus.GroupBy(x => new { x.MEMBER_CODE, x.MASTERMEMBER.STATUS_CODE, x.StatusMonth }).Where(x => x.Count() > 1).Select(x => new { x.Key.STATUS_CODE, x.Key.MEMBER_CODE, x.Key.StatusMonth, details = x }).OrderBy(x => x.MEMBER_CODE).ThenBy(x => x.StatusMonth).ToList();
            
            var lstMember = lstEntry.GroupBy(x => new { x.MEMBER_CODE,x.STATUS_CODE }).ToList();            

            AppLib.WriteLog("No of Duplicate Entry on Month End Status: {0}, No Member on their Entries: {1} [A: {2}, D: {3}, S: {4}, R: {5} ]", lstEntry.Count(), lstMember.Count(), lstMember.Where(x=> x.Key.STATUS_CODE == 1 || x.Key.STATUS_CODE==6).Count(), lstMember.Where(x=> x.Key.STATUS_CODE==2).Count(), lstMember.Where(x=> x.Key.STATUS_CODE==3).Count(), lstMember.Where(x=> x.Key.STATUS_CODE==4).Count() );
            
        }

        static void MemberInitScanByBankCode(decimal BANK_CODE)
        {
            try
            {
                AppLib.WriteLogFileName = "NUBE_" + "MemberInitScanByBankCode";

                AppLib.WriteLog("MemberInitScanByBankCode");
                AppLib.WriteLog("------------------------");
                AppLib.WriteLog("");
                AppLib.WriteLog("");

                var bank = db.MASTERBANKs.FirstOrDefault(x => x.BANK_CODE == BANK_CODE);
                if (bank != null)
                {
                    AppLib.WriteLog("Bank Id: {0}, Bank Code : {1}, Bank Name : {2}",bank.BANK_CODE, bank.BANK_USERCODE, bank.BANK_NAME);
                    AppLib.WriteLog("No of Members: {0} [CLERICAL: {1} [ New Join: {2}, Re-Join: {3}], NON-CLERICAL: {4} [ New Join: {5}, Re-Join: {6}]]", bank.MASTERMEMBERs.Count(), bank.MASTERMEMBERs.Where(x => x.MEMBERTYPE_CODE == 1).Count(), bank.MASTERMEMBERs.Where(x => x.MEMBERTYPE_CODE == 1 && x.REJOINED == 0).Count(), bank.MASTERMEMBERs.Where(x => x.MEMBERTYPE_CODE == 1 && x.REJOINED == 1).Count(), bank.MASTERMEMBERs.Where(x => x.MEMBERTYPE_CODE == 2).Count(), bank.MASTERMEMBERs.Where(x => x.MEMBERTYPE_CODE == 2 && x.REJOINED == 0).Count(), bank.MASTERMEMBERs.Where(x => x.MEMBERTYPE_CODE == 2 && x.REJOINED == 1).Count());
                    AppLib.WriteLog("");

                    #region Clerical New Join
                    if (bank.MASTERMEMBERs.Where(x=> x.MEMBERTYPE_CODE==1 && x.REJOINED==0).Count()>0)
                    {
                        int i = 1;
                        AppLib.WriteLog("");
                        AppLib.WriteLog("New Join CLERICAL");
                        AppLib.WriteLog("");
                        AppLib.WriteLog("{0,5} {1,-10} {2,-10} {3,-50} {4,-15} {5,-15} {6,-10} {7,-10} {8,-10} {9,-10} {10,-10} {11,-10} {12,-10} {13,-10} {14,-10} {15,-10} {16,-10} {17,-10} {18,-10} {19,-10} {20,-10} {21,-10} {22,-10} {23,-10} {24,-10}", "SNo", "Code", "MemberNo", "Member Name", "IC Old", "IC New", "DOB", "DOJ","MEND", "LPAID", "SUBSAMT", "TSUBSAMT", "ACCSUBS", "SUBSDUE", "BFAMT", "TBFAMT", "ACCBF", "BFDUE", "TOTMNTS", "TOTMNTSPAD", "TOTMNTSDUE", "STATUSCDE", "RESIGNED", "STRUCKOFF", "STATUS");
                        foreach (var mm in bank.MASTERMEMBERs.Where(x => x.MEMBERTYPE_CODE == 1 && x.REJOINED == 0).ToList())
                        {
                            var Status = mm.MemberMonthEndStatus.OrderBy(x => x.StatusMonth).FirstOrDefault();
                            string statusMsg = "";
                            if(mm.DATEOFJOINING>=new DateTime(2005, 9, 1))
                            {
                                if (Status.SUBSCRIPTION_AMOUNT != 8 || Status.TOTALSUBCRP_AMOUNT!=8 || Status.ACCSUBSCRIPTION!=8 || Status.SUBSCRIPTIONDUE!=0)
                                {
                                    statusMsg += "Subs Mismatch, ";
                                }

                                if (Status.BF_AMOUNT != 3 || Status.TOTALBF_AMOUNT != 3 || Status.ACCBF!= 3 || Status.BFDUE != 0)
                                {
                                    statusMsg += "BF Mismatch, ";
                                }

                                if (Status.TOTAL_MONTHS !=1 || Status.TOTALMONTHSPAID!=1 || Status.TOTALMONTHSDUE != 0)
                                {
                                    statusMsg += "Total Month Mismatch, ";
                                }

                                if(Status.STATUS_CODE!=1 && Status.STATUS_CODE != 5)
                                {
                                    statusMsg += "Status Coce Mismatch, ";
                                }

                                if (Status.STRUCKOFF == 1)
                                {
                                    statusMsg += "Struckoff Mismatch, ";
                                }

                                if (Status.RESIGNED == 1)
                                {
                                    statusMsg += "Resigned Mismatch, ";
                                }

                                if(mm.DATEOFJOINING.Value.Year != Status.LASTPAYMENTDATE.Value.Year ||  mm.DATEOFJOINING.Value.Month != Status.LASTPAYMENTDATE.Value.Month)
                                {
                                    statusMsg += "Last payment Date Mismatch, ";
                                }

                                if (mm.DATEOFJOINING.Value.Year != Status.StatusMonth.Value.Year || mm.DATEOFJOINING.Value.Month != Status.StatusMonth.Value.Month)
                                {
                                    statusMsg += "Status Month Date Mismatch, ";
                                }
                            }
                            else
                            {
                                if (Status.StatusMonth.Value.Year != 2005 || Status.StatusMonth.Value.Month != 09)
                                {
                                    statusMsg += "Status Month Date Mismatch, ";
                                }
                                var ts = DateTimeSpan.CompareDates( mm.DATEOFJOINING.Value , new DateTime(2005, 9, 1));
                                if ((Status.TOTALMONTHSPAID + Status.TOTALMONTHSDUE) != ( (ts.Years*12) + ts.Months)+2)
                                {
                                    statusMsg += "Total Month Mismatch, ";
                                }

                                if (Status.ACCSUBSCRIPTION != (Status.SUBSCRIPTION_AMOUNT * Status.TOTALMONTHSPAID) || Status.SUBSCRIPTIONDUE != (Status.SUBSCRIPTION_AMOUNT * Status.TOTALMONTHSDUE))
                                {
                                    statusMsg += "Subs Mismatch, ";
                                }

                                if (Status.ACCBF != (Status.TOTALMONTHSPAID * 3) || Status.BFDUE != (Status.TOTALMONTHSDUE *3) )
                                {
                                    statusMsg += "BF Mismatch, ";
                                }   
                            }

                            if (!string.IsNullOrWhiteSpace(statusMsg)){                               
                                try
                                {
                                    MemberMonthEndErrorLog d = new MemberMonthEndErrorLog() {
                                        Member_Code = mm.MEMBER_CODE,
                                        YY=Status.StatusMonth.Value.Year,
                                        MM=Status.StatusMonth.Value.Month,
                                        ErrorMsg = "Member_Init" + statusMsg
                                    };
                                }catch(Exception ex) { AppLib.WriteLog(ex); }
                                statusMsg = " Error=> " + statusMsg;
                            }

                            AppLib.WriteLog("{0,5} {1,-10} {2,-10} {3,-50} {4,-15} {5,-15} {6:dd/MM/yyyy} {7:dd/MM/yyyy} {8:dd/MM/yyyy} {9:dd/MM/yyyy} {10,-10} {11,-10} {12,-10} {13,-10} {14,-10} {15,-10} {16,-10} {17,-10} {18,-10} {19,-10} {20,-10} {21,-10} {22,-10} {23,-10} {24,-10}", i++, mm.MEMBER_CODE, mm.MEMBER_ID, mm.MEMBER_NAME, mm.ICNO_OLD, mm.ICNO_NEW, mm.DATEOFBIRTH, mm.DATEOFJOINING, Status.LASTPAYMENTDATE,Status.StatusMonth, Status.SUBSCRIPTION_AMOUNT, Status.TOTALSUBCRP_AMOUNT, Status.ACCSUBSCRIPTION, Status.SUBSCRIPTIONDUE, Status.BF_AMOUNT, Status.TOTALBF_AMOUNT, Status.ACCBF, Status.BFDUE, Status.TOTAL_MONTHS, Status.TOTALMONTHSPAID, Status.TOTALMONTHSPAID, Status.STATUS_CODE, Status.RESIGNED, Status.STRUCKOFF, statusMsg);

                        }
                    }
                    #endregion

                    #region Clerical Re-Join
                    if (bank.MASTERMEMBERs.Where(x => x.MEMBERTYPE_CODE == 1 && x.REJOINED == 1).Count() > 0)
                    {
                        int i = 1;
                        AppLib.WriteLog("");
                        AppLib.WriteLog("Re-Join CLERICAL");
                        AppLib.WriteLog("");
                        AppLib.WriteLog("{0,5} {1,-10} {2,-10} {3,-50} {4,-15} {5,-15} {6,-10} {7,-10} {8,-10} {9,-10} {10,-10} {11,-10} {12,-10} {13,-10} {14,-10} {15,-10} {16,-10} {18,-10} {19,-10} {20,-10} {21,-10} {22,-10} {23,-10} {24,-10} {25,-10}", "SNo", "Code", "MemberNo", "Member Name", "IC Old", "IC New", "DOB", "DOJ", "MEND", "LPAID", "SUBSAMT", "TSUBSAMT", "ACCSUBS", "SUBSDUE", "BFAMT", "TBFAMT", "ACCBF", "BFDUE", "TOTMNTS", "TOTMNTSPAD", "TOTMNTSDUE", "STATUSCDE", "RESIGNED", "STRUCKOFF", "STATUS");
                        foreach (var mm in bank.MASTERMEMBERs.Where(x => x.MEMBERTYPE_CODE == 1 && x.REJOINED == 1).ToList())
                        {
                            var Status = mm.MemberMonthEndStatus.OrderBy(x => x.StatusMonth).FirstOrDefault();
                            string statusMsg = "";
                            if (mm.DATEOFJOINING >= new DateTime(2005, 9, 1))
                            {
                                if (Status.SUBSCRIPTION_AMOUNT != 8 || Status.TOTALSUBCRP_AMOUNT != 8 || Status.ACCSUBSCRIPTION != 8 || Status.SUBSCRIPTIONDUE != 0)
                                {
                                    statusMsg += "Subs Mismatch, ";
                                }

                                if (Status.BF_AMOUNT != 3 || Status.TOTALBF_AMOUNT != 3 || Status.ACCBF != 3 || Status.BFDUE != 0)
                                {
                                    statusMsg += "BF Mismatch, ";
                                }

                                if (Status.TOTAL_MONTHS != 1 || Status.TOTALMONTHSPAID != 1 || Status.TOTALMONTHSDUE != 0)
                                {
                                    statusMsg += "Total Month Mismatch, ";
                                }

                                if (Status.STATUS_CODE != 1 && Status.STATUS_CODE != 5)
                                {
                                    statusMsg += "Status Coce Mismatch, ";
                                }

                                if (Status.STRUCKOFF == 1)
                                {
                                    statusMsg += "Struckoff Mismatch, ";
                                }

                                if (Status.RESIGNED == 1)
                                {
                                    statusMsg += "Resigned Mismatch, ";
                                }

                                if (mm.DATEOFJOINING.Value.Year != Status.LASTPAYMENTDATE.Value.Year || mm.DATEOFJOINING.Value.Month != Status.LASTPAYMENTDATE.Value.Month)
                                {
                                    statusMsg += "Last payment Date Mismatch, ";
                                }

                                if (mm.DATEOFJOINING.Value.Year != Status.StatusMonth.Value.Year || mm.DATEOFJOINING.Value.Month != Status.StatusMonth.Value.Month)
                                {
                                    statusMsg += "Status Month Date Mismatch, ";
                                }
                            }
                            else
                            {
                                if (Status.StatusMonth.Value.Year != 2005 || Status.StatusMonth.Value.Month != 09)
                                {
                                    statusMsg += "Status Month Date Mismatch, ";
                                }
                                var ts = DateTimeSpan.CompareDates(mm.DATEOFJOINING.Value, new DateTime(2005, 9, 1));
                                if ((Status.TOTALMONTHSPAID + Status.TOTALMONTHSDUE) != ((ts.Years * 12) + ts.Months))
                                {
                                    statusMsg += "Total Month Mismatch, ";
                                }

                                if (Status.ACCSUBSCRIPTION != (Status.SUBSCRIPTION_AMOUNT * Status.TOTALMONTHSPAID) || Status.SUBSCRIPTIONDUE != (Status.SUBSCRIPTION_AMOUNT * Status.TOTALMONTHSDUE))
                                {
                                    statusMsg += "Subs Mismatch, ";
                                }

                                if (Status.ACCBF != (Status.TOTALMONTHSPAID * 3) || Status.BFDUE != (Status.TOTALMONTHSDUE * 3))
                                {
                                    statusMsg += "BF Mismatch, ";
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(statusMsg))
                            {
                                try
                                {
                                    MemberMonthEndErrorLog d = new MemberMonthEndErrorLog()
                                    {
                                        Member_Code = mm.MEMBER_CODE,
                                        YY = Status.StatusMonth.Value.Year,
                                        MM = Status.StatusMonth.Value.Month,
                                        ErrorMsg = "Member_Init" + statusMsg
                                    };
                                }
                                catch (Exception ex) { AppLib.WriteLog(ex); }
                                statusMsg = " Error=> " + statusMsg;
                            }

                            AppLib.WriteLog("{0,5} {1,-10} {2,-10} {3,-50} {4,-15} {5,-15} {6:dd/MM/yyyy} {7:dd/MM/yyyy} {8:dd/MM/yyyy} {9:dd/MM/yyyy} {10,-10} {11,-10} {12,-10} {13,-10} {14,-10} {15,-10} {16,-10} {17,-10} {18,-10} {19,-10} {20,-10} {21,-10} {22,-10} {23,-10} {24,-10} {25,-10}", i++, mm.MEMBER_CODE, mm.MEMBER_ID, mm.MEMBER_NAME, mm.ICNO_OLD, mm.ICNO_NEW, mm.DATEOFBIRTH, mm.DATEOFJOINING, Status.LASTPAYMENTDATE, Status.StatusMonth, Status.SUBSCRIPTION_AMOUNT, Status.TOTALSUBCRP_AMOUNT, Status.ACCSUBSCRIPTION, Status.SUBSCRIPTIONDUE, Status.BF_AMOUNT, Status.TOTALBF_AMOUNT, Status.ACCBF, Status.BFDUE, Status.TOTAL_MONTHS, Status.TOTALMONTHSPAID, Status.TOTALMONTHSPAID, Status.STATUS_CODE, Status.RESIGNED, Status.STRUCKOFF, statusMsg);

                        }
                    }
                    #endregion

                    #region Non-Clerical New Join
                    if (bank.MASTERMEMBERs.Where(x => x.MEMBERTYPE_CODE == 2 && x.REJOINED == 0).Count() > 0)
                    {
                        int i = 1;
                        AppLib.WriteLog("");
                        AppLib.WriteLog("New Join NON-CLERICAL");
                        AppLib.WriteLog("");
                        AppLib.WriteLog("{0,5} {1,-10} {2,-10} {3,-50} {4,-15} {5,-15} {6,-10} {7,-10} {8,-10} {9,-10} {10,-10} {11,-10} {12,-10} {13,-10} {14,-10} {15,-10} {16,-10} {18,-10} {19,-10} {20,-10} {21,-10} {22,-10} {23,-10} {24,-10} {25,-10}", "SNo", "Code", "MemberNo", "Member Name", "IC Old", "IC New", "DOB", "DOJ", "MEND", "LPAID", "SUBSAMT", "TSUBSAMT", "ACCSUBS", "SUBSDUE", "BFAMT", "TBFAMT", "ACCBF", "BFDUE", "TOTMNTS", "TOTMNTSPAD", "TOTMNTSDUE", "STATUSCDE", "RESIGNED", "STRUCKOFF", "STATUS");
                        foreach (var mm in bank.MASTERMEMBERs.Where(x => x.MEMBERTYPE_CODE == 2 && x.REJOINED == 0).ToList())
                        {
                            var Status = mm.MemberMonthEndStatus.OrderBy(x => x.StatusMonth).FirstOrDefault();
                            string statusMsg = "";
                            if (mm.DATEOFJOINING >= new DateTime(2005, 9, 1))
                            {
                                if (Status.SUBSCRIPTION_AMOUNT != 8 || Status.TOTALSUBCRP_AMOUNT != 8 || Status.ACCSUBSCRIPTION != 8 || Status.SUBSCRIPTIONDUE != 0)
                                {
                                    statusMsg += "Subs Mismatch, ";
                                }

                                if (Status.BF_AMOUNT != 3 || Status.TOTALBF_AMOUNT != 3 || Status.ACCBF != 3 || Status.BFDUE != 0)
                                {
                                    statusMsg += "BF Mismatch, ";
                                }

                                if (Status.TOTAL_MONTHS != 1 || Status.TOTALMONTHSPAID != 1 || Status.TOTALMONTHSDUE != 0)
                                {
                                    statusMsg += "Total Month Mismatch, ";
                                }

                                if (Status.STATUS_CODE != 1 && Status.STATUS_CODE != 5)
                                {
                                    statusMsg += "Status Coce Mismatch, ";
                                }

                                if (Status.STRUCKOFF == 1)
                                {
                                    statusMsg += "Struckoff Mismatch, ";
                                }

                                if (Status.RESIGNED == 1)
                                {
                                    statusMsg += "Resigned Mismatch, ";
                                }

                                if (mm.DATEOFJOINING.Value.Year != Status.LASTPAYMENTDATE.Value.Year || mm.DATEOFJOINING.Value.Month != Status.LASTPAYMENTDATE.Value.Month)
                                {
                                    statusMsg += "Last payment Date Mismatch, ";
                                }

                                if (mm.DATEOFJOINING.Value.Year != Status.StatusMonth.Value.Year || mm.DATEOFJOINING.Value.Month != Status.StatusMonth.Value.Month)
                                {
                                    statusMsg += "Status Month Date Mismatch, ";
                                }
                            }
                            else
                            {
                                if (Status.StatusMonth.Value.Year != 2005 || Status.StatusMonth.Value.Month != 09)
                                {
                                    statusMsg += "Status Month Date Mismatch, ";
                                }
                                var ts = DateTimeSpan.CompareDates(mm.DATEOFJOINING.Value, new DateTime(2005, 9, 1));
                                if ((Status.TOTALMONTHSPAID + Status.TOTALMONTHSDUE) != ((ts.Years * 12) + ts.Months))
                                {
                                    statusMsg += "Total Month Mismatch, ";
                                }

                                if (Status.ACCSUBSCRIPTION != (Status.SUBSCRIPTION_AMOUNT * Status.TOTALMONTHSPAID) || Status.SUBSCRIPTIONDUE != (Status.SUBSCRIPTION_AMOUNT * Status.TOTALMONTHSDUE))
                                {
                                    statusMsg += "Subs Mismatch, ";
                                }

                                if (Status.ACCBF != (Status.TOTALMONTHSPAID * 3) || Status.BFDUE != (Status.TOTALMONTHSDUE * 3))
                                {
                                    statusMsg += "BF Mismatch, ";
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(statusMsg))
                            {
                                try
                                {
                                    MemberMonthEndErrorLog d = new MemberMonthEndErrorLog()
                                    {
                                        Member_Code = mm.MEMBER_CODE,
                                        YY = Status.StatusMonth.Value.Year,
                                        MM = Status.StatusMonth.Value.Month,
                                        ErrorMsg = "Member_Init" + statusMsg
                                    };
                                }
                                catch (Exception ex) { AppLib.WriteLog(ex); }
                                statusMsg = " Error=> " + statusMsg;
                            }

                            AppLib.WriteLog("{0,5} {1,-10} {2,-10} {3,-50} {4,-15} {5,-15} {6:dd/MM/yyyy} {7:dd/MM/yyyy} {8:dd/MM/yyyy} {9:dd/MM/yyyy} {10,-10} {11,-10} {12,-10} {13,-10} {14,-10} {15,-10} {16,-10} {17,-10} {18,-10} {19,-10} {20,-10} {21,-10} {22,-10} {23,-10} {24,-10} {25,-10}", i++, mm.MEMBER_CODE, mm.MEMBER_ID, mm.MEMBER_NAME, mm.ICNO_OLD, mm.ICNO_NEW, mm.DATEOFBIRTH, mm.DATEOFJOINING, Status.LASTPAYMENTDATE, Status.StatusMonth, Status.SUBSCRIPTION_AMOUNT, Status.TOTALSUBCRP_AMOUNT, Status.ACCSUBSCRIPTION, Status.SUBSCRIPTIONDUE, Status.BF_AMOUNT, Status.TOTALBF_AMOUNT, Status.ACCBF, Status.BFDUE, Status.TOTAL_MONTHS, Status.TOTALMONTHSPAID, Status.TOTALMONTHSPAID, Status.STATUS_CODE, Status.RESIGNED, Status.STRUCKOFF, statusMsg);

                        }
                    }
                    #endregion

                    #region Non-Clerical Re-Join
                    if (bank.MASTERMEMBERs.Where(x => x.MEMBERTYPE_CODE == 2 && x.REJOINED == 1).Count() > 0)
                    {
                        int i = 1;
                        AppLib.WriteLog("");
                        AppLib.WriteLog("Re-Join Non-CLERICAL");
                        AppLib.WriteLog("");
                        AppLib.WriteLog("{0,5} {1,-10} {2,-10} {3,-50} {4,-15} {5,-15} {6,-10} {7,-10} {8,-10} {9,-10} {10,-10} {11,-10} {12,-10} {13,-10} {14,-10} {15,-10} {16,-10} {18,-10} {19,-10} {20,-10} {21,-10} {22,-10} {23,-10} {24,-10} {25,-10}", "SNo", "Code", "MemberNo", "Member Name", "IC Old", "IC New", "DOB", "DOJ", "MEND", "LPAID", "SUBSAMT", "TSUBSAMT", "ACCSUBS", "SUBSDUE", "BFAMT", "TBFAMT", "ACCBF", "BFDUE", "TOTMNTS", "TOTMNTSPAD", "TOTMNTSDUE", "STATUSCDE", "RESIGNED", "STRUCKOFF", "STATUS");
                        foreach (var mm in bank.MASTERMEMBERs.Where(x => x.MEMBERTYPE_CODE == 2 && x.REJOINED == 1).ToList())
                        {
                            var Status = mm.MemberMonthEndStatus.OrderBy(x => x.StatusMonth).FirstOrDefault();
                            string statusMsg = "";
                            if (mm.DATEOFJOINING >= new DateTime(2005, 9, 1))
                            {
                                if (Status.SUBSCRIPTION_AMOUNT != 8 || Status.TOTALSUBCRP_AMOUNT != 8 || Status.ACCSUBSCRIPTION != 8 || Status.SUBSCRIPTIONDUE != 0)
                                {
                                    statusMsg += "Subs Mismatch, ";
                                }

                                if (Status.BF_AMOUNT != 3 || Status.TOTALBF_AMOUNT != 3 || Status.ACCBF != 3 || Status.BFDUE != 0)
                                {
                                    statusMsg += "BF Mismatch, ";
                                }

                                if (Status.TOTAL_MONTHS != 1 || Status.TOTALMONTHSPAID != 1 || Status.TOTALMONTHSDUE != 0)
                                {
                                    statusMsg += "Total Month Mismatch, ";
                                }

                                if (Status.STATUS_CODE != 1 && Status.STATUS_CODE != 5)
                                {
                                    statusMsg += "Status Coce Mismatch, ";
                                }

                                if (Status.STRUCKOFF == 1)
                                {
                                    statusMsg += "Struckoff Mismatch, ";
                                }

                                if (Status.RESIGNED == 1)
                                {
                                    statusMsg += "Resigned Mismatch, ";
                                }

                                if (mm.DATEOFJOINING.Value.Year != Status.LASTPAYMENTDATE.Value.Year || mm.DATEOFJOINING.Value.Month != Status.LASTPAYMENTDATE.Value.Month)
                                {
                                    statusMsg += "Last payment Date Mismatch, ";
                                }

                                if (mm.DATEOFJOINING.Value.Year != Status.StatusMonth.Value.Year || mm.DATEOFJOINING.Value.Month != Status.StatusMonth.Value.Month)
                                {
                                    statusMsg += "Status Month Date Mismatch, ";
                                }
                            }
                            else
                            {
                                if (Status.StatusMonth.Value.Year != 2005 || Status.StatusMonth.Value.Month != 09)
                                {
                                    statusMsg += "Status Month Date Mismatch, ";
                                }
                                var ts = DateTimeSpan.CompareDates(mm.DATEOFJOINING.Value, new DateTime(2005, 9, 1));
                                if ((Status.TOTALMONTHSPAID + Status.TOTALMONTHSDUE) != ((ts.Years * 12) + ts.Months))
                                {
                                    statusMsg += "Total Month Mismatch, ";
                                }

                                if (Status.ACCSUBSCRIPTION != (Status.SUBSCRIPTION_AMOUNT * Status.TOTALMONTHSPAID) || Status.SUBSCRIPTIONDUE != (Status.SUBSCRIPTION_AMOUNT * Status.TOTALMONTHSDUE))
                                {
                                    statusMsg += "Subs Mismatch, ";
                                }

                                if (Status.ACCBF != (Status.TOTALMONTHSPAID * 3) || Status.BFDUE != (Status.TOTALMONTHSDUE * 3))
                                {
                                    statusMsg += "BF Mismatch, ";
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(statusMsg))
                            {
                                try
                                {
                                    MemberMonthEndErrorLog d = new MemberMonthEndErrorLog()
                                    {
                                        Member_Code = mm.MEMBER_CODE,
                                        YY = Status.StatusMonth.Value.Year,
                                        MM = Status.StatusMonth.Value.Month,
                                        ErrorMsg = "Member_Init" + statusMsg
                                    };
                                }
                                catch (Exception ex) { AppLib.WriteLog(ex); }
                                statusMsg = " Error=> " + statusMsg;
                            }

                            AppLib.WriteLog("{0,5} {1,-10} {2,-10} {3,-50} {4,-15} {5,-15} {6:dd/MM/yyyy} {7:dd/MM/yyyy} {8:dd/MM/yyyy} {9:dd/MM/yyyy} {10,-10} {11,-10} {12,-10} {13,-10} {14,-10} {15,-10} {16,-10} {17,-10} {18,-10} {19,-10} {20,-10} {21,-10} {22,-10} {23,-10} {24,-10} {25,-10}", i++, mm.MEMBER_CODE, mm.MEMBER_ID, mm.MEMBER_NAME, mm.ICNO_OLD, mm.ICNO_NEW, mm.DATEOFBIRTH, mm.DATEOFJOINING, Status.LASTPAYMENTDATE, Status.StatusMonth, Status.SUBSCRIPTION_AMOUNT, Status.TOTALSUBCRP_AMOUNT, Status.ACCSUBSCRIPTION, Status.SUBSCRIPTIONDUE, Status.BF_AMOUNT, Status.TOTALBF_AMOUNT, Status.ACCBF, Status.BFDUE, Status.TOTAL_MONTHS, Status.TOTALMONTHSPAID, Status.TOTALMONTHSPAID, Status.STATUS_CODE, Status.RESIGNED, Status.STRUCKOFF, statusMsg);

                        }
                    }

                    #endregion
                }
                else
                {
                    AppLib.WriteLog("Bank Code {0} is not found", BANK_CODE);
                }
            }
            catch(Exception ex) { AppLib.WriteLog(ex); }
           

        }



    }
}
