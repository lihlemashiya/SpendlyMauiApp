
using Spendly.Data;
using Spendly.Models;
using SQLite;
using System.Globalization;

namespace Spendly.Views;

public partial class ExpenseDetailPage : ContentPage
{
    private readonly ExpenseDatabase _database;
    
 private readonly Expense _expense;
    // The page receives the database AND the expense to show
    public ExpenseDetailPage(ExpenseDatabase database, Expense expense)
    {
        InitializeComponent();
        _database = database;
        _expense = expense;
        // Every {Binding ...} on this page now reads and writes _expense
        BindingContext = _expense;
        if (_expense.Id == 0)
        {
            Title = "New expense";
            DeleteButton.IsVisible = false; // nothing to delete yet
        }
        else
        {
            Title = "Edit expense";
            AmountEntry.Text = _expense.Amount.ToString("0.00", CultureInfo.InvariantCulture);
        }
    }
    // CREATE or UPDATE
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // 1. Check the amount: allow 45.50 or 45,50
        string amountText = (AmountEntry.Text ?? "").Trim().Replace(',', '.');
        bool isNumber = double.TryParse(amountText, NumberStyles.Float,
        CultureInfo.InvariantCulture, out double amount);
        if (!isNumber || amount <= 0)
        {
            await DisplayAlert("Check the amount", "Enter an amount bigger than 0, for example 45.50.",
           "OK");
            return;
        }
        // 2. Check the title
        if (string.IsNullOrWhiteSpace(_expense.Title))
        {
            await DisplayAlert("Add a title", "Say what you bought, for example Lunch.", "OK");
            return;
        }
        // 3. Put the cleaned-up values into the expense
        _expense.Amount = Math.Round(amount, 2);
        _expense.Title = _expense.Title.Trim();
        // 4. Save (INSERT if Id is 0, UPDATE otherwise) and go back
        try
        {
            await _database.SaveExpenseAsync(_expense);
            await Navigation.PopAsync();
        }
        catch (SQLiteException)
        {
            await DisplayAlert("Not saved", "Something went wrong saving this expense. Please try again.",
           "OK");
        }
        
    }
    // DELETE
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool sure = await DisplayAlert("Delete expense",
        $"Delete \"{_expense.Title}\"? This cannot be undone.", "Delete", "Cancel");
        if (!sure)
            return;
        await _database.DeleteExpenseAsync(_expense);
        await Navigation.PopAsync();
    }
}
