using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using P2Project.SharedKernel.Errors;

namespace P2Project.Core.Extensions;

public static class FormFileExtensions
{
    public static async Task<Result<byte[], Error>> ToByteArrayAsync(this IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return Errors.General.Failure("Файл не загружен или пустой");

        using var memoryStream = new MemoryStream();

        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}