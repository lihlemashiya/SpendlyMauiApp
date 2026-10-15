using System.Globalization;
using SQLite;
using Spendly.Helpers;
namespace Spendly.Models;

[Table("expenses")]
public class Expense
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [NotNull, MaxLength(60)]
    public string Title { get; set; } = string.Empty;
    public double Amount { get; set; }
    [NotNull, Indexed]
    public string Category { get; set; } = "Food";
    public DateTime Date { get; set; } = DateTime.Today;
    [MaxLength(200)]
    public string Note { get; set; } = string.Empty;
    // ----- Display properties: [Ignore] means they are NOT saved in the table -----
    [Ignore]
    public string AmountText => "R " + Amount.ToString("N2", CultureInfo.InvariantCulture);
    [Ignore]
    public string DateText => Date.ToString("d MMM yyyy", CultureInfo.InvariantCulture);
    [Ignore]
    public string Initial => string.IsNullOrEmpty(Category) ? "?" : Category.Substring(0, 1);
    [Ignore]
    public Color CategoryColor => CategoryHelper.ColorFor(Category);
}