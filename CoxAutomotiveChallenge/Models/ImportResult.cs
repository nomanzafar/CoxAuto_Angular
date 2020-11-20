namespace CoxAuto.Models
{
    public class ImportResult
    {
        public int RowNo { get; set; }
        public string PerformedAction { get; set; }
        public string Message { get; set; }
        public Deal Deal { get; set; }
    }
}
