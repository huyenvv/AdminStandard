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

    public static class RoleList
    {
        public const string SystemManager = "SystemManager";
        public const string ApproveTicket = "ApproveTicket";
        public const string Normal = "Normal";
    }

    public class TicketStatus
    {
        public const int KhoiTao = 0;
        public const int ChoThongQua = 1;
        public const int ChoDuyet = 2;
        public const int DaDuyet = 3;

        public static string[] TicketStatusTitle = new string[] { "Khởi tạo", "Chờ thông qua", "Chờ duyệt", "Đã duyệt" };
    }
    public class CheckoutStatus
    {
        public const int KhoiTao = 0;
        public const int ChoKiemSoat = 1;
        public const int ChoDuyet = 2;
        public const int DaDuyet = 3;

        public static string[] CheckoutStatusTitle = new string[] { "Khởi tạo", "Chờ kiểm soát NB", "Chờ duyệt", "Đã duyệt" };
    }
}