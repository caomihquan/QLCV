namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FileCongVan")]
    public partial class FileCongVan
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(500)]
        public string PathID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long IDCongVan { get; set; }
    }
}
