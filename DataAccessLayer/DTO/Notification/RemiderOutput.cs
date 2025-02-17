﻿namespace DataAccessLayer.DTO.Notification
{
    public class RemiderOutput
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public int? TypeID { get; set; }
        public string Notes { get; set; }
        public string ApprovalProcessID { get; set; }
        public int ApprovalStatusID { get; set; }
        public string ApprovalStatus { get; set; }
        public int? StatusID { get; set; }
        public int PK { get; set; }
        public string StatusDesc { get; set; }
        public int AllowAccept { get; set; }
        public int AllowReject { get; set; }
        public int AllowDelete { get; set; }
        public int AllowEdit { get; set; }
        public int? NextApprovalID { get; set; }
        public int? PrivillgeType { get; set; }
        public int EmployeeID { get; set; }
    }
}
