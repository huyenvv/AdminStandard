﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Standard.Controllers
{
    public class BaseController : Controller
    {
        protected DB_9CF750_dbEntities db = DB.Entites;
	}
}