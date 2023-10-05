using WebBomba.Helpers;
using WebBomba.Interfaces;

namespace WebBomba.Services
{
    public class ImageWorker : IImageWorker
    {
        private readonly IConfiguration _configuration;
        public ImageWorker(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public string ImageSave(IFormFile image) 
        {
            var imageSizes = _configuration.GetValue<string>("ImageSizes");
            var sizes = imageSizes.Split(",");
            string imageName = Guid.NewGuid().ToString() + ".webp";
            foreach (var size in sizes)
            {
                int width = int.Parse(size);
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                var bytes = ImageProcessingHelper.ResizeImage(image, width, width);
                System.IO.File.WriteAllBytes(Path.Combine(dir, size+"_"+imageName), bytes);
            }
            return imageName;
        }

        public string ImageSave(string url)
        {
            string imageName = Guid.NewGuid().ToString() + ".webp";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Send a GET request to the image URL
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    // Check if the response status code indicates success (e.g., 200 OK)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the image bytes from the response content
                        byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;
                        var imageSizes = _configuration.GetValue<string>("ImageSizes");
                        var sizes = imageSizes.Split(",");
                       
                        foreach (var size in sizes)
                        {
                            int width = int.Parse(size);
                            var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                            var bytes = ImageProcessingHelper.ResizeImage(imageBytes, width, width);
                            System.IO.File.WriteAllBytes(Path.Combine(dir, size + "_" + imageName), bytes);
                        }

                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve image. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return imageName;
        }

        public void RemoveImage(string name)
        {
            var imageSizes = _configuration.GetValue<string>("ImageSizes");
            var sizes = imageSizes.Split(",");
            string baseImagePath = name;

            foreach (var size in sizes)
            {
                string imagePathToDelete = Path.Combine(Directory.GetCurrentDirectory(), "images", size + "_" + baseImagePath);

                if (File.Exists(imagePathToDelete))
                {
                    File.Delete(imagePathToDelete);
                }
            }
        }
    }
}
