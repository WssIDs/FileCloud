namespace FileCloud.Server.Models.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtAuthSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TokenLifeTime { get; set; }
    }
}