using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLCV.Controllers
{
    public class FileCongVanController : BaseController
    {
        // GET: FileCongVan
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new FileDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }


        [HttpDelete]
        public ActionResult Delete(long id, string user)
        {
            var result = new FileDao().Delete(id, user);
            if (result)
            {
                SetAlert("Thành Công ", "success");
                return RedirectToAction("Index", "FileCongVan");
            }
            else
            {
                ModelState.AddModelError("", "Cập Nhật Không Thành Công");
            }
            return View("Index");

        }
    }
}