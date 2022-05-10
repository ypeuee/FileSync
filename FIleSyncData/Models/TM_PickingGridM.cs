using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIleSyncData.Models
{
    /// <summary>
    /// 摘果机格子信息
    /// </summary>
    [Table("TM_PickingGrid")]
    public class TM_PickingGridM
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("Id")]
        public long Id { get; set; }

        [Required]
        [Column("AreaId")]
        public long AreaId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column("GridName", TypeName = "varchar")]
        public string GridName { get; set; }

        /// <summary>
        /// 标签设备ID
        /// </summary>
        [Required]
        [Column("EquipmentID", TypeName = "int")]
        public int EquipmentID { get; set; }


        /// <summary>
        /// 设备通道
        /// </summary>
        [Required]
        [Column("ChannelID", TypeName = "int")]
        public int ChannelID { get; set; }

        /// <summary>
        /// TPLID设备ID
        /// </summary>
        [Required]
        [Column("TPLID", TypeName = "int")]
        public int TPLID { get; set; }

    }
}