namespace WebRozetka.Models.NovaPoshta
{

    public class NPWarehouseResponseViewModel
    {
        public List<NPWarehouseItemViewModel> Data { get; set; }
    }
    public class NPWarehouseItemViewModel
    {
        public string Ref { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }
        public string SettlementRef { get; set; }
    }
}
