﻿using FileCloudClient.Models;

namespace FileCloudClient.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAdminService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<List<UserInfoModel>> GetAllUsersAsync();
    }
}