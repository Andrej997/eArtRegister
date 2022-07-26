using System.IO;

namespace eArtRegister.API.Application.Common.Models
{
    public class UploadedFileModel
    {
        public Stream Content { get; set; }
        public string ContentType { get; set; }
        public long CompanyId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }

        public UploadedFileModel()
        {
            Name = Path.GetRandomFileName();
        }

        public string SetFullName(string path)
        {
            FullName = string.Format("{0}{1}{2}", path, Name, Extension);
            return FullName;
        }


    }
}
