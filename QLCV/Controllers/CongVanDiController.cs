using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace QLCV.Controllers
{
    public class CongVanDiController : BaseController
    {
        // GET: CongVanDi
        [HasCredential(RoleID = "VIEW_CONGVANGUI")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new CongVanDiDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HasCredential(RoleID = "VIEW_CONGVANGUI")]
        public ActionResult Detail(long id)
        {
            var model = new CongVanDiDao().GetCongVanDi(id);     
            var images = model.EmailSend;

            XElement xImages = XElement.Parse(images);
            List<string> listImagesReturn = new List<string>();

            foreach (XElement element in xImages.Elements())
            {
                if (element == null)
                {

                }
                else
                {
                    listImagesReturn.Add(element.Value);
                }

            }
            ViewBag.Count=listImagesReturn.Count();
            ViewBag.xuly = listImagesReturn;
            return View(model);
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_CONGVANGUI")]
        public ActionResult Delete(long id)
        {
            var result = new CongVanDiDao().DeleteCongVanDi(id);
            if (result > 0)
            {
                SetAlert("Thành Công ", "success");
                return RedirectToAction("Index", "CongVanDi");
            }
            else
            {
                ModelState.AddModelError("", "Cập Nhật Không Thành Công");
            }
            return View("Index");

        }
    }
}