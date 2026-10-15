namespace Spendly.Helpers;
// Every category the app knows, and the colour that goes with each one.
public static class CategoryHelper
{
    // Used by the Picker on the expense form
    public static List<string> Categories { get; } = new()
 {
 "Food", "Transport", "Study", "Airtime", "Fun", "Other"
 };
    public static Color ColorFor(string category)
    {
        return category switch
        {

            "Food" => Color.FromArgb("#F97316"), // orange
            "Transport" => Color.FromArgb("#0EA5E9"), // sky blue
            "Study" => Color.FromArgb("#8B5CF6"), // violet
            "Airtime" => Color.FromArgb("#10B981"), // green
            "Fun" => Color.FromArgb("#EC4899"), // pink
            _ => Color.FromArgb("#64748B") // slate (Other)
        };
    }
}