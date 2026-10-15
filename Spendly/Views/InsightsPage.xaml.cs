using Spendly.Data;
using Spendly.Models;
using System.Globalization;
using Spendly.Data;
using Spendly.Models;
namespace Spendly.Views;

public partial class InsightsPage : ContentPage
{
    private readonly ExpenseDatabase _database;
    public InsightsPage(ExpenseDatabase database)
    {
        InitializeComponent();
        _database = database;
    }
    // Reload every time the tab is opened, so new expenses show up
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadInsightsAsync();
    }
    private async Task LoadInsightsAsync()
    {
        // 1. Ask the database for everything this page needs
        double total = await _database.GetGrandTotalAsync();
        List<Expense> expenses = await _database.GetExpensesAsync();
        List<CategoryTotal> categories = await _database.GetCategoryTotalsAsync();
        Expense? biggest = await _database.GetBiggestExpenseAsync();
        // 2. The three stat cards
        double average = expenses.Count > 0 ? total / expenses.Count : 0;
        TotalLabel.Text = Rand(total);
        CountLabel.Text = expenses.Count.ToString();
        AverageLabel.Text = Rand(average);
        // 3. Work out each category's share (0.0 to 1.0) for the progress bars
        foreach (CategoryTotal category in categories)
        {
            category.Share = total > 0 ? category.Total / total : 0;
        }
        BindableLayout.SetItemsSource(CategoryList, categories);
        // 4. Biggest expense, or the empty message when there is no data
        bool hasData = biggest is not null;
        EmptyLabel.IsVisible = !hasData;
        BiggestTitle.IsVisible = hasData;
        BiggestCard.IsVisible = hasData;
        CategoryTitle.IsVisible = hasData;
        BiggestCard.BindingContext = biggest;
    }
    // Formats a number as rands, e.g. R 1,234.50
    private static string Rand(double value)
    {
        return "R " + value.ToString("N2", CultureInfo.InvariantCulture);
    }
}