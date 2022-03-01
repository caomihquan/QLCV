namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Credential
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        [Required(ErrorMessage = "Bạn Phải Nhập")]
        public string RoleID { get; set; }

        [Key]
        [Required(ErrorMessage = "Bạn Phải Nhập")]
        [Column(Order = 1)]
        [StringLength(20)]
        public string UserGroupID { get; set; }
    }
}
