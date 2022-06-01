using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCloud.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class PathData
    {
        /// <summary>
        /// 
        /// </summary>
        public PathData()
        {
            Files = new HashSet<FileInfoData>();
            Paths = new HashSet<PathData>();
        }

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
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("PathDataId")]
        public PathData RootPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<FileInfoData> Files { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<PathData> Paths { get; set; }
    }
}
