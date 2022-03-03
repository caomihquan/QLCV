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
    public class CongVanDao
    {
        CongVanWebDbContext db;

        public CongVanDao()
        {
            db = new CongVanWebDbContext();
        }

        public IEnumerable<CongVanDen> ListAllPaging(string searchString,int page, int pageSize)
        {
            var model = db.Database.SqlQuery<CongVanDen>("PSP_GetCongVan").ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                IQueryable<CongVanDen> search = db.CongVanDens.Where(x => x.TenCongVan.Contains(searchString) || x.ID.ToString().Contains(searchString));
                return search.OrderByDescending(x=>x.CreatedDate).ToPagedList(page, pageSize);
            }

            return model.ToPagedList(page, pageSize);
        }
        /*        public IEnumerable<CongVanDen> SearchCongVanDen(int page, int pageSize)
                {
                    IQueryable<CongVanDen> model = db.CongVanDens;
                    if (!string.IsNullOrEmpty(searchString))
                    {

                      model = model.Where(x => x.TenCongVan.Contains(searchString) || x.ID.ToString().Contains(searchString));
                    }

                    return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
                }*/

        public bool InsertUpdateCongVanDen(CongVanDen congvanden)
        {

            /* Object[] param ={
                 new SqlParameter("@ID",congvanden.ID),               
                 new SqlParameter("@TenCongVan",congvanden.TenCongVan),
                 new SqlParameter("@NoiDung",congvanden.NoiDung),
                 new SqlParameter("@EmailSend",congvanden.EmailSend),
                 new SqlParameter("@CreatedDate",congvanden.CreatedDate),
                 new SqlParameter("@CreatedBy",congvanden.CreatedBy),
                 new SqlParameter("@ModifiedDate",congvanden.ModifiedDate),
                 new SqlParameter("@ModifiedBy",congvanden.ModifiedBy),

             };

             int result = db.Database.ExecuteSqlCommand("PSP_InsertCongVanDen @ID,@TenCongVan,@NoiDung,@EmailSend,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy", param);*/
            db.CongVanDens.Add(congvanden);
            db.SaveChanges();
            if (!string.IsNullOrEmpty(congvanden.FilePath))
            {
                string[] tags = congvanden.FilePath.Substring(1).Split(',');
                foreach (var tag in tags)
                {
                    var tagId = tag;
                    var existedTag = this.CheckTag(congvanden.ID,tagId);
                    if (!existedTag)
                    {
                        this.InsertContentTag(congvanden.ID, tagId);
                    }
                    //insert to content tag
                    

                }
            }

                return true;
            

        }


        public bool UpdateCongVan(CongVanDen entity)
        {
            try
            {
                var content = db.CongVanDens.Find(entity.ID);
                content.TenCongVan = entity.TenCongVan;
                content.NoiDung = entity.NoiDung;
                content.ModifiedDate = DateTime.Now;
                

                if (!string.IsNullOrEmpty(content.FilePath = entity.FilePath))
                {
                    this.RemoveAllFile(content.ID);
                    string[] tags = content.FilePath.Substring(1).Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = tag;
                        var existedTag = this.CheckTag(content.ID, tagId);
                        if (!existedTag)
                        {
                            this.InsertContentTag(content.ID, tagId);
                        }

                    }
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public CongVanDen GetCategoryByID(long iD)
        {
            var list = db.Database.SqlQuery<CongVanDen>("PSP_GetCongVanDenID @ID", new SqlParameter("@ID", iD)).SingleOrDefault();
            return list;
        }


        public int DeleteCongVanDen(long id)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@ID",id)

            };

            int result = db.Database.ExecuteSqlCommand("PSP_XoaCongVanDenID @ID", param);
            return result;
        }


        public void InsertContentTag(long idcongvan, string duongdan)
        {
            var contentTag = new FileCongVan();
            contentTag.IDCongVan = idcongvan;
            contentTag.PathID = duongdan;
            db.FileCongVans.Add(contentTag);
            db.SaveChanges();
        }
        public bool CheckTag(long id ,string path)
        {
            return db.FileCongVans.Count(x => x.IDCongVan == id&&x.PathID==path) > 0;
        }

        public void RemoveAllFile(long contentId)
        {
            db.FileCongVans.RemoveRange(db.FileCongVans.Where(x => x.IDCongVan == contentId));
            db.SaveChanges();
        }
    }
}
