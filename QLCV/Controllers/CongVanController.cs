using Common;
using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace QLCV.Controllers
{
    public class CongVanController : BaseController
    {
        // GET: CongVan
        [HasCredential(RoleID = "VIEW_CONGVAN")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            
            var dao = new CongVanDao();
            var model = dao.ListAllPaging(searchString,page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }


        [HttpGet]
        [HasCredential(RoleID = "ADD_CONGVAN")]
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]        
        public ActionResult Create(CongVanDen congvanden)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var id = true;
            if (ModelState.IsValid)
            {
                var dao = new CongVanDao();
                congvanden.ModifiedBy = session.UserName;
                congvanden.CreatedBy = session.UserName;
                congvanden.EmailSend = "<Images></Images>";
                congvanden.CreatedDate = DateTime.Now;
                congvanden.ModifiedDate = DateTime.Now;
                id = dao.InsertUpdateCongVanDen(congvanden);
                if (id)
                {
                    SetAlert("Thêm Thành Công ", "success");
                    return RedirectToAction("Index", "CongVan");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm Sản Phẩm Không Thành Công");
                }
            }
            return View(congvanden);
        }

        [HasCredential(RoleID = "EDIT_CONGVAN")]
        public ActionResult Edit(long id)
        {
            var list = new CongVanDao().GetCategoryByID(id);
            return View(list);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CongVanDen congvanden)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            if (ModelState.IsValid)
            {
                var dao = new CongVanDao();
                congvanden.ModifiedBy = session.UserName;
                congvanden.EmailSend = "<Images></Images>";                
                congvanden.ModifiedDate = DateTime.Now;
                bool result = dao.InsertUpdateCongVanDen(congvanden);
                if (result)
                {
                    SetAlert("Sửa Thành Công ", "success");
                    return RedirectToAction("Index", "CongVan");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật Sản Phẩm Không thành công");
                }
            }
            return View(congvanden);
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_CONGVAN")]
        public ActionResult Delete(long id)
        {
            var result = new CongVanDao().DeleteCongVanDen(id);
            if (result > 0)
            {
                SetAlert("Thành Công ", "success");
                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("", "Cập Nhật Không Thành Công");
            }
            return View("Index");

        }
        [HasCredential(RoleID = "FORWARD_CONGVAN")]
        public ActionResult ChuyenTiep(long id)
        {
            var list = new CongVanDao().GetCategoryByID(id);
            return View(list);
        }
        public JsonResult ListName(string q)
        {
            var data = new CongVanDiDao().ListEmail(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveImages(long id, string images)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var listImages = serializer.Deserialize<List<string>>(images);

            XElement xElement = new XElement("Images");

            foreach (var item in listImages)
            {

                xElement.Add(new XElement("Image", item));
            }
            var dao = new CongVanDiDao();

            try
            {
                dao.UpdateImages(id, xElement.ToString());
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    status = false
                });
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChuyenTiep(CongVanDen idd,string tencongvan,string noidung)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var congvandi = new CongVanDi();
            congvandi.TenCongVan = tencongvan;
            congvandi.NoiDung = noidung;
            congvandi.IDNguoiGui = session.UserID;          
            congvandi.SendedDate = DateTime.Now;
            congvandi.SendedBy = session.UserName;
            congvandi.Status = true;
            var product = new CongVanDao().GetCategoryByID(idd.ID);
            var images = product.EmailSend;
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
            
            congvandi.EmailSend = product.EmailSend;

            foreach (var item in listImagesReturn)
            {
                string a = System.IO.File.ReadAllText(Server.MapPath("/Assets/Template/CongVanDi.html"));
                a = a.Replace("{{UserName}}", session.UserName);
                a = a.Replace("{{TenCongVan}}", tencongvan);
                a = a.Replace("{{NoiDung}}", noidung);
                a = a.Replace("{{IDNguoiGui}}", session.UserID.ToString());
                a = a.Replace("{{Email}}", session.Email);
                a = a.Replace("{{NgayGui}}", DateTime.Now.ToString());
                new MailHelper().SendMail(item, "Feedback Mới", a);           
            }
            var id = new CongVanDiDao().Insert(congvandi);

            if (id > 0)
            {
                SetAlert("Thêm Thành Công ", "success");
                return RedirectToAction("Index", "CongVan");
            }
            else
            {
                ModelState.AddModelError("", "Thêm Sản Phẩm Không Thành Công");
            }
            return View("Index");
        }
        [HasCredential(RoleID = "VIEW_CONGVAN")]
        public ActionResult Detail(long id)
        {

            var model = new CongVanDao().GetCategoryByID(id);
            return View(model);
        }


        


    }
}