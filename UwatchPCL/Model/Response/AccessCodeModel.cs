using System;
using Newtonsoft.Json;
using UwatchPCL.Model;

namespace UwatchPCL
{
    public class AccessCodeModel:BaseModel
    {
        [JsonProperty("RoleIdList")]
        public object RoleIdList { get; set; }

        [JsonProperty("UserAccessCodeID")]
        public int UserAccessCodeId { get; set; }

        [JsonProperty("RoleID")]
        public int RoleId { get; set; }

        [JsonProperty("AccessCode")]
        public string AccessCode { get; set; }

        public string model_no { get; set; }

        public string serial_no { get; set; }

        [JsonProperty("UsedDate")]
        public object UsedDate { get; set; }

        [JsonProperty("UsedBy")]
        public long UsedBy { get; set; }

        [JsonProperty("SoldDate")]
        public object SoldDate { get; set; }

        [JsonProperty("SoldBy")]
        public object SoldBy { get; set; }

        [JsonProperty("OrderID")]
        public object OrderId { get; set; }

        [JsonProperty("CustomerID")]
        public object CustomerId { get; set; }

        [JsonProperty("Email")]
        public object Email { get; set; }

        [JsonProperty("SoldOnly")]
        public bool SoldOnly { get; set; }

        [JsonProperty("IsPrinted")]
        public bool IsPrinted { get; set; }

        [JsonProperty("CreatedDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonProperty("CreatedBy")]
        public long CreatedBy { get; set; }

        [JsonProperty("ModifyDate")]
        public object ModifyDate { get; set; }

        [JsonProperty("ModifyBy")]
        public object ModifyBy { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty("ErrorCode")]
        public long ErrorCode { get; set; }

        [JsonProperty("ErrorMessage")]
        public object ErrorMessage { get; set; }

        [JsonProperty("PageIndex")]
        public long PageIndex { get; set; }

        [JsonProperty("TotalPage")]
        public long TotalPage { get; set; }

        [JsonProperty("TotalRecords")]
        public long TotalRecords { get; set; }

        [JsonProperty("RecordPerPage")]
        public object RecordPerPage { get; set; }

        [JsonProperty("SearchText")]
        public object SearchText { get; set; }

        [JsonProperty("orderColumn")]
        public object OrderColumn { get; set; }

        [JsonProperty("order")]
        public object Order { get; set; }

      
    }
}
