using Model.DAO;
using Model.EF;
using QLCV.Common;
using QLCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QLCV.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {

            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var id = true;
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckEmail(user.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");

                }
                else if (dao.CheckUserName(user.UserName))
                {
                    ModelState.AddModelError("", "tên tài khoản đã tồn tại");

                }
                else
                {
                    if (user.Address == null)
                    {
                        user.Address = "";
                    }
                    else
                    {
                        user.Address = user.Address;
                    }
                    if (user.Phone == null)
                    {
                        user.Phone = "";
                    }
                    else
                    {
                        user.Phone = user.Phone;
                    }
                    user.Password = user.Password;
                    user.ModifiedBy = session.UserName;
                    user.CreatedBy = session.UserName;
                    user.CreatedDate = DateTime.Now;
                    user.ModifiedDate = DateTime.Now;
                    user.Status = true;
                    id = dao.InsertUpdateUser(user);
                    if (id)
                    {
                        SetAlert("Thêm Thành Công ", "success");
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm Không Thành Công");
                    }
                }
                
            }
            SetViewBag();
            return View(user);
        }
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(long id)
        {
            var list = new UserDao().GetUserByID(id);
            SetViewBag(list.GroupID);
            return View(list);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(User user)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckEmail(user.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");

                }
                else if (dao.CheckUserName(user.UserName))
                {
                    ModelState.AddModelError("", "tên tài khoản đã tồn tại");

                }
                else
                {
                    user.Password = user.Password;
                    user.ModifiedBy = session.UserName;
                    user.ModifiedDate = DateTime.Now;
                    if (user.Address == null)
                    {
                        user.Address = "";
                    }

                    else
                    {
                        user.Address = user.Address;
                    }
                    if (user.CreatedBy == null)
                    {
                        user.CreatedBy = "";
                    }
                    else
                    {
                        user.CreatedBy = user.CreatedBy;
                    }
                    if (user.Phone == null)
                    {
                        user.Phone = "";
                    }
                    else
                    {
                        user.Phone = user.Phone;
                    }
                    bool result = dao.InsertUpdateUser(user);
                    if (result)
                    {
                        SetAlert("Sửa Thành Công ", "success");
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật Không thành công");
                    }
                }

            }
            SetViewBag(user.GroupID);
            return View(user);
        }

        public void SetViewBag(string selectedId = null)
        {
            var dao = new UserDao();
            ViewBag.GroupID = new SelectList(dao.GetGroupUser(), "ID", "Name", selectedId);
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            var result = new UserDao().DeleteUser(id);
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

    }
}