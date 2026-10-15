using System.Globalization;
using Spendly.Helpers;

namespace Spendly.Models;

//NOT a table. Holds one row of the "GROUP BY Category" query on the Insights page.
public class CategoryTotal
{
    //These three come from the SQL query (the names match the SQL column names)
    public string Category { get; set; } = string.Empty;
    public double Total { get; set; }
    public int Count { get; set; }

    // Set in code: this category's part of all spending, form 0.0 to 1.0
    public double Share { get; set; }

    public string TotalText => "R " + Total . ToString("N2" , CultureInfo .InvariantCulture);
    public string DetailText => $"{Count} expense(s) . {Math.Round(Share * 100)}% of spending";
    public Color CategoryColor => CategoryHelper.ColorFor(Category);





}