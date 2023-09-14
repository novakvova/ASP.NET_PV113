namespace WebBomba.Interfaces
{
    public interface IImageWorker
    {
        string ImageSave(IFormFile image);
        string ImageSave(string url);
    }
}
