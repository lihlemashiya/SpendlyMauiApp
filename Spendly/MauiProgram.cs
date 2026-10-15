using Microsoft.Extensions.Logging;
using Spendly.Data;
using Spendly.Views;

namespace Spendly
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            //ONE database object for the whole app(one connection to the file)
            builder.Services.AddSingleton<ExpenseDatabase>();
            builder.Services.AddSingleton<ExpensesPage>();
            builder.Services.AddTransient<InsightsPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
