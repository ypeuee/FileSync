using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIleSyncData.Models
{

    /// <summary>
    /// 摘果机工位信息
    /// </summary>
    [Table("TM_PickingArea")]
    public class TM_PickingAreaM
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("Id")]
        public Int64 Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column("AreaName", TypeName = "varchar")]
        public string AreaName { get; set; }

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
