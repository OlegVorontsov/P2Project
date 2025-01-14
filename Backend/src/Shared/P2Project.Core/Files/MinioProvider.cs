using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using P2Project.Core.Files.Models;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Core.Files
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

        public async Task<Result<string, Error>> UploadFile(
            FileData fileData,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await CreateBucketIfNotExists(
                    fileData.FileInfoDto.BucketName, cancellationToken);

                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(fileData.FileInfoDto.BucketName)
                    .WithStreamData(fileData.FileStream)
                    .WithObjectSize(fileData.FileStream.Length)
                    .WithObject(fileData.FileInfoDto.FilePath.Path);

                var result = await _minioClient.PutObjectAsync(
                    putObjectArgs, cancellationToken);
                return result.ObjectName;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to upload file to minio");
                return Error.Failure("file.upload", "Failed to upload file to minio");
            }
        }

        public async Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
            IEnumerable<FileData> filesData,
            CancellationToken cancellationToken = default)
        {
            var semaphoreSlim = new SemaphoreSlim(MAX_PARALLEL);
            var filesList = filesData.ToList();
            try
            {
                await CreateBucketsIfNotExist(
                    filesList.Select(file => file.FileInfoDto.BucketName),
                    cancellationToken);

                var tasks = filesList.Select(
                    async file => await PutObject(file, semaphoreSlim, cancellationToken));

                var pathResult = await Task.WhenAll(tasks);

                if (pathResult.Any(p => p.IsFailure))
                    return pathResult.First().Error;

                var results = pathResult.Select(p => p.Value).ToList();

                _logger.LogInformation("Uploaded files {files}",
                    results.Select(f => f.Path));

                return results;
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

        public async Task<Result<string, Error>> DeleteFileByFileMetadata(
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

        public async Task<UnitResult<Error>> DeleteFileByFileInfo(
            FileInfoDto fileInfoDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var isBucketExist = await IsBucketExist(
                    fileInfoDto.BucketName, cancellationToken);

                if (isBucketExist == false)
                    throw new Exception($"Bucket {fileInfoDto.BucketName} not exist");

                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(fileInfoDto.BucketName)
                    .WithObject(fileInfoDto.FilePath.Path);

                var statObject = await _minioClient.StatObjectAsync(statObjectArgs, cancellationToken);
                if (statObject.Size == 0)
                    return Result.Success<Error>();

                var removeArgs = new RemoveObjectArgs()
                    .WithBucket(fileInfoDto.BucketName)
                    .WithObject(fileInfoDto.FilePath.Path);

                await _minioClient.RemoveObjectAsync(removeArgs, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
               "Failed to remove file in minio with path {path} in bucket {bucket}",
               fileInfoDto.FilePath.Path,
               fileInfoDto.BucketName);

                return Error.Failure("file.remove", "Failed to remove file in minio");
            }
            return Result.Success<Error>();
        }

        public async Task<Result<string, Error>> GetFile(
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

        private async Task CreateBucketIfNotExists(
            string bucketName,
            CancellationToken cancellationToken = default)
        {
            var bucketExist = await IsBucketExist(
                bucketName, cancellationToken);

            if (bucketExist == false)
            {
                await CreateBucket(bucketName, cancellationToken);
            }
        }

        private async Task CreateBucketsIfNotExist(
            IEnumerable<string> buckets,
            CancellationToken cancellationToken = default)
        {
            HashSet<String> bucketNames = [.. buckets];

            foreach (var bucketName in bucketNames)
            {
                await CreateBucketIfNotExists(bucketName, cancellationToken);
            }
        }

        private async Task CreateBucket(
            string bucketName,
            CancellationToken cancellationToken = default)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(
                bucketName);
            await _minioClient.MakeBucketAsync(
                makeBucketArgs, cancellationToken);
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

        private async Task<Result<FilePath, Error>> PutObject(
            FileData fileData,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken = default)
        {
            await semaphoreSlim.WaitAsync(cancellationToken);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(fileData.FileInfoDto.BucketName)
                .WithStreamData(fileData.FileStream)
                .WithObjectSize(fileData.FileStream.Length)
                .WithObject(fileData.FileInfoDto.FilePath.Path);

            try
            {
                await _minioClient
                    .PutObjectAsync(putObjectArgs, cancellationToken);

                return fileData.FileInfoDto.FilePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to upload file in minio with path {path} in bucket {bucket}",
                    fileData.FileInfoDto.FilePath.Path,
                    fileData.FileInfoDto.BucketName);

                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
