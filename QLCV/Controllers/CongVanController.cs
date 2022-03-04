using Common;
using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        public ActionResult Create(CongVanDen congvanden, List<HttpPostedFileBase> file)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var id = true;
            string chuoi = "";
            if (ModelState.IsValid)
            {
                var dao = new CongVanDao();
                congvanden.ModifiedBy = session.UserName;
                congvanden.CreatedBy = session.UserName;
                congvanden.EmailSend = "<Images></Images>";
                congvanden.CreatedDate = DateTime.Now;
                congvanden.ModifiedDate = DateTime.Now;
                if (file[0] == null)
                {
                    congvanden.FilePath = null;
                }
                else
                {
                    foreach (HttpPostedFileBase f in file)
                    {
                        string files = Path.GetFileName(f.FileName);
                        string _path = Path.Combine(Server.MapPath("/Data"), files);
                        var video = _path;
                        f.SaveAs(_path);

                        chuoi = chuoi + "," + video;

                    }
                    congvanden.FilePath = chuoi;
                }              
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
        public ActionResult Edit(CongVanDen congvanden,List<HttpPostedFileBase> file)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            string chuoi = "";
            if (ModelState.IsValid)
            {
                var dao = new CongVanDao();
                congvanden.ModifiedBy = session.UserName;
                congvanden.EmailSend = "<Images></Images>";                
                congvanden.ModifiedDate = DateTime.Now;
                if (file[0] == null)
                {
                    congvanden.FilePath = congvanden.FilePath;
                }
                else
                {
                    foreach (HttpPostedFileBase f in file)
                    {
                        string files = Path.GetFileName(f.FileName);
                        string _path = Path.Combine(Server.MapPath("/Data"), files);
                        var video = _path;
                        f.SaveAs(_path);

                        chuoi = chuoi + "," + video;

                    }
                    congvanden.FilePath = chuoi;
                }
                bool result = dao.UpdateCongVan(congvanden);
                if (result)
                {
                    SetAlert("Sửa Thành Công ", "success");
                    return RedirectToAction("Index", "CongVan");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật Không thành công");
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
        public ActionResult ChuyenTiep(CongVanDen idd,string tencongvan,string noidung, List<HttpPostedFileBase> file)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var congvandi = new CongVanDi();
            congvandi.TenCongVan = tencongvan;
            congvandi.NoiDung = noidung;
            congvandi.IDNguoiGui = session.UserID;
            congvandi.SendedDate = DateTime.Now;
            congvandi.SendedBy = session.UserName;
            
            var product = new CongVanDao().GetCategoryByID(idd.ID);
            congvandi.FilePath = product.FilePath;
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
            string chuoi = "";
            foreach (var item in listImagesReturn)
            {
                string a = System.IO.File.ReadAllText(Server.MapPath("/Assets/Template/CongVanDi.html"));
                a = a.Replace("{{UserName}}", session.UserName);
                a = a.Replace("{{TenCongVan}}", tencongvan);
                a = a.Replace("{{NoiDung}}", noidung);
                a = a.Replace("{{IDNguoiGui}}", session.UserID.ToString());
                a = a.Replace("{{Email}}", session.Email);
                a = a.Replace("{{NgayGui}}", DateTime.Now.ToString());
                SendMail(item, "Công Văn Mới", a,file,product.FilePath,ref chuoi);
            }
            congvandi.FilePath = chuoi;
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


        public void SendMail(string toEmailAddress, string subject, string content, List<HttpPostedFileBase> file,string duongdanfile, ref string filepath)
        {
            var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();

            bool enabledSsl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());

            string body = content;
            string chuoi = "";
            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));

            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            if (file[0] == null)
            {
                if (duongdanfile == null)
                {

                }
                else
                {
                    string[] tags = duongdanfile.Substring(1).Split(',');
                    foreach (var item in tags)
                    {
                        string fileName = Path.GetFileName(item);
                        byte[] bytes = System.IO.File.ReadAllBytes(item);
                        message.Attachments.Add(new Attachment(new MemoryStream(bytes), fileName));
                    }
                }               
            }
            else
            {
                foreach (HttpPostedFileBase f in file)
                {
                    string files = Path.GetFileName(f.FileName);
                    message.Attachments.Add(new Attachment(f.InputStream, files));                    
                    string _path = Path.Combine(Server.MapPath("/Data"), files);
                    var video = _path;
                    f.SaveAs(_path);
                    chuoi = chuoi + "," + video;
                }
                    filepath = chuoi;
            }
            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = enabledSsl;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);
        }

        [HttpPost]
        public JsonResult ClearFilePath(long id)
        {
            var result = new CongVanDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}