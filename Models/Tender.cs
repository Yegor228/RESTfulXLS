namespace RESTfulXLS.Models
{
    public class Tender
    {

        public int Id {  get; set; }

        public string? Name {  get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd {  get; set;}

        public string? Url {  get; set; }

    }
}
