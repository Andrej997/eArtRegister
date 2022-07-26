using eArtRegister.API.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace eArtRegister.API.WebApi.Extensions
{
    public static class WebApiExtensions
    {
        public static UploadedFileModel GetUploadFileModel(this IFormFile file)
        {
            return new UploadedFileModel
            {
                Content = file.OpenReadStream(),
                ContentType = file.ContentType,
                Extension = Path.GetExtension(file.FileName),
                Title = Path.GetFileNameWithoutExtension(file.FileName)
            };
        }
    }
}
