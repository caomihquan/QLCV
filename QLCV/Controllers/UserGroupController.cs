using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLCV.Controllers
{
    public class UserGroupController : BaseController
    {
        // GET: UserGroup
        [HasCredential(RoleID = "VIEW_USERGROUP")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {

            var dao = new UserGroupDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_USERGROUP")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserGroup usergroup)
        {

            var id = true;
            if (ModelState.IsValid)
            {

                var dao = new UserGroupDao();
                if (dao.CheckQuyen(usergroup.ID))
                {
                    ModelState.AddModelError("", "đã tồn tại");

                }
                else
                {
                    id = dao.InsertUpdateUser(usergroup);

                    if (id)
                    {
                        SetAlert("Thêm Thành Công ", "success");
                        return RedirectToAction("Index", "UserGroup");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm Không Thành Công");
                    }
                }
               
            }
            return View(usergroup);
        }
        [HasCredential(RoleID = "EDIT_USERGROUP")]
        public ActionResult Edit(string id)
        {
            var list = new UserGroupDao().GetUserByID(id);
            return View(list);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(UserGroup user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserGroupDao();
                bool result = dao.InsertUpdateUser(user);
                if (result)
                {
                    SetAlert("Sửa Thành Công ", "success");
                    return RedirectToAction("Index", "UserGroup");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật Không thành công");
                }
            }
            return View(user);
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USERGROUP")]
        public ActionResult Delete(string id)
        {
            var result = new UserGroupDao().DeleteUserGroup(id);
            if (result > 0)
            {
                SetAlert("Thành Công ", "success");
                return RedirectToAction("Index", "UserGroup");
            }
            else
            {
                ModelState.AddModelError("", "Cập Nhật Không Thành Công");
            }
            return View("Index");

        }
    }
}