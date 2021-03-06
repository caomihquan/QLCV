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
        [Display(Name = "Vai Tr?")]
        public string RoleID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        [Display(Name = "Nh?m User")]
        public string UserGroupID { get; set; }
    }
}
