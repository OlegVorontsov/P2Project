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
        private const int EXPIRY = 60 * 60 * 24;
        private const int MAX_PARALLEL = 10;

        private readonly IMinioClient _minioClient;
        private readonly ILogger<MinioProvider> _logger;

        public MinioProvider(
            IMinioClient minioClient,
            ILogger<MinioProvider> logger)
        {
            _minioClient = minioClient;
            _logger = logger;
        }

        public async Task<Result<string, Error>> UploadFiles(
            FileData fileData,
            CancellationToken cancellationToken = default)
        {
            var semaphoreSlim = new SemaphoreSlim(MAX_PARALLEL);
            try
            {
                var isBucketExist = await IsBucketExist(
                    fileData.BucketName, cancellationToken);

                if (isBucketExist == false)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(fileData.BucketName);
                    await _minioClient.MakeBucketAsync(
                        makeBucketArgs, cancellationToken);
                }

                List<Task> tasks = [];
                foreach (var file in fileData.Files)
                {
                    await semaphoreSlim.WaitAsync(cancellationToken);

                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(fileData.BucketName)
                        .WithStreamData(file.Stream)
                        .WithObjectSize(file.Stream.Length)
                        .WithObject(file.ObjectName);

                    var task = _minioClient.PutObjectAsync(
                        putObjectArgs, cancellationToken);

                    semaphoreSlim.Release();

                    tasks.Add(task);
                }
                await Task.WhenAll(tasks);
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

        public async Task<Result<string, Error>> DeleteFile(
            FileMetadata fileMetadata,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var isBucketExist = await IsBucketExist(
                    fileMetadata, cancellationToken);

                if (isBucketExist == false)
                    throw new Exception($"Bucket {fileMetadata.BucketName} not exist");

                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(fileMetadata.BucketName)
                    .WithObject(fileMetadata.ObjectName);

                await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

                return $"File {fileMetadata.ObjectName} was deleted";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to delete file in minio");
                return Error.Failure("file.delete", "Fail to delete file in minio");
            }
        }

        public async Task<Result<string, Error>> GetFileURL(
            FileMetadata fileMetadata,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var isBucketExist = await IsBucketExist(
                    fileMetadata, cancellationToken);

                if (isBucketExist == false)
                    throw new Exception($"Bucket {fileMetadata.BucketName} not exist");

                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(fileMetadata.BucketName)
                    .WithObject(fileMetadata.ObjectName)
                    .WithExpiry(EXPIRY);

                var url = await _minioClient.PresignedGetObjectAsync(
                    presignedGetObjectArgs);

                return url;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to get file in minio");
                return Error.Failure("file.get", "Fail to get file in minio");
            }
        }

        private async Task<bool> IsBucketExist(
            string bucketName,
            CancellationToken cancellationToken)
        {
            var bucketExistBucketArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(
                bucketExistBucketArgs, cancellationToken);
            return bucketExist;
        }

        private async Task<bool> IsBucketExist(
            FileMetadata fileMetadata,
            CancellationToken cancellationToken)
        {
            var bucketExistBucketArgs = new BucketExistsArgs()
                .WithBucket(fileMetadata.BucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(
                bucketExistBucketArgs, cancellationToken);
            return bucketExist;
        }
    }
}
