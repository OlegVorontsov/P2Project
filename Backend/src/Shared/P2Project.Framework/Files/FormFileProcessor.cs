using FilesService.Core.Dtos;
using Microsoft.AspNetCore.Http;

namespace P2Project.Framework.Files
{
    public class FormFileProcessor : IAsyncDisposable
    {
        private readonly List<Stream> _fileStreams = [];

        public List<UploadFileDto> ToUploadFileDtos(
            IFormFileCollection files)
        {
            List<UploadFileDto> fileDtos = [];

            foreach (var file in files)
            {
                var stream = file.OpenReadStream();

                var fileDto = new UploadFileDto(
                    stream, file.FileName, file.Length, file.ContentType);
                
                fileDtos.Add(fileDto);
                _fileStreams.Add(fileDto.Stream);
            }

            return fileDtos;
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var item in _fileStreams)
            {
                await item.DisposeAsync();
            }
        }
    }
}
