using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLCV.Controllers
{
    public class CredentialController : BaseController
    {
        // GET: Credential
        [HasCredential(RoleID = "VIEW_QUYEN")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {

            var dao = new CredentialDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_QUYEN")]
        public ActionResult Create()
        {
            SetViewBag();
            SetViewBagCreDential();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Credential credential)
        {

            var id = true;
            if (ModelState.IsValid)
            {
                var dao = new CredentialDao();
                if (dao.CheckQuyen(credential.RoleID, credential.UserGroupID))
                {
                    ModelState.AddModelError("", "đã tồn tại");

                }
                else
                {
                    id = dao.InsertUpdateCrendential(credential);
                    if (id)
                    {
                        SetAlert("Thêm Thành Công ", "success");
                        return RedirectToAction("Index", "Credential");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm Sản Phẩm Không Thành Công");
                    }
                }
                
            }
            SetViewBag();
            SetViewBagCreDential();
            return View(credential);
        }
        [HasCredential(RoleID = "EDIT_QUYEN")]
        public ActionResult Edit(string id ,string user)
        {
            var list = new CredentialDao().GetUserByID(id,user);
            SetViewBag(list.UserGroupID);
            SetViewBagCreDential(list.RoleID);
            return View(list);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Credential credential)
        {
            if (ModelState.IsValid)
            {
                var dao = new CredentialDao();
                if (dao.CheckQuyen(credential.RoleID,credential.UserGroupID))
                {
                    ModelState.AddModelError("", "đã tồn tại");
                }
                else
                {
                    bool result = dao.InsertUpdateCrendential(credential);
                    if (result)
                    {
                        SetAlert("Sửa Thành Công ", "success");
                        return RedirectToAction("Index", "Credential");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật Sản Phẩm Không thành công");
                    }

                }
                
            }
            SetViewBag(credential.UserGroupID);
            SetViewBagCreDential(credential.RoleID);
            return View(credential);
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_QUYEN")]
        public ActionResult Delete(string id,string user)
        {
            var result = new CredentialDao().DeleteUserGroup(id,user);
            if (result > 0)
            {
                SetAlert("Thành Công ", "success");
                return RedirectToAction("Index", "Credential");
            }
            else
            {
                ModelState.AddModelError("", "Cập Nhật Không Thành Công");
            }
            return View("Index");

        }


        public void SetViewBag(string selectedId = null)
        {
            var dao = new CredentialDao();
            ViewBag.UserGroupID = new SelectList(dao.GetGroupUser(), "ID", "Name", selectedId);
        }
        public void SetViewBagCreDential(string selectedId = null)
        {
            var dao = new CredentialDao();
            ViewBag.RoleID = new SelectList(dao.GetGroupCredential(), "ID", "Name", selectedId);
        }

    }
}