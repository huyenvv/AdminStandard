﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Standard
{
    public class Constant
    {
        public const string SESSION_TicketDetails = "SESSION_TicketDetails";
        public const string SESSION_CheckoutDetails = "SESSION_CheckoutDetails";

        public const string SESSION_MessageSuccess = "SESSION_MessageSuccess";
        public const string SESSION_MessageError = "SESSION_MessageError";

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
        public const string Statistic = "Statistic";
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
        public const int Reject = 5;

        public static string[] TicketStatusTitle = new string[] { "Khởi tạo", "Chờ thông qua", "Chờ kiểm duyệt", "Chờ duyệt", "Đã duyệt", "Từ chối duyệt" };
    }
    public class CheckoutStatus
    {
        public const int KhoiTao = 0;
        public const int ChoKiemTra = 1;
        public const int ChoDuyet = 2;
        public const int DaDuyet = 3;
        public const int HoanThanh = 4;

        public static string[] CheckoutStatusTitle = new string[] { "Khởi tạo", "Chờ kiểm tra", "Chờ duyệt", "Đã duyệt", "Hoàn thành" };
    }

    public class PaymentMethod
    {
        public const int TienMat = 0;
        public const int ChuyenKhoan = 1;
        public const int QuyetToanTamUng = 2;
    }

    public class DoKhan
    {
        public const int BinhThuong = 0;
        public const int Khan = 1;
        public const int RatKhan = 2;

        public static string[] Title = new string[] { "Bình thường", "Khẩn", "Rất khẩn" };
    }
}