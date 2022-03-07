using Common;
using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class UserDao
    {
        CongVanWebDbContext db;

        public UserDao()
        {
            db = new CongVanWebDbContext();
        }

        public IEnumerable<User> ListAllPaging(string searchString, int page, int pageSize)
        {
            var model = db.Database.SqlQuery<User>("PSP_GetAllUser").ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                IQueryable<User> search = db.Users.Where(x => x.UserName.Contains(searchString) || x.ID.ToString().Contains(searchString)||x.Name.Contains(searchString));
                return search.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }

            return model.ToPagedList(page, pageSize);
        }

        public bool InsertUpdateUser(User user)
        {

            Object[] param ={
                new SqlParameter("@ID",user.ID),
                new SqlParameter("@UserName",user.UserName),
                new SqlParameter("@Password",user.Password),
                new SqlParameter("@GroupID",user.GroupID),
                new SqlParameter("@Name",user.Name),
                new SqlParameter("@Address",user.Address),
                new SqlParameter("@Email",user.Email),
                new SqlParameter("@Phone",user.Phone),
                new SqlParameter("@CreatedDate",user.CreatedDate),
                new SqlParameter("@CreatedBy",user.CreatedBy),
                new SqlParameter("@ModifiedDate",user.ModifiedDate),
                new SqlParameter("@ModifiedBy",user.ModifiedBy),
                new SqlParameter("@Status",user.Status)
            };

            int result = db.Database.ExecuteSqlCommand("PSP_InsertUpdateUser @ID,@UserName,@Password,@GroupID,@Name,@Address,@Email,@Phone,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy,@Status", param);
            if (result >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public User GetByID(string userName)
        {
            var list = db.Database.SqlQuery<User>("PSP_GetUser @UserName", new SqlParameter("@UserName", userName)).SingleOrDefault();
            return list;
        }


        public User GetUserByID(long id)
        {
            var list = db.Database.SqlQuery<User>("PSP_GetUserID @ID", new SqlParameter("@ID", id)).SingleOrDefault();
            return list;
        }



        public List<string> GetListCredential(string userName)
        {
            var user = db.Users.Single(x => x.UserName == userName ||x.Email==userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();

        }

        public int Login(string userName, string passWord, bool isLoginAdmin = false)
        {
            
            var result = db.Database.SqlQuery<User>("PSP_GetUser @UserName", new SqlParameter("@UserName", userName)).SingleOrDefault();
            if (result == null)
            {
                return 0;
            }
            else
            {

                if (isLoginAdmin == true)
                {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == passWord)
                                return 1;
                            else
                                return -2;
                        }
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                            return 1;
                        else
                            return -2;
                    }
                }
            }
        }

        public int DeleteUser(int id)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@ID",id)

            };

            int result = db.Database.ExecuteSqlCommand("PSP_XoaUser @ID", param);
            return result;
        }

        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }
        public bool CheckUserName(string username)
        {
            return db.Users.Count(x => x.UserName == username) > 0;
        }

        public List<UserGroup> GetGroupUser()
        {
            var list = db.Database.SqlQuery<UserGroup>("PSP_GetGroupUser").ToList();
            return list;
        }
    }
}
