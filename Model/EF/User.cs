namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public long ID { get; set; }

        [StringLength(50)]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Yêu cầu nhập")]
        public string UserName { get; set; }

        [StringLength(32)]
        [Required(ErrorMessage = "Yêu cầu nhập")]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Yêu cầu nhập")]
        [Display(Name = "Nhóm Người Dùng")]
        public string GroupID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Yêu cầu nhập")]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Yêu cầu nhập")]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Số Điện Thoại")]
        public string Phone { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
        [Display(Name = "Trạng Thái")]
        public bool Status { get; set; }
    }
}
