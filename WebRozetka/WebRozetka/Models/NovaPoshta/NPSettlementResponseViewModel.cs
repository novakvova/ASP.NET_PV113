namespace WebRozetka.Models.NovaPoshta
{

    public class NPSettlementResponseViewModel
    {
        public List<NPSettlementItemViewModel> Data { get; set; }
    }
    public class NPSettlementItemViewModel
    {
        public string Ref { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
    }
}
