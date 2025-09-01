using System.ComponentModel.DataAnnotations;

namespace Kakeibo.Models
{
    public class Monthly_report
    {
        public int Year { get; set; }
        public int Month { get; set; }
        [Display(Name = "合計収入")]
        public int TotalIncome { get; set; }
        [Display(Name = "合計支出")]
        public int TotalExpense { get; set; }
        [Display(Name = "収支")]
        public int Balance { get; set; }

    }
}
