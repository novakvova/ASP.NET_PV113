using SixLabors.ImageSharp.Formats.Webp;

namespace WebBomba.Helpers
{
    public static class ImageProcessingHelper
    {
        public static byte[] ResizeImage(IFormFile formFile, int width, int height)
        {
            using(var image = Image.Load(formFile.OpenReadStream()))
            {
                image.Mutate(x =>
                {
                    x.Resize(new ResizeOptions
                    {
                        Size = new Size(width, height),
                        Mode = ResizeMode.Max
                    });
                });
                using(var ms = new MemoryStream())
                {
                    image.Save(ms, new WebpEncoder());
                    return ms.ToArray();
                }
            }
        }

        public static byte[] ResizeImage(byte[] bytes, int width, int height)
        {
            using (var image = Image.Load(bytes))
            {
                image.Mutate(x =>
                {
                    x.Resize(new ResizeOptions
                    {
                        Size = new Size(width, height),
                        Mode = ResizeMode.Max
                    });
                });
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, new WebpEncoder());
                    return ms.ToArray();
                }
            }
        }
    }
}
