using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Standard
{
    public class Constant
    {
    }

    public static class RoleList
    {
        public const string SystemManager = "SystemManager";
        public const string ApproveTicket = "ApproveTicket";
        public const string Normal = "Normal";
    }

    public static class SESSION
    {
        public const string TicketDetail = "TicketDetail";
    }

    public enum Status
    {
        KhoiTao = 1,
        ChoThongQua = 2,
        ChoDuyet = 4,
        DaDuyet = 5
    }
}