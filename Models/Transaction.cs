using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kakeibo.Models;

public partial class Transaction
{
    public int Id { get; set; }

    [Display(Name = "カテゴリ")]
    public int CategoryId { get; set; }

    [Required]
    [Display(Name = "タイトル")]
    public string Title { get; set; } = null!;

    [Required]
    [Display(Name = "金額")]
    public int Amount { get; set; }

    [Required]
    [Display(Name = "登録日")]
    public DateTime Date { get; set; }


    public virtual Category? Category { get; set; }
}
