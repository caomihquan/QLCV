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
        public long ID { get; set; }

        [Display(Name = "Tên Công Văn")]
        [Required(ErrorMessage = "Yêu cầu nhập")]
        [StringLength(250)]
        public string TenCongVan { get; set; }
        [Display(Name = "Nội Dung")]
        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Yêu cầu nhập")]
        public string NoiDung { get; set; }

        [Column(TypeName = "xml")]
        public string EmailSend { get; set; }

        [Column(TypeName = "ntext")]
        public string FilePath { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}
