using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }
        [NotMapped] //should not be mapped to a database column. 
        public IFormFile File { get; set; } //file data of the image
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtension { get; set; } //e.g., ".jpg", ".png"
        public long FileSizeInBytes { get; set; }
        public string FilePath { get; set; }
    }
}
