namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserGroup")]
    public partial class UserGroup
    {
        [StringLength(20)]
        [Required(ErrorMessage = "Bạn Phải Nhập")]
        public string ID { get; set; }
        [Required(ErrorMessage = "Bạn Phải Nhập")]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
