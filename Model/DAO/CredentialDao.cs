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
    public class CredentialDao
    {
        CongVanWebDbContext db;

        public CredentialDao()
        {
            db = new CongVanWebDbContext();
        }

        public IEnumerable<Credential> ListAllPaging(string searchString, int page, int pageSize)
        {
            var model = db.Database.SqlQuery<Credential>("PSP_GetCredential").ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                IQueryable<Credential> search = db.Credentials.Where(x => x.RoleID.Contains(searchString) || x.UserGroupID.Contains(searchString));
                return search.OrderByDescending(x => x.RoleID).ToPagedList(page, pageSize);
            }

            return model.ToPagedList(page, pageSize);
        }


        public bool InsertUpdateCrendential(Credential credential)
        {

            Object[] param ={
                new SqlParameter("@RoleID",credential.RoleID),
                new SqlParameter("@UserGroupID",credential.UserGroupID)
            };

            int result = db.Database.ExecuteSqlCommand("PSP_InsertUpdateCredential @RoleID,@UserGroupID", param);
            if (result >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public int DeleteUserGroup(string id,string usergroup)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@RoleID",id),
                new SqlParameter("@UserGroupID",usergroup)

            };
            int result = db.Database.ExecuteSqlCommand("PSP_XoaCredential @RoleID,@UserGroupID", param);
            return result;
        }

        public Credential GetUserByID(string id , string usergroup)
        {
            Object[] param ={
                new SqlParameter("@RoleID",id),
                new SqlParameter("@UserGroupID",usergroup)
            };
            var list = db.Database.SqlQuery<Credential>("PSP_GetCredentialID @RoleID,@UserGroupID",param).SingleOrDefault();
            return list;
        }

        public bool CheckQuyen(string roleid,string group)
        {
            return db.Credentials.Count(x => x.RoleID == roleid && x.UserGroupID==group) > 0;
        }

        public List<UserGroup> GetGroupUser()
        {
            var list = db.Database.SqlQuery<UserGroup>("PSP_GetGroupUser").ToList();
            return list;
        }
        public List<Role> GetGroupCredential()
        {
            var list = db.Database.SqlQuery<Role>("PSP_GetRole").ToList();
            return list;
        }

    }
}
