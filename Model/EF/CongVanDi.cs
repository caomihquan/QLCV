namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CongVanDi")]
    public partial class CongVanDi
    {
        public long ID { get; set; }

        [StringLength(250)]
        public string TenCongVan { get; set; }

        [Column(TypeName = "ntext")]
        public string NoiDung { get; set; }
        [StringLength(500)]
        public string FilePath { get; set; }

        public long? IDNguoiGui { get; set; }

        [StringLength(500)]
        public string EmailSend { get; set; }

        public DateTime? SendedDate { get; set; }

        [StringLength(50)]
        public string SendedBy { get; set; }
    }
}
