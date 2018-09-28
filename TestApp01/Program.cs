using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using Common;
namespace TestApp01
{
    class Program
    {
        static string ServerName = "ntc.my";
        static string DBName_BFS = "nubev2";
        static string DBName_Status = "nubestatus";
        static string UserId = "nubesw";
        static string Password = "BizSoft2017+";        
        static string MenuSelection = "";

        static SqlConnection conBFS = new SqlConnection();
        static SqlConnection ConStatus = new SqlConnection();

        static void Main(string[] args)
        {
            if (args.Length == 5)
            {
                ServerName = args[0];
                UserId = args[1];
                Password = args[2];
                DBName_BFS = args[3];
                DBName_Status = args[4];
            }
            DBConnect();  
            fnSelect();
            DBDisconnect();            
        }
        static void ChangeServer()
        {
            try
            {
                AppLib.DisplayClear();
                AppLib.WriteLog("ChangeServer Start");


                AppLib.DisplayMsg(1, 1, "Server Name          : ");
                AppLib.DisplayMsg(1, 3, "User Id              : ");
                AppLib.DisplayMsg(1, 5, "Password             : ");
                AppLib.DisplayMsg(1, 7, "Database Name BFS    : ");
                AppLib.DisplayMsg(1, 9, "Database Name Status : ");

                ServerName = AppLib.ReadMsg(24, 1);
                UserId = AppLib.ReadMsg(24, 3);
                Password = AppLib.ReadMsg(24, 5);
                DBName_BFS = AppLib.ReadMsg(24, 7);
                DBName_Status = AppLib.ReadMsg(24, 9);
                AppLib.WriteLog("ChangeServer End");
                DBConnect();
            }
            catch(Exception ex) { AppLib.WriteLog(ex); }
        }
        static void DBConnect()
        {
            try
            {

                AppLib.WriteLog("DBConnect Start");

                string ConStrBFS = string.Format("data source={0};initial catalog={1};integrated security=false;user id={2};password={3}", ServerName, DBName_BFS, UserId, Password);
                string ConStrStatus = string.Format("data source={0};initial catalog={1};integrated security=false;user id={2};password={3}", ServerName, DBName_Status, UserId, Password);

                AppLib.WriteLog(string.Format("ConStrBFS: {0}", ConStrBFS));
                AppLib.WriteLog(string.Format("ConStrStatus: {0}", ConStrStatus));
                conBFS = new SqlConnection(ConStrBFS);
                ConStatus = new SqlConnection(ConStrStatus);

                conBFS.Open();
                ConStatus.Open();


                AppLib.WriteLog("DBConnect End");
            }catch(Exception ex) { AppLib.WriteLog(ex); MenuSelection = "x"; }

        }
        static void DBDisconnect()
        {
            try
            {
                AppLib.WriteLog("DBDisconnect Start");
                conBFS.Close();
                ConStatus.Close();
                AppLib.WriteLog("DBDisconnect End");
            }
            catch(Exception ex) { AppLib.WriteLog(ex); }
        }
        static void fnSelect()
        {
            try
            {
                
                AppLib.WriteLog("fnSelect Start");
                
                
                do
                {
                    int Left = 1;
                    int Top = 1;
                    AppLib.DisplayClear();
                    AppLib.DisplayMsg(Left, Top++, string.Format(" 1. Server Change [ Connected=>{0}]",ServerName));
                    AppLib.DisplayMsg(Left, Top++, " 2. Month End Count");
                    AppLib.DisplayMsg(Left, Top++, " 3. Scan by Member No");
                    AppLib.DisplayMsg(Left, Top++, " 4. Scan by Member Range");
                    AppLib.DisplayMsg(Left, Top++, " 5. Merge Member Status");
                    AppLib.DisplayMsg(Left, Top, " Select [1-5] or x=> Exit : ");

                    MenuSelection = AppLib.ReadMsg(Left + 28, Top);

                    switch (MenuSelection)
                    {
                        case "1":
                            ChangeServer();
                            break;
                        case "2" :
                            MonthEndCount();
                            break;
                        case "3":
                            MemberScan();
                            break;
                        case "4":
                            MemberRangeScan();
                            break;
                        case "5":
                            MergeMemberStatus();
                            break;
                        default:

                            break;
                    }

                } while (MenuSelection != "x");
                AppLib.WriteLog("fnSelect End");
            }catch(Exception ex) { AppLib.WriteLog(ex); }
        }
        static void MonthEndCount()
        {
            try
            {
                AppLib.WriteLog("MonthEndCount Start");
                AppLib.DisplayClear();
                long total = 0; 
                for(int yy = 2005; yy <= 2018; yy++)
                {
                    for(int mm = (yy == 2005 ? 9 : 1); mm <= (yy == 2018 ? 7 : 12); mm++)
                    {
                        string TBName = string.Format("Status{0:d2}{1}", mm, yy);
                        AppLib.DisplayMsg(1, 1, TBName);
                        string Qry = string.Format("Select count(*) from {0}", TBName);
                        SqlCommand cmd = new SqlCommand(Qry, ConStatus);
                        var data = cmd.ExecuteScalar();
                        AppLib.WriteLog(string.Format("{0} => {1}", TBName, data));
                        total += long.Parse(data.ToString());
                    }
                }
                AppLib.WriteLog(string.Format("Total = {0}", total));
                AppLib.WriteLog("MonthEndCount End");
            }
            catch(Exception ex) { AppLib.WriteLog(ex); }
        }
        static void MergeMemberStatus()
        {
            try
            {
                AppLib.WriteLog("MergeMemberStatus Start");
                AppLib.DisplayClear();
                long total = 0;
                for (int yy = 2005; yy <= 2018; yy++)
                {
                    for (int mm = (yy == 2005 ? 9 : 1); mm <= (yy == 2018 ? 7 : 12); mm++)
                    {
                        string TBName = string.Format("Status{0:d2}{1}", mm, yy);
                        AppLib.DisplayMsg(1, 1, TBName);
                        string Qry = string.Format("insert into {0}..MemberMonthEndStatus select '{3}-{4:d2}-01' ,* from {1}..{2}", DBName_BFS,DBName_Status ,TBName,yy,mm);
                        SqlCommand cmd = new SqlCommand(Qry, ConStatus);
                        var data = cmd.ExecuteNonQuery();
                        AppLib.WriteLog(string.Format("{0} => {1}", TBName, data));
                        total += long.Parse(data.ToString());
                    }
                }
                AppLib.WriteLog(string.Format("Total = {0}", total));
                AppLib.WriteLog("MergeMemberStatus End");
            }
            catch (Exception ex) { AppLib.WriteLog(ex); }
        }
        static void MemberScan()
        {
            try
            {
                AppLib.WriteLog("MemberScan Start");
                AppLib.DisplayClear();
                AppLib.DisplayMsg(1, 1, "Member No : ");
                string MemberNo = AppLib.ReadMsg(14, 1);
                MemberScanByNo(MemberNo);               
                AppLib.WriteLog("MemberScan End");
            }
            catch(Exception ex) { AppLib.WriteLog(ex); }
        }
        static void MemberScanByNo(string MemberNo)
        {
            try
            {
                AppLib.WriteLog(string.Format("Start MemberScanByNo : {0}", MemberNo));
                AppLib.DisplayClear();
                String Qry = string.Format("Select Member_Code,DateOfJoining from MasterMember where Member_Id = {0}", MemberNo);
                SqlCommand cmd = new SqlCommand(Qry, conBFS);
                var drMember = cmd.ExecuteReader();
                if(!drMember.Read())
                {
                    AppLib.WriteLog(string.Format("Member No {0} is Not Found", MemberNo));
                    drMember.Close();
                    return;
                }
                string MemberCode = drMember["Member_Code"].ToString();
                DateTime DOJ = (DateTime) drMember["DateOfJoining"];
                drMember.Close();
                AppLib.WriteLogDT("Member Code : {0}, DOJ : {1:dd/MM/yyyy}", MemberCode, DOJ);

                
                int yyStart = DOJ.Year <= 2005 ? 2005 : DOJ.Year;                
                int mmStart = DOJ.Year < 2005 || (DOJ.Year == 2005 && DOJ.Month <= 9) ? 9 : DOJ.Month;

                int yyEnd = 2018;
                int mmEnd = 7;


                int MMType=0, BankCode, BankBranchCode=0, NubeBranchCode=0,PaidMonth,TotalPaidMonth,TotalDueMonth, tmpTotalPaidMonth, tmpTotalDueMonth, StatusCode=0,Resigned=0,Cancelled=0,StructOff=0;
                decimal SubAmt,BFAmt,SubPaid,BFPaid, SubDue, BFDue, SubTotPaid, BFTotPaid, tmpSubDue,tmpBFDue,tmpSubTotPaid,tmpBFTotPaid,AcBenefit;
                DateTime LastPaidDate;


                for (int yy = yyStart; yy <= yyEnd; yy++)
                {
                    for (int mon = (yy == yyStart ? mmStart : 1); mon <= (yy == yyEnd ? mmEnd : 12); mon++)
                    {
                        string TBName = string.Format("Status{0:d2}{1}", mon, yy);
                        AppLib.DisplayMsg(1, 1, string.Format("Member No : {0}",MemberNo));
                        AppLib.DisplayMsg(1, 3, string.Format("Member Code : {0}", MemberCode));
                        AppLib.DisplayMsg(1, 5, string.Format("DOJ : {0:dd/MM/yyyy}", DOJ));
                        AppLib.DisplayMsg(1, 7, string.Format("Year : {0}", yy));
                        AppLib.DisplayMsg(1, 9, string.Format("Month : {0:d2}", mon));
                        Qry = string.Format("Select * from {0} where Member_Code = {1}", TBName, MemberCode);
                        cmd = new SqlCommand(Qry, ConStatus);
                        var dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            if(MMType != int.Parse(dr["MemberType_Code"].ToString()))
                            {
                                if (yy != yyStart || mon != mmStart) MemberType_Changed(MemberCode,MMType, int.Parse(dr["MemberType_Code"].ToString()),yy,mon);

                                MMType = int.Parse(dr["MemberType_Code"].ToString());
                            }
                            BankCode = int.Parse(dr["Bank_Code"].ToString());

                            if(BankBranchCode != int.Parse(dr["Branch_Code"].ToString()))
                            {
                                if (yy != yyStart || mon != mmStart) BankBranch_Changed(MemberCode, BankBranchCode, int.Parse(dr["Branch_Code"].ToString()), yy, mon);
                                BankBranchCode = int.Parse(dr["Branch_Code"].ToString());
                            }
                            
                            if( NubeBranchCode != int.Parse(dr["NUBE_BRANCH_CODE"].ToString()))
                            {
                                if (yy != yyStart || mon != mmStart) NubeBranch_Changed(MemberCode, NubeBranchCode, int.Parse(dr["NUBE_BRANCH_CODE"].ToString()), yy, mon);
                                NubeBranchCode = int.Parse(dr["NUBE_BRANCH_CODE"].ToString());
                            }


                            if (StatusCode != int.Parse(dr["STATUS_CODE"].ToString()))
                            {
                                if (yy != yyStart || mon != mmStart) Status_Changed(MemberCode, StatusCode, int.Parse(dr["STATUS_CODE"].ToString()), yy, mon);
                                StatusCode = int.Parse(dr["STATUS_CODE"].ToString());
                            }

                            if (Resigned != int.Parse(dr["RESIGNED"].ToString()))
                            {
                                if (yy != yyStart || mon != mmStart) Resigned_Changed(MemberCode, Resigned, int.Parse(dr["RESIGNED"].ToString()), yy, mon);
                                Resigned = int.Parse(dr["RESIGNED"].ToString());
                            }

                            if (Cancelled != int.Parse(dr["CANCELLED"].ToString()))
                            {
                                if (yy != yyStart || mon != mmStart) Cancelled_Changed(MemberCode, Cancelled, int.Parse(dr["CANCELLED"].ToString()), yy, mon);
                                Cancelled = int.Parse(dr["CANCELLED"].ToString());
                            }

                            if (StructOff != int.Parse(dr["STRUCKOFF"].ToString()))
                            {
                                if (yy != yyStart || mon != mmStart) StructOff_Changed(MemberCode, StructOff, int.Parse(dr["STRUCKOFF"].ToString()), yy, mon);
                                StructOff = int.Parse(dr["STRUCKOFF"].ToString());
                            }
                            
                            
                            PaidMonth = int.Parse(dr["TOTAL_MONTHS"].ToString());
                            TotalPaidMonth = int.Parse(dr["TOTALMONTHSPAID"].ToString());
                            TotalDueMonth = int.Parse(dr["TOTALMONTHSDUE"].ToString());

                            SubAmt = decimal.Parse(dr["SUBSCRIPTION_AMOUNT"].ToString());
                            BFAmt = decimal.Parse(dr["BF_AMOUNT"].ToString());
                            SubPaid = decimal.Parse(dr["TOTALSUBCRP_AMOUNT"].ToString());
                            BFPaid = decimal.Parse(dr["TOTALBF_AMOUNT"].ToString());
                            SubDue = decimal.Parse(dr["SUBSCRIPTIONDUE"].ToString());
                            BFDue = decimal.Parse(dr["BFDUE"].ToString());
                            SubTotPaid = decimal.Parse(dr["ACCSUBSCRIPTION"].ToString());
                            BFTotPaid = decimal.Parse(dr["ACCBF"].ToString());
                            AcBenefit = decimal.Parse(dr["ACCBENEFIT"].ToString());

                            LastPaidDate = (DateTime)dr["LASTPAYMENTDATE"];

                            if(yy==yyStart && mon == mmStart)
                            {
                                tmpTotalPaidMonth = TotalPaidMonth;
                                tmpTotalDueMonth = TotalDueMonth;
                                tmpBFDue = BFDue;
                                tmpBFTotPaid = BFTotPaid;
                                tmpSubDue = SubDue;
                                tmpSubTotPaid = SubTotPaid;                                

                                if(yy==DOJ.Year && mon == DOJ.Month)
                                {
                                    if (tmpTotalPaidMonth != 1)
                                    {
                                    
                                    }
                                }
                                else
                                {

                                }
                            }
                            else
                            {

                            }

                            AppLib.WriteLogDT("Month: {0}-{1:d2}, MMType: {2}, Bank: {3:d4}, BankBranch: {4:d4}, NubeBranch: {5:d2}, PaidMonth: {6:d2}, "
                                    + "TotalPaidMonth: {7:d3}, TotalDueMonth: {8:d3}, Status: {9}, Resigned: {10}, Cancelled: {11}, StructOff: {12}, "
                                    + "SubAmt: {13:N2}, BFAmt: {14:N2}, SubPaid: {15:N2}, BFPaid: {16:N2}, SubDue: {17:N2}, BFDue: {18:N2}, "
                                    + "SubTotPaid: {19:N2}, BFTotPaid: {20:N2}, AcBenefit: {21:N2}, LastPaidDate: {22:dd/MM/yyyy}"
                                    , yy,mon, MMType,BankCode,BankBranchCode,NubeBranchCode,PaidMonth
                                    , TotalPaidMonth,TotalDueMonth,StatusCode,Resigned,Cancelled,StructOff
                                    , SubAmt, BFAmt, SubPaid, BFPaid, SubDue,BFDue,SubTotPaid,BFTotPaid,AcBenefit,LastPaidDate);
                            
                        }
                        dr.Close();                   
                    }
                }
                AppLib.WriteLog(string.Format("End MemberScanByNo : {0}", MemberNo));
            }
            catch (Exception ex) { AppLib.WriteLog(ex); }            
        }
        static void MemberRangeScan()
        {
            try
            {
                AppLib.WriteLog("Start MemberRangeScan");


                AppLib.DisplayClear();
                AppLib.DisplayMsg(1, 1, "Starting Member No : ");
                AppLib.DisplayMsg(1, 3, "Number of Member   : ");
                string MemberNo = AppLib.ReadMsg(25, 1);
                string Num = AppLib.ReadMsg(25, 3);

                List<string> lstMemberId = new List<string>();
                AppLib.WriteLogDT("Starting Member No: {0}, Number Of Member: {1}", MemberNo, Num);

                string Qry = string.Format("select top {0} Member_Id from MASTERMEMBER where MEMBER_ID >= {1} order by status_code, MEMBER_ID", Num,MemberNo);
                SqlCommand cmd = new SqlCommand(Qry, conBFS);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lstMemberId.Add(dr["Member_Id"].ToString());
                }
                dr.Close();
                AppLib.WriteLogDT("List of Members No : {0}", string.Join(", ", lstMemberId));

                foreach(var MNo in lstMemberId)
                {
                    MemberScanByNo(MNo);
                }

                AppLib.WriteLog("End MemberRangeScan");
            }
            catch(Exception ex) { AppLib.WriteLog(ex); }
        }
        
        #region Events
        static void MemberMonthEndErrorLog(string MemberCode,int Year, int Mon,string ErrorMsg)
        {
            try
            {
                AppLib.WriteLogDT("MemberMonthEndErrorLog => Member_Code: {0}, Year: {1}, Mon: {2}, ErrorMsg: {3]", MemberCode, Year, Mon,ErrorMsg);
                string Qry = string.Format("insert into MemberMonthEndErrorLog values({0},{1},{2},'{3}')", MemberCode, Year, Mon, ErrorMsg);
                SqlCommand cmd = new SqlCommand(Qry, conBFS);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { AppLib.WriteLog(ex); }
        }
        static void MemberType_Changed(string MemberCode, int OldMemberType,int NewMemberType,int Year,int Mon)
        {
            
            try
            {
                AppLib.WriteLogDT("MemberType_Changed => Member_Code: {0}, Old: {0}, New : {1}, Year: {2}, Mon: {3}", MemberCode, OldMemberType, NewMemberType, Year, Mon);
                string Qry = string.Format("insert into MemberDataChanged values({0},{1},{2},{3},{4},'MemberType_Changed')", MemberCode,Year,Mon,OldMemberType,NewMemberType);
                SqlCommand cmd = new SqlCommand(Qry, conBFS);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex) { AppLib.WriteLog(ex); }
        }

        static void BankBranch_Changed(string MemberCode, int OldValue, int NewValue, int Year, int Mon)
        {

            try
            {
                AppLib.WriteLogDT("BankBranch_Changed => Member_Code: {0}, Old: {0}, New : {1}, Year: {2}, Mon: {3}", MemberCode, OldValue, NewValue, Year, Mon);
                string Qry = string.Format("insert into MemberDataChanged values({0},{1},{2},{3},{4},'BankBranch_Changed')", MemberCode, Year, Mon, OldValue, NewValue);
                SqlCommand cmd = new SqlCommand(Qry, conBFS);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { AppLib.WriteLog(ex); }
        }

        static void NubeBranch_Changed(string MemberCode, int OldValue, int NewValue, int Year, int Mon)
        {

            try
            {
                AppLib.WriteLogDT("NubeBranch_Changed => Member_Code: {0}, Old: {0}, New : {1}, Year: {2}, Mon: {3}", MemberCode, OldValue, NewValue, Year, Mon);
                string Qry = string.Format("insert into MemberDataChanged values({0},{1},{2},{3},{4},'NubeBranch_Changed')", MemberCode, Year, Mon, OldValue, NewValue);
                SqlCommand cmd = new SqlCommand(Qry, conBFS);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { AppLib.WriteLog(ex); }
        }


        static void Status_Changed(string MemberCode, int OldValue, int NewValue, int Year, int Mon)
        {

            try
            {
                AppLib.WriteLogDT("Status_Changed => Member_Code: {0}, Old: {0}, New : {1}, Year: {2}, Mon: {3}", MemberCode, OldValue, NewValue, Year, Mon);
                string Qry = string.Format("insert into MemberDataChanged values({0},{1},{2},{3},{4},'Status_Changed')", MemberCode, Year, Mon, OldValue, NewValue);
                SqlCommand cmd = new SqlCommand(Qry, conBFS);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { AppLib.WriteLog(ex); }
        }

        static void Resigned_Changed(string MemberCode, int OldValue, int NewValue, int Year, int Mon)
        {

            try
            {
                AppLib.WriteLogDT("Resigned_Changed => Member_Code: {0}, Old: {0}, New : {1}, Year: {2}, Mon: {3}", MemberCode, OldValue, NewValue, Year, Mon);
                string Qry = string.Format("insert into MemberDataChanged values({0},{1},{2},{3},{4},'Resigned_Changed')", MemberCode, Year, Mon, OldValue, NewValue);
                SqlCommand cmd = new SqlCommand(Qry, conBFS);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { AppLib.WriteLog(ex); }
        }

        static void Cancelled_Changed(string MemberCode, int OldValue, int NewValue, int Year, int Mon)
        {

            try
            {
                AppLib.WriteLogDT("Cancelled_Changed => Member_Code: {0}, Old: {0}, New : {1}, Year: {2}, Mon: {3}", MemberCode, OldValue, NewValue, Year, Mon);
                string Qry = string.Format("insert into MemberDataChanged values({0},{1},{2},{3},{4},'Cancelled_Changed')", MemberCode, Year, Mon, OldValue, NewValue);
                SqlCommand cmd = new SqlCommand(Qry, conBFS);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { AppLib.WriteLog(ex); }
        }

        static void StructOff_Changed(string MemberCode, int OldValue, int NewValue, int Year, int Mon)
        {

            try
            {
                AppLib.WriteLogDT("StructOff_Changed => Member_Code: {0}, Old: {0}, New : {1}, Year: {2}, Mon: {3}", MemberCode, OldValue, NewValue, Year, Mon);
                string Qry = string.Format("insert into MemberDataChanged values({0},{1},{2},{3},{4},'StructOff_Changed')", MemberCode, Year, Mon, OldValue, NewValue);
                SqlCommand cmd = new SqlCommand(Qry, conBFS);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { AppLib.WriteLog(ex); }
        }

        #endregion
    }
}
