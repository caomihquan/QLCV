using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class FileDao
    {
        CongVanWebDbContext db;

        public FileDao()
        {
            db = new CongVanWebDbContext();
        }


        public List<FileCongVan> ListById(long id)
        {
          
            var model = db.FileCongVans.Where(x => x.IDCongVan == id).ToList();
            return model;

        }
        public IEnumerable<FileCongVan> ListAllPaging(string searchString, int page, int pageSize)
        {
            var model = db.Database.SqlQuery<FileCongVan>("PSP_File").ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                IQueryable<FileCongVan> search = db.FileCongVans.Where(x => x.IDCongVan.ToString().Contains(searchString) || x.PathID.Contains(searchString));
                return search.OrderByDescending(x => x.PathID).ToPagedList(page, pageSize);
            }

            return model.ToPagedList(page, pageSize);
        }
        public bool Delete(long id,string path)
        {
            try
            {
                var productcategory = db.FileCongVans.SingleOrDefault(x => x.PathID == path && x.IDCongVan == id);
                db.FileCongVans.Remove(productcategory);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}
