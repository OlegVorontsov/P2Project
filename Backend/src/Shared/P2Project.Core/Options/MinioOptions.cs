
namespace P2Project.Core.Options
{
    public class MinioOptions
    {
        public const string MINIO = "Minio";
        public string EndPoint { get; init; } = string.Empty;
        public string AccessKey { get; init; } = string.Empty;
        public string SecretKey { get; init; } = string.Empty;
        public bool WithSSl {  get; init; } = false;
    }
}
