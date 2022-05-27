using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileCloud.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class FileInfoData
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Name { get; set; }

        [Required]
        public string OrginalFileName { get; set; }

        [Required]
        [ForeignKey("Path")]
        public Guid PathId { get; set; }

        [Required]
        public virtual PathData Path { get; set; }
    }
}
