using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kakeibo.Models;

public partial class Category
{
    public int Id { get; set; }
    [Required]
    [Display(Name = "カテゴリ")]
    public string Name { get; set; } = null!;
    [Required]
    [Display(Name = "チェック：支出/チェックなし：収入　：　")]
    public bool IsExpense { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
