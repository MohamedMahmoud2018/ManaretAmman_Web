using BusinessLogicLayer.Extensions;
using System.Runtime.CompilerServices;

namespace BusinessLogicLayer.Common
{
    public static class Constants
    {
        #region EmployeeLeaves
        public const string EmployeeLeaves = "EmployeeLeaves";
        public const string LeaveTypeID    = "LeaveTypeID";
        #endregion

        #region EmployeeVacations
        public const string VacationType = "VacationType";
        public const string VacationTypeId = "VacationTypeId";
        #endregion


        #region TimingMethode
        public static int? ConvertFromDateFormat(int indecator,DateTime? dateValue=null,string timeValue="")
        {
            return indecator == 1 ? dateValue.ConvertFromDateTimeToUnixTimestamp() : timeValue.ConvertFromTimeStringToMinutes();
        }
        #endregion

        #region Dictionaries
         static readonly Dictionary<int, DictionarData> EmployeeLoanDictionary = new Dictionary<int, DictionarData> 
        {
            { 0,new DictionarData{ NameEn="NonSchedule",NameAr="غير مجدولة"} },
            { 1,new DictionarData{ NameEn="Schedule",NameAr="مجدولة"} }
        };

        public static Dictionary<int, DictionarData> GetEmployeeLoanDictionary => EmployeeLoanDictionary;
        #endregion


        #region Enums

        #endregion
    }
    public class DictionarData
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
