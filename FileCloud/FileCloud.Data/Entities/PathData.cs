using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCloud.Data.Entities
{
    public class PathData
    {
        public PathData()
        {
            Files = new HashSet<FileInfoData>();
            Paths = new HashSet<PathData>();
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Root { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<FileInfoData> Files { get; set; }

        public virtual ICollection<PathData> Paths { get; set; }
    }
}
