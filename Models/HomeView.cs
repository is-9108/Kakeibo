namespace Kakeibo.Models
{
    public class HomeView
    {
        public IEnumerable<Home> Homes { get; set; }　= new List<Home>();
        public IEnumerable<Monthly_report> Monthly_Reports { get; set; } = new List<Monthly_report>();
    }
}
