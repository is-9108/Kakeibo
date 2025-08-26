using System.ComponentModel.DataAnnotations;

namespace Kakeibo.Models
{
    public partial class Home
    {
        [Display(Name = "カテゴリ")]
        public string Title { get; set; }
        public bool IsExpense { get; set; }
        [Display(Name = "合計金額")]
        public int Amount { get; set; }

    }
}
