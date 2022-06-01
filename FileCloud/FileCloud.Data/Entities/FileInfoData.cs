using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileCloud.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class FileInfoData
    {
        /// <summary>
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid Name { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string OrginalFileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDeleted { get; set; }

        [Required]
        [ForeignKey("Path")]
        public Guid PathId { get; set; }

        [Required]
        public virtual PathData Path { get; set; }
    }
}
