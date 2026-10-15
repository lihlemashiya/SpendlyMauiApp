using Spendly.Data;
using Spendly.Models;
using System.Globalization;
namespace Spendly.Views;

public partial class ExpensesPage : ContentPage
{
    private readonly ExpenseDatabase _database;
    // MAUI passes in the shared ExpenseDatabase (registered in MauiProgram.cs)
    public ExpensesPage(ExpenseDatabase database)
    {
        InitializeComponent();
        
    _database = database;
    }
    // Runs the first time the page shows AND every time we come back to it
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadExpensesAsync();
    }
    // READ: get every expense and fill the list and the summary card
    private async Task LoadExpensesAsync()
    {
        List<Expense> expenses = await _database.GetExpensesAsync();
        ExpensesCollection.ItemsSource = expenses;
        // Only this month's expenses count towards the summary card
        DateTime today = DateTime.Today;
        List<Expense> thisMonth = expenses
        .Where(e => e.Date.Year == today.Year && e.Date.Month == today.Month)
        .ToList();
        double monthTotal = thisMonth.Sum(e => e.Amount);
        MonthTotalLabel.Text = "R " + monthTotal.ToString("N2", CultureInfo.InvariantCulture);
        MonthCountLabel.Text = $"{thisMonth.Count} expenses in {today:MMMM}";
    }
    // CREATE: open the form with a brand-new expense (Id = 0)
    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ExpenseDetailPage(_database, new Expense()));
    }
    // UPDATE: open the form with the expense that was tapped
    private async void OnExpenseTapped(object sender, TappedEventArgs e)
    {
        if (sender is Border card && card.BindingContext is Expense expense)
        {
            await Navigation.PushAsync(new ExpenseDetailPage(_database, expense));
        }
    }
    // DELETE: swipe left on a card, confirm, delete, reload
    private async void OnDeleteInvoked(object sender, EventArgs e)
    {
        var expense = (Expense)((SwipeItem)sender).BindingContext;
        bool sure = await DisplayAlert("Delete expense",
        $"Delete \"{expense.Title}\" ({expense.AmountText})?", "Delete", "Cancel");
        if (!sure)
            return;
        await _database.DeleteExpenseAsync(expense);
        await LoadExpensesAsync();
    }
}