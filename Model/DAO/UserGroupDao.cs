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
    public class UserGroupDao
    {
        CongVanWebDbContext db;

        public UserGroupDao()
        {
            db = new CongVanWebDbContext();
        }


        public IEnumerable<UserGroup> ListAllPaging(string searchString, int page, int pageSize)
        {
            var model = db.Database.SqlQuery<UserGroup>("PSP_GetAllUserGroup").ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                IQueryable<UserGroup> search = db.UserGroups.Where(x => x.ID.Contains(searchString) || x.Name.Contains(searchString));
                return search.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
            }

            return model.ToPagedList(page, pageSize);
        }


        public bool InsertUpdateUser(UserGroup usergroup)
        {

            Object[] param ={
                new SqlParameter("@ID",usergroup.ID),
                new SqlParameter("@Name",usergroup.Name)
            };

            int result = db.Database.ExecuteSqlCommand("PSP_InsertUpdateUserGroup @ID,@Name", param);
            if (result >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public int DeleteUserGroup(string id)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@ID",id)

            };
            int result = db.Database.ExecuteSqlCommand("PSP_XoaUserGroup @ID", param);
            return result;
        }

        public UserGroup GetUserByID(string id)
        {
            var list = db.Database.SqlQuery<UserGroup>("PSP_GetUserGroupID @ID", new SqlParameter("@ID", id)).SingleOrDefault();
            return list;
        }

        public bool CheckQuyen(string id)
        {
            return db.Credentials.Count(x => x.RoleID == id) > 0;
        }
    }
}
