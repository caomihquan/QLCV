﻿namespace Model.EF
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
        [Required(ErrorMessage = "Bạn Phải Nhập UserName")]
        public string UserName { get; set; }

        [StringLength(32)]
        [Required(ErrorMessage = "Bạn Phải Nhập PassWord")]
        public string Password { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Bạn Phải Chọn Loại Tài Khoản")]
        public string GroupID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Bạn Phải Họ Tên")]
        public string Name { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Bạn Phải Nhập Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

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
