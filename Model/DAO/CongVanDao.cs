﻿using Model.EF;
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
          
            Object[] param ={
                new SqlParameter("@ID",congvanden.ID),               
                new SqlParameter("@TenCongVan",congvanden.TenCongVan),
                new SqlParameter("@NoiDung",congvanden.NoiDung),
                new SqlParameter("@EmailSend",congvanden.EmailSend),
                new SqlParameter("@CreatedDate",congvanden.CreatedDate),
                new SqlParameter("@CreatedBy",congvanden.CreatedBy),
                new SqlParameter("@ModifiedDate",congvanden.ModifiedDate),
                new SqlParameter("@ModifiedBy",congvanden.ModifiedBy),
                new SqlParameter("@Status",congvanden.Status)
            };

            int result = db.Database.ExecuteSqlCommand("PSP_InsertCongVanDen @ID,@TenCongVan,@NoiDung,@EmailSend,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy,@Status", param);
            if(result >= 1) {
                return true;
            }
            else
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
    }
}
