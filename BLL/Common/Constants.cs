using BusinessLogicLayer.Extensions;
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
    }
}
