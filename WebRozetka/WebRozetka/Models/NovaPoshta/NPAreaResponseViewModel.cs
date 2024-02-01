namespace WebRozetka.Models.NovaPoshta
{

    public class NPAreaResponseViewModel
    {
        public List<NPAreaItemViewModel> Data { get; set; }
    }
    public class NPAreaItemViewModel
    {
        public string Ref { get; set; }
        public string Description { get; set; }
    }
}
