namespace FileCloudClient.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITokenUpdateService
    {
        Task RunAsync();

        void Stop();
    }
}