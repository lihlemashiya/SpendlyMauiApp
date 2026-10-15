using SQLite;
using Spendly.Models;


namespace Spendly.Data;

public class ExpenseDatabase
{
    private SQLiteAsyncConnection? _database;

    // Connect once, create the table, and add sample data on the very first run
    private async Task Init()
    {
        if (_database is not null)
            return;
        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await _database.CreateTableAsync<Expense>();

        int rows = await _database.Table<Expense>().CountAsync();
        if (rows == 0)
            await AddSampleDataAsync();
    }
    // All expenses, newest first
    public async Task<List<Expense>> GetExpensesAsync()
    { 
        await Init();
        return await _database!.Table<Expense>()
            .OrderByDescending(e => e.Date)
            .ThenByDescending(e => e.Id)
            .ToListAsync();
    }

    //The single most expensive expense (null if the table is empty)
    public async Task<Expense?> GetBiggestExpenseAsync()
    {
        await Init();
        return await _database!.Table<Expense>()
            .OrderByDescending(e => e.Amount)
            .FirstOrDefaultAsync();
    }
    //Raw SQL: one row per category with its total and count
    public async Task<List<CategoryTotal>> GetCategoryTotalsAsync()
    {
        await Init();
        return await _database!.QueryAsync<CategoryTotal>(
            "SELECT Category, SUM(Amount) AS Total, COUNT(*) AS Count " +
            "FROM expenses GROUP BY Category ORDER BY Total DESC");

    }
    //Raw SQL: the total of every expense (0 if there are none)
    public async Task<double> GetGrandTotalAsync()
    {
        await Init();
        return await _database!.ExecuteScalarAsync<double>(
            "SELECT IFNULL(SUM(Amount), 0) FROM expenses");
    }

    //--------- create + update -----------
    //Id 0 = a new expense (INSERT). Any other Id = an existing one (UPDATE).
    public async Task<int> SaveExpenseAsync(Expense expense)
    {
        await Init();
        if (expense.Id != 0)
            return await _database!.UpdateAsync(expense);
        else
            return await _database!.InsertAsync(expense);
    }
    //--------DELETE--------
    public async Task<int> DeleteExpenseAsync(Expense expense)
    {
        await Init();
        return await _database!.DeleteAsync(expense);
    }

    //-------SAMPLE DATA------
    private async Task AddSampleDataAsync()
    {
        DateTime today = DateTime.Today;

        var samples = new List<Expense>
        {
            new Expense {Title = "Campus Café Lunch", Amount = 65.00, Category = "Food", Date = today},
            new Expense {Title ="Taxi to campus", Amount = 28.00, Category = "Transport", Date = today},
            new Expense {Title = "Data bundle 2GB", Amount = 149.00, Category = "Airtime", Date = today.AddDays(-1)},
            new Expense {Title = "Printing notes", Amount = 42.50, Category = "Study", Date = today.AddDays(-2)},
            new Expense {Title = "Movie night", Amount = 95.00, Category = "Fun", Date = today.AddDays(-3)},
            new Expense {Title = "Groceries", Amount = 312.40, Category = "Food", Date = today.AddDays(-4)}
        };

        await _database!.InsertAllAsync(samples);

    }

    }










