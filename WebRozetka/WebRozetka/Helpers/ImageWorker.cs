using SixLabors.ImageSharp.Formats.Webp;

namespace WebRozetka.Helpers
{
    public static class ImageWorker
    {
        public static async Task<string> SaveImageAsync(IFormFile image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                string fileName = await SaveBytesCompres(ms.ToArray());
                return fileName;
            }
        }

        public static async Task<string> SaveImageAsync(string base64)
        {
            if(base64.Contains(","))
                base64 = base64.Split(',')[1];
            var bytes = Convert.FromBase64String(base64);
            var fileName = await SaveBytesCompres(bytes);
            return fileName;
        }

        private static async Task<string> SaveBytesCompres(byte[] bytes)
        {
            string imageName = Path.GetRandomFileName() + ".webp";
            string dirSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "images", imageName);
            using (var image = Image.Load(bytes))
            {
                image.Mutate(x =>
                {
                    x.Resize(new ResizeOptions
                    {
                        Size = new Size(1200, 1200),
                        Mode = ResizeMode.Max
                    });
                });

                using (var stream = System.IO.File.Create(dirSaveImage))
                {
                    await image.SaveAsync(stream, new WebpEncoder());
                }
            }
            return imageName;
        }

    }
}
