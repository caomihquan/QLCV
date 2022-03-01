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
    public class CongVanDiDao
    {
        CongVanWebDbContext db;
        public CongVanDiDao()
        {
            db = new CongVanWebDbContext();
        }
        public long Insert(CongVanDi convandi)
        {
            db.CongVanDis.Add(convandi);
            db.SaveChanges();
            return convandi.ID;
        }

        public bool UpdateImages(long id, string images)
        {
            try
            {
                var about = db.CongVanDens.Find(id);
                about.EmailSend = images;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<string> ListEmail(string keyword)
        {
            return db.Users.Where(x => x.Email.Contains(keyword) || x.Name.Contains(keyword) || x.UserName.Contains(keyword)).Select(x => x.Email).ToList();
        }
        public IEnumerable<CongVanDi> ListAllPaging(string searchString, int page, int pageSize)
        {
            var model = db.Database.SqlQuery<CongVanDi>("PSP_GetCongVanGuiDi").ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                IQueryable<CongVanDi> search = db.CongVanDis.Where(x => x.TenCongVan.Contains(searchString) || x.ID.ToString().Contains(searchString));
                return search.OrderByDescending(x => x.SendedDate).ToPagedList(page, pageSize);
            }

            return model.ToPagedList(page, pageSize);
        }

        public CongVanDi GetCongVanDi(long id)
        {
            var list = db.Database.SqlQuery<CongVanDi>("PSP_GetCongVanGuiDiID @ID", new SqlParameter("@ID", id)).SingleOrDefault();
            return list;
        }


        public int DeleteCongVanDi(long id)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@ID",id)

            };

            int result = db.Database.ExecuteSqlCommand("PSP_XoaCongVanDiID @ID", param);
            return result;
        }

    }
}
