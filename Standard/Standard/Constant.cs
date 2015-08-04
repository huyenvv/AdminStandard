using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Standard
{
    public class Constant
    {
        public const string SESSION_TicketDetails = "SESSION_TicketDetails";
        public const string SESSION_CheckoutDetails = "SESSION_CheckoutDetails";

    }
    public static class UploadFolder
    {
        public const string Common = "Common";
        public const string Ticket = "Ticket";
        public const string Checkout = "Checkout";
    }
    public static class RoleList
    {
        public const string SystemManager = "SystemManager";
        public const string ApproveTicket = "ApproveTicket";
        public const string Normal = "Normal";
        public const string Accounting = "Accounting";
    }
    public class DeptType
    {
        public const int None = 0;
        public const int KiemTra = 1;
        public const int KiemSoatNB = 2;

        public static string[] Title = new string[] { "-", "Kiểm tra", "Kiểm soát NB" };
    }
    public class TicketStatus
    {
        public const int KhoiTao = 0;
        public const int ChoThongQua = 1;
        public const int ChoKiemDuyet = 2;
        public const int ChoDuyet = 3;
        public const int DaDuyet = 4;

        public static string[] TicketStatusTitle = new string[] { "Khởi tạo", "Chờ thông qua", "Chờ kiểm duyệt", "Chờ duyệt", "Đã duyệt" };
    }
    public class CheckoutStatus
    {
        public const int KhoiTao = 0;
        public const int ChoKiemSoat = 1;
        public const int ChoDuyet = 2;
        public const int DaDuyet = 3;

        public static string[] CheckoutStatusTitle = new string[] { "Khởi tạo", "Chờ kiểm soát NB", "Chờ duyệt", "Đã duyệt" };
    }

    public class PaymentMethod
    {
        public const int TienMat = 0;
        public const int ChuyenKhoan = 1;
        public const int QuyetToanTamUng = 2;
    }
}