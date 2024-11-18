using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using P2Project.Application.FileProvider;
using P2Project.Application.FileProvider.Models;
using P2Project.Domain.Shared;

namespace P2Project.Infrastructure.Providers
{
    public class MinioProvider : IFileProvider
    {
        private readonly IMinioClient _minioClient;
        private readonly ILogger<MinioProvider> _logger;

        public MinioProvider(
            IMinioClient minioClient,
            ILogger<MinioProvider> logger)
        {
            _minioClient = minioClient;
            _logger = logger;
        }

        public async Task<Result<string, Error>> UploadFile(
            UploadFileRecord uploadFileRecord,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var bucketExistBucketArgs = new BucketExistsArgs()
                    .WithBucket("photos");

                var bucketExist = await _minioClient.BucketExistsAsync(
                    bucketExistBucketArgs, cancellationToken);

                if (bucketExist == false)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket("photos");
                    await _minioClient.MakeBucketAsync(
                        makeBucketArgs, cancellationToken);
                }

                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(uploadFileRecord.BucketName)
                    .WithStreamData(uploadFileRecord.Stream)
                    .WithObjectSize(uploadFileRecord.Stream.Length)
                    .WithObject(uploadFileRecord.ObjectName);

                var result = await _minioClient.PutObjectAsync(
                    putObjectArgs, cancellationToken);

                return result.ObjectName;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Upload file to minio failed");

                return Error.Failure(
                    "file.upload", "Upload file to minio failed");
            }

        }
    }
}
