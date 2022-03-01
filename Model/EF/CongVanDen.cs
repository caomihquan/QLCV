namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CongVanDen")]
    public partial class CongVanDen
    {
        [Key]
        public long ID { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Bạn Phải Nhập Tên Công Văn")]
        public string TenCongVan { get; set; }
        [Required(ErrorMessage = "Bạn Phải Nhập Nội Dung")]
        [Column(TypeName = "ntext")]
        public string NoiDung { get; set; }

        [Column(TypeName = "xml")]
        public string EmailSend { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public bool Status { get; set; }
    }
}
