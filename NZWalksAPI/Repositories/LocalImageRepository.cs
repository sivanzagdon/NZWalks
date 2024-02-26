using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext nZWalksDbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            NZWalksDbContext nZWalksDbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            //Combine= method to generate a local file path for storing an image file within an application
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{ image.FileName}{ image.FileExtension}");

            //Upload Image to local path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // https://localhost:1234/Images/image.jpg

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://" +
                $"{httpContextAccessor.HttpContext.Request.Host}" +
                $"{httpContextAccessor.HttpContext.Request.PathBase}" +
                $"/Image/{image.FileName}{image.FileExtension}";

            image.FilePath= urlFilePath;

            //Add image to Image table
            await nZWalksDbContext.Images.AddAsync(image);
            await nZWalksDbContext.SaveChangesAsync();
            return image;
        }
    }
}
