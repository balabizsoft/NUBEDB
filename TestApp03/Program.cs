using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace TestApp03
{
    class Program
    {
        static DALNUBEV2.nubev2Entities dbv2 = new DALNUBEV2.nubev2Entities();
        static DALNUBEV3.DBNUBEMembership_3_0_1Entities dbv3 = new DALNUBEV3.DBNUBEMembership_3_0_1Entities();

        //static void WriteCityStateAndCountry()
        //{
        //    Console.WriteLine("Start_Country");
        //    DALNUBEV3.Country Country = new DALNUBEV3.Country() { Name = "Malaysia" };
        //    dbv3.Countries.Add(Country);
        //    Console.WriteLine("End_Country");
        //    Console.WriteLine("Start_State and City");
        //    foreach (var s in dbv2.MASTERSTATEs.ToList())
        //    {
        //        DALNUBEV3.State s3 = new DALNUBEV3.State() { Name = s.STATE_NAME, StateCode = s.STATE_CODE.ToString(), isDeleted = s.DELETED == 1 };
        //        Country.States.Add(s3);
        //        foreach (var c in dbv2.MASTERCITies.Where(x => x.STATE_CODE == s.STATE_CODE).ToList())
        //        {
        //            DALNUBEV3.City c3 = new DALNUBEV3.City() { Name = c.CITY_NAME, CityCode = c.CITY_CODE.ToString(), isDeleted = c.DELETED == 1 };
        //            s3.Cities.Add(c3);
        //        }
        //    }
        //    Console.WriteLine("End_State and City");
        //    dbv3.SaveChanges();
        //}

        static void WriteNubeBranch()
        {
            Console.WriteLine("Start_NUBE Branch");
            var cm = dbv3.CompanyDetails.FirstOrDefault();
            foreach (var nb in dbv2.MASTERNUBEBRANCHes.ToList())
            {
                cm.NUBEBranches.Add(new DALNUBEV3.NUBEBranch() { Name = nb.NUBE_BRANCH_NAME, V2Code = nb.NUBE_BRANCH_CODE.ToString() });
            }
            Console.WriteLine("End_NUBE Branch");
            dbv3.SaveChanges();
        }

        static void PersonTitle()
        {
            Console.WriteLine("Start_Persion Title");
            var cm = dbv3.CompanyDetails.FirstOrDefault();
            var lst = dbv2.MASTERMEMBERs.Select(x => x.MEMBER_TITLE).Distinct().ToList();
            foreach (var r in lst)
            {
              if(!string.IsNullOrWhiteSpace(r)) cm.NUBEPersonTitles.Add(new DALNUBEV3.NUBEPersonTitle() { Name = r });
            }
            Console.WriteLine("End_Persion Title");
            dbv3.SaveChanges();
        }

        static void Relation()
        {
            var cm = dbv3.CompanyDetails.FirstOrDefault();
            Console.WriteLine("Start_Relation");
            foreach (var r in dbv2.MASTERRELATIONs.ToList())
            {
                cm.NUBERelations.Add(new DALNUBEV3.NUBERelation() { Name = r.RELATION_NAME });
            }
            Console.WriteLine("End_Relation");
            dbv3.SaveChanges();
        }

        static void BankAndBranch()
        {
            var cm = dbv3.CompanyDetails.FirstOrDefault();
            Console.WriteLine("Start Bank");
            foreach (var b in dbv2.MASTERBANKs)
            {
                DALNUBEV3.NUBEBank b3 = new DALNUBEV3.NUBEBank() { UserCode = b.BANK_USERCODE, Name = b.BANK_NAME, V2Code = b.BANK_CODE.ToString() };
                cm.NUBEBanks.Add(b3);
                foreach (var bb in dbv2.MASTERBANKBRANCHes.Where(x => x.BANK_CODE == b.BANK_CODE).ToList())
                {
                    DALNUBEV3.NUBEBankBranch bb3 = new DALNUBEV3.NUBEBankBranch();
                    bb3.V2Code = bb.BANKBRANCH_CODE.ToString();
                    bb3.Name = bb.BANKBRANCH_NAME;
                    bb3.UserCode = bb.BANKBRANCH_USERCODE;
                    bb3.AddressLine1 = bb.BANKBRANCH_ADDRESS1;
                    bb3.AddressLine2 = bb.BANKBRANCH_ADDRESS2;
                    bb3.AddressLine3 = bb.BANKBRANCH_ADDRESS3;
                    var city = dbv2.MASTERCITies.FirstOrDefault(x => x.CITY_CODE == bb.BANKBRANCH_CITY_CODE);
                    if (city != null)
                    {
                        var state = dbv2.MASTERSTATEs.FirstOrDefault(x => x.STATE_CODE == city.STATE_CODE);
                        bb3.CityName = city.CITY_NAME;
                        bb3.StateName = state.STATE_NAME;
                    }
                    
                    
                    bb3.PostalCode = bb.BANKBRANCH_ZIPCODE;
                    bb3.Phone1= bb.BANKBRANCH_PHONE1;
                    bb3.Phone2 = bb.BANKBRANCH_PHONE2;
                    bb3.EMailId = bb.BANKBRANCH_EMAIL;
                    if (bb.NUBE_BRANCH_CODE != null)
                    {
                        if (bb.NUBE_BRANCH_CODE > 0)
                        {
                            var nb = dbv3.NUBEBranches.FirstOrDefault(x => x.V2Code == bb.NUBE_BRANCH_CODE.ToString());
                            if (nb != null)
                            {
                                bb3.NUBEBranchId = nb.Id;
                            }
                            else
                            {

                            }
                        }
                    }
                    bb3.IsHeadQuarters = bb.HEAD_QUARTERS == 1;
                    bb3.IsDeleted = bb.DELETED == 1;
                    bb3.IsMerged = bb.MERGED == 1;


                    b3.NUBEBankBranches.Add(bb3);
                }
            }
            Console.WriteLine("End Bank");
            dbv3.SaveChanges();
        }
        static void ResignationReason()
        {
            var cm = dbv3.CompanyDetails.FirstOrDefault();
            Console.WriteLine("Start_Reason");
            foreach (var r in dbv2.MASTERRESIGNSTATUS.ToList())
            {
                cm.NUBEResignationReasons.Add(new DALNUBEV3.NUBEResignationReason() {V2Code=r.RESIGNSTATUS_CODE.ToString(), Name = r.RESIGNSTATUS_NAME,UserCode=r.RESIGNSTATUS_SHORTCODE, BenefitAmountPerYear=r.AmtPerYear2,BenefitAmountTilMinimumYear=r.AmtPerYear1, HasBenefit=r.IsBenefitValid, MinimumYear=r.MinimumYear,MinimumRefund=r.MinimumRefund, MaximumRefund=r.MaximumRefund});
            }
            Console.WriteLine("End_Reason");
            dbv3.SaveChanges();
        }
        static void Member()
        {
            //var str = "Enter the Bank Code : ";
            //AppLib.DisplayClear();
            //AppLib.DisplayMsg(1, 1, str);
            //var Bank_Code = Convert.ToDecimal(AppLib.ReadMsg(str.Length + 2, 1));
            //var lst = dbv2.MASTERMEMBERs.Where(x => x.BANK_CODE == Bank_Code).ToList();

            var lst = dbv2.MASTERMEMBERs.ToList();
            AppLib.DisplayMsg(1, 3, string.Format("No of Member : {0}", lst.Count()));
            if (lst.Count() == 0)
            {
                AppLib.DisplayMsg(1, 5, "No member on that bank");
            }
            else
            {
                var i = 1;
                try
                {
                    
                    foreach (var m in lst)
                    {
                        AppLib.DisplayMsg(1, 5, string.Format("Done Member : {0}", i++));
                        var em3 = dbv3.NUBEMemberships.FirstOrDefault(x => x.V2Code == m.MEMBER_CODE.ToString());
                        if (em3 != null) continue;
                        var ag = dbv3.AccountGroups.FirstOrDefault(x => x.GroupName == "Sundry Debtors");
                        DALNUBEV3.Ledger l3 = new DALNUBEV3.Ledger();
                        ag.Ledgers.Add(l3);
                        DALNUBEV3.NUBEMembership m3 = new DALNUBEV3.NUBEMembership();
                        l3.NUBEMemberships.Add(m3);

                        if (!string.IsNullOrWhiteSpace(m.MEMBER_TITLE))
                        {
                            var pt = dbv3.NUBEPersonTitles.FirstOrDefault(x => x.Name == m.MEMBER_TITLE);
                            if (pt != null)
                            {
                                m3.NUBEPersonTitleId = pt.Id;
                            }
                        }
                        m3.V2Code = m.MEMBER_CODE.ToString();

                        l3.PersonIncharge = m.MEMBER_NAME;
                        l3.LedgerName = m.MEMBER_ID.ToString();

                        m3.NRIC_N = m.ICNO_NEW;
                        m3.NRIC_O = m.ICNO_OLD;

                        if (m.BRANCH_CODE != null)
                        {
                            var bb = dbv3.NUBEBankBranches.FirstOrDefault(x => x.V2Code == m.BRANCH_CODE.ToString());
                            if (bb != null)
                            {
                                m3.NUBEBankBranchId = bb.Id;
                            }
                        }

                        l3.AddressLine1 = m.ADDRESS1;
                        l3.AddressLine2 = m.ADDRESS2;
                        l3.AddressLine3 = m.ADDRESS3;

                        var city = dbv2.MASTERCITies.FirstOrDefault(x => x.CITY_CODE == m.CITY_CODE);
                        if (city != null)
                        {
                            var state = dbv2.MASTERSTATEs.FirstOrDefault(x => x.STATE_CODE == city.STATE_CODE);
                            l3.CityName = city.CITY_NAME;
                            l3.StateName = state.STATE_NAME;
                        }

                        l3.PostalCode = m.ZIPCODE;
                        l3.TelephoneNo = m.PHONE;
                        l3.MobileNo = m.MOBILE;
                        l3.EMailId = m.EMAIL;

                        m3.DOB = m.DATEOFBIRTH;
                        m3.DOE = m.DATEOFEMPLOYMENT;
                        m3.DOJ = m.DATEOFJOINING;
                        m3.DOL = m.LASTPAYMENT_DATE;

                        m3.NUBEMemberTypeId = Convert.ToInt32(m.MEMBERTYPE_CODE);
                        m3.NUBEGenderId = string.IsNullOrWhiteSpace(m.SEX) ? (int?)null : m.SEX == "Male" ? 1 : 2;
                        m3.NUBERaceId = Convert.ToInt32(m.RACE_CODE);
                        m3.Rejoin = m.REJOINED == 1;
                        m3.NUBEStatusId = 2;
                        m3.NUBELevyId = string.IsNullOrWhiteSpace(m.LEVY) ? (int?)null : (m.LEVY == "N/A" ? 1 : (m.LEVY == "Yes" ? 2 : 3));
                        m3.NUBETDFId = string.IsNullOrWhiteSpace(m.TDF) ? (int?)null : (m.TDF == "N/A" ? 1 : (m.TDF == "Yes" ? 2 : 3));

                        m3.Salary = Convert.ToDecimal(m.Salary ?? 0);
                        m3.LevyAmount = m.LEVY_AMOUNT ?? 0;
                        m3.TDFAmount = m.TDF_AMOUNT ?? 0;
                        m3.LevyPayDate = m.LevyPaymentDate;
                        m3.TDFPayDate = m.Tdf_PaymentDate;

                        var mg = dbv2.MASTERGUARDIANs.FirstOrDefault(x => x.MEMBER_CODE == m.MEMBER_CODE);
                        if (mg != null)
                        {
                            var mg3 = new DALNUBEV3.NUBEMembershipGuardian();
                            mg3.NRIC_N = mg.ICNO_NEW;
                            mg3.NRIC_O = mg.ICNO_OLD;
                            mg3.Name = mg.NAME;
                            mg3.NUBEGenderId = string.IsNullOrWhiteSpace(mg.SEX) ? (int?)null : mg.SEX == "Male" ? 1 : 2;

                            var r2 = dbv2.MASTERRELATIONs.FirstOrDefault(x => x.RELATION_CODE == mg.RELATION_CODE);
                            if (r2 != null)
                            {
                                var r3 = dbv3.NUBERelations.FirstOrDefault(x => x.Name == r2.RELATION_NAME);
                                if (r3 != null)
                                {
                                    mg3.NUBERelationId = r3.Id;
                                }
                                else
                                {

                                }

                            }
                            mg3.AddressLine1 = mg.ADDRESS1;
                            mg3.AddressLine2 = mg.ADDRESS2;
                            mg3.AddressLine3 = mg.ADDRESS3;

                            city = dbv2.MASTERCITies.FirstOrDefault(x => x.CITY_CODE == mg.CITY_CODE);
                            if (city != null)
                            {
                                var state = dbv2.MASTERSTATEs.FirstOrDefault(x => x.STATE_CODE == city.STATE_CODE);
                                mg3.CityName = city.CITY_NAME;
                                mg3.StateName = state.STATE_NAME;
                            }

                            mg3.PostalCode = mg.ZIPCODE;
                            mg3.PhoneNo = mg.PHONE;
                            mg3.MobileNo = mg.MOBILE;

                            m3.NUBEMembershipGuardians.Add(mg3);
                        }

                        foreach (var mn in dbv2.MASTERNOMINEEs.Where(x => x.MEMBER_CODE == m.MEMBER_CODE).ToList())
                        {
                            var mn3 = new DALNUBEV3.NUBEMembershipNominee();

                            mn3.NRIC_N = mn.ICNO_NEW;
                            mn3.NRIC_O = mn.ICNO_OLD;
                            mn3.Name = mn.NAME;
                            mn3.NUBEGenderId = string.IsNullOrWhiteSpace(mn.SEX) ? (int?)null : mn.SEX == "Male" ? 1 : 2;

                            var r2 = dbv2.MASTERRELATIONs.FirstOrDefault(x => x.RELATION_CODE == mn.RELATION_CODE);
                            if (r2 != null)
                            {
                                var r3 = dbv3.NUBERelations.FirstOrDefault(x => x.Name == r2.RELATION_NAME);
                                if (r3 != null)
                                {
                                    mn3.NUBERelationId = r3.Id;
                                }
                                else
                                {

                                }

                            }
                            mn3.AddressLine1 = mn.ADDRESS1;
                            mn3.AddressLine2 = mn.ADDRESS2;
                            mn3.AddressLine3 = mn.ADDRESS3;


                            city = dbv2.MASTERCITies.FirstOrDefault(x => x.CITY_CODE == mn.CITY_CODE);
                            if (city != null)
                            {
                                var state = dbv2.MASTERSTATEs.FirstOrDefault(x => x.STATE_CODE == city.STATE_CODE);
                                mn3.CityName = city.CITY_NAME;
                                mn3.StateName = state.STATE_NAME;
                            }

                            mn3.PostalCode = mn.ZIPCODE;
                            mn3.PhoneNo = mn.PHONE;
                            mn3.MobileNo = mn.MOBILE;

                            m3.NUBEMembershipNominees.Add(mn3);
                        }

                    }
                    dbv3.SaveChanges();
                }
                catch(Exception ex)
                {

                }
                
            }


        }

        static void MemberResignation()
        {
            var lst = dbv2.RESIGNATIONs.ToList();
            AppLib.DisplayClear();
            AppLib.DisplayMsg(1, 1, "No of Member : " + lst.Count().ToString());

            var i = 1;
            foreach(var r in lst)
            {
                var m = dbv3.NUBEMemberships.FirstOrDefault(x => x.V2Code == r.MEMBER_CODE.ToString());
                var mr = new DALNUBEV3.NUBEMembershipResignation();
                m.NUBEMembershipResignations.Add(mr);
                mr.AccBenefit = r.ACCBENEFIT;
                mr.AccBF = r.ACCBF;
                mr.Amount = r.AMOUNT;
                mr.ChequeDate = r.CHEQUEDATE;
                mr.ChequeNo = r.CHEQUENO;
                mr.ClaimerName = r.CLAIMER_NAME;
                mr.ContributedMonth = Convert.ToInt32(r.MONTHS_CONTRIBUTED);
                mr.Date = r.RESIGNATION_DATE;
                mr.InsuranceAmount = r.InsuranceAmount;
                mr.PayMode = r.PayMode;
                mr.ServiceYear = Convert.ToInt32(r.SERVICE_YEAR);
                mr.TotalArrear = Convert.ToInt32( r.TOTALARREARS);
                mr.UnionContribution = r.UnionContribution;
                mr.VoucherDate = r.VOUCHER_DATE;


                var nrr = dbv3.NUBEResignationReasons.FirstOrDefault(x => x.V2Code == r.REASON_CODE.ToString());
                if (nrr != null)
                {
                    mr.NUBEResignationReasonId = nrr.Id;
                }

                var r2 = dbv2.MASTERRELATIONs.FirstOrDefault(x => x.RELATION_CODE == r.RELATION_CODE);
                if (r2 != null)
                {
                    var r3 = dbv3.NUBERelations.FirstOrDefault(x => x.Name == r2.RELATION_NAME);
                    if (r3 != null)
                    {
                        mr.NUBERelationId = r3.Id;
                    }
                    else
                    {

                    }

                }
                AppLib.DisplayMsg(1, 3, "Done Member : " + i++.ToString());
            }
            dbv3.SaveChanges();

        }
        //static void MemberStatus()
        //{
        //    AppLib.WriteLogFileName = "MemberStatus";

        //    AppLib.WriteLog("Member Status");
        //    AppLib.WriteLog("-------------");
        //    AppLib.WriteLog("");
        //    AppLib.WriteLog("");
        //    var str = "Enter the Bank Code : ";
        //    AppLib.DisplayClear();
        //    AppLib.DisplayMsg(1, 1, str);
        //    var Bank_Id = Convert.ToInt32(AppLib.ReadMsg(str.Length + 2, 1));

        //    var bb = dbv3.Banks.FirstOrDefault(x => x.Id == Bank_Id);
        //    if (bb == null)
        //    {
        //        AppLib.WriteLog("{0} Bank Id is invliad",Bank_Id);
        //    }
        //    else
        //    {
        //        AppLib.WriteLog("{0} Bank Id is vliad. Bank User Code : {1}, Bank Name : {2}", bb.Id,bb.Code,bb.Name);
        //        AppLib.WriteLog("");
        //        AppLib.WriteLog("");
        //    }

        //    var lst = dbv3.Members.Where(x => x.BankBranch.BankId == Bank_Id).ToList();
        //    AppLib.DisplayMsg(1, 3, string.Format("No of Member : {0}", lst.Count()));
        //    AppLib.WriteLog("No of Member : {0}", lst.Count());
        //    AppLib.WriteLog("");
        //    AppLib.WriteLog("");

        //    if (lst.Count() == 0)
        //    {
        //        AppLib.DisplayMsg(1, 5, "No member on that bank");
        //    }
        //    else
        //    {
        //        var i = 0;
        //        foreach (var m in lst)
        //        {
        //            AppLib.DisplayMsg(1, 5, string.Format("Done Member : {0}", i++));
        //            AppLib.WriteLog("SNo: {0},Id: {1}, Code: {2}, Member No: {3}, Member Name: {4}, DOJ: {5:dd/MM/yyyy}",i,m.Id, m.MemberCode, m.MemberNo, m.MemberName,m.DOJ);
        //            AppLib.WriteLog("");
        //            AppLib.WriteLog("");

        //            DateTime dtStart,dtEnd;

        //            if (m.DOJ.Value.Year < 2005)
        //            {
        //                dtStart = new DateTime(2005, 9, 1);
        //            }
        //            else if (m.DOJ.Value.Year == 2005)
        //            {
        //                if (m.DOJ.Value.Month < 9)
        //                {
        //                    dtStart = new DateTime(2005, 9, 1);
        //                }
        //                else
        //                {
        //                    dtStart = new DateTime(2005, m.DOJ.Value.Month, 1);
        //                }
        //            }
        //            else
        //            {
        //                dtStart = new DateTime(m.DOJ.Value.Year, m.DOJ.Value.Month, 1);
        //            }

        //            var mresign = dbv2.RESIGNATIONs.FirstOrDefault(x => x.MEMBER_CODE.ToString() == m.MemberCode);
        //            dtEnd = (mresign == null) ? new DateTime(2019, 2, 1): mresign.CHEQUEDATE.Value;
        //            AppLib.WriteLog("\tHistory start: {0:dd/MM/yyyy}, End : {1:dd/MM/yyyy}", dtStart, dtEnd);
        //            AppLib.WriteLog("");
        //            var dt = dtStart;
        //            var j = 1;
        //            while (dt <= dtEnd)
        //            {
        //                var ms = dbv2.MemberMonthEndStatus.FirstOrDefault(x => x.StatusMonth == dt && x.MEMBER_CODE.ToString() == m.MemberCode);
        //                AppLib.WriteLog("{0:d3} {1:dd/MM/yyyy}", j, dt);



        //                dt = dt.AddMonths(1);
        //                j = j + 1;
        //            }
        //        }
        //    }
        //}

        
        static void MemberFeeGeneration()
        {
            AppLib.DisplayClear();

            try
            {

                var LID_EntranceFee = 79590;
                var LID_Badge = 79591;
                var LID_BuildingFund = 79592;
                var LID_BenevolentFund = 79593;
                var LID_Subscription = 79594;
                var LID_Penalty = 79595;
                var LID_Resignation = 79596;
                var LID_Insurance = 79598;


                

                DateTime OnePerStart = new DateTime(2016, 4, 1);

                var str = "Enter the year : ";
                AppLib.DisplayMsg(1, 1, str);
                var yy = Convert.ToInt32(AppLib.ReadMsg(str.Length, 1));

                var lst = dbv3.NUBEMemberships.Where(x => x.DOJ.Value.Year == yy).ToList();

                AppLib.DisplayMsg(1, 3, "No of Member : " + lst.Count().ToString());

                var i = 1;
                foreach(var m in lst)
                {
                    var dtFrom = m.DOJ.Value.AddMonths(1);
                    dtFrom = new DateTime(dtFrom.Year, dtFrom.Month, 1);
                    var dtTo = new DateTime(2019, 2, 1);

                    #region Member Registration
                    var jm = new DALNUBEV3.Journal();
                    dbv3.Journals.Add(jm);
                    jm.RefCode = $"M{m.Id}";
                    jm.JournalDate = m.DOJ.Value;
                    decimal Subcription = 0;
                    var Insurance = 0;
                    var EntranceFee = m.Rejoin==true ? 0: 5 ;
                    decimal Penalty = 0;
                    var Badge = 5;
                    var BuildingFund = 2;
                    var BenevolentFund = 3;
                    var Subscription_C = 8;         //id=1
                    var Subscription_NC = 4;        //id=2

                    if (m.DOJ < OnePerStart)
                    {
                        Subcription = m.NUBEMemberTypeId == 1 ? Subscription_C : Subscription_NC;
                        Penalty = m.Rejoin == true ? Subcription * 3 : 0;
                    }
                    else
                    {
                        if (m.Salary == null)
                        {
                            Subcription = m.NUBEMemberTypeId == 1 ? Subscription_C : Subscription_NC;
                            Penalty = m.Rejoin == true ? Subcription * 3 : 0;
                        }
                        else
                        {
                            Insurance = 7;
                            Subcription = (m.Salary.Value / 100)-(Insurance+BenevolentFund);
                            Penalty = m.Rejoin == true ? (m.Salary.Value / 100) * 3 : 0;
                        }
                    }

                    jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() {
                        LedgerId = m.LedgerId,
                        DrAmt = EntranceFee + BuildingFund + Badge + Subcription + BenevolentFund + Insurance,                        
                    });

                   
                    jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() { LedgerId = LID_BuildingFund, CrAmt = BuildingFund });
                    jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() { LedgerId = LID_Badge, CrAmt = Badge});
                    jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() { LedgerId = LID_Subscription, CrAmt = Subcription });
                    jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() { LedgerId = LID_BenevolentFund, CrAmt = BenevolentFund });
                    if (EntranceFee > 0) jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() { LedgerId = LID_EntranceFee, CrAmt = EntranceFee });
                    if (Penalty > 0) jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() { LedgerId = LID_Penalty, CrAmt = Penalty });
                    if (Insurance>0) jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() { LedgerId = LID_Insurance, CrAmt = Insurance });
                    #endregion

                    #region Member Resignation
                    var mResign = m.NUBEMembershipResignations.FirstOrDefault();
                    if (mResign != null)
                    {
                        jm = new DALNUBEV3.Journal();
                        dbv3.Journals.Add(jm);
                        jm.JournalDate = mResign.VoucherDate.Value;
                        jm.RefCode = $"R{mResign.Id}";

                        jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() {LedgerId=LID_Resignation,CrAmt = mResign.Amount.Value });
                        jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() { LedgerId = m.LedgerId, DrAmt = mResign.Amount.Value });
                        dtTo = mResign.VoucherDate.Value;
                        dtTo = new DateTime(dtTo.Year, dtTo.Month, 1);
                    }



                    #endregion

                    #region Member Fee Generation                    
                    while (dtFrom < dtTo)
                    {
                        Insurance = 0;
                        if (dtFrom < OnePerStart)
                        {
                            Subcription = m.NUBEMemberTypeId == 1 ? Subscription_C : Subscription_NC;                            
                        }
                        else
                        {
                            if (m.Salary == null)
                            {
                                Subcription = m.NUBEMemberTypeId == 1 ? Subscription_C : Subscription_NC;                                
                            }
                            else
                            {
                                Insurance = 7;
                                Subcription = (m.Salary.Value / 100) - (Insurance + BenevolentFund);                                
                            }
                        }

                        jm = new DALNUBEV3.Journal();
                        dbv3.Journals.Add(jm);
                        jm.JournalDate = dtFrom;                        
                        
                        jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() { LedgerId = m.LedgerId, DrAmt = Subcription+BenevolentFund+Insurance });

                        jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() { LedgerId = LID_Subscription, CrAmt = Subcription });
                        jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() { LedgerId = LID_BenevolentFund, CrAmt = BenevolentFund });
                        if (Insurance>0) jm.JournalDetails.Add(new DALNUBEV3.JournalDetail() { LedgerId = LID_Insurance, CrAmt = Insurance });

                        dtFrom = dtFrom.AddMonths(1);
                    }

                    #endregion

                    AppLib.DisplayMsg(1, 5, "Done Member : " + i++.ToString());
                }
                AppLib.DisplayMsg(1, 7, "Storing... Please wait...");
                dbv3.SaveChanges();
                Console.ReadKey();
            }
            catch(Exception ex)
            {

            }
            
        }

        static void Main(string[] args)
        {
            //WriteCityStateAndCountry();

            //WriteNubeBranch();
            //PersonTitle();
            //Relation();
            //BankAndBranch();
            //Member();

            //ResignationReason();
            //MemberResignation();

            MemberFeeGeneration();
        }
    }
}
