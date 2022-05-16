using FileCloud.Shared.Models;
using FileCloudClient.Components.Table;

namespace FileCloudClient.Models
{
    public class UserInfoModel : UserModel, IRow
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
