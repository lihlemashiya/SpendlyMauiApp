namespace Spendly;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        // Always use the light theme, so our colours look right
        // even if the phone is in dark mode.
        UserAppTheme = AppTheme.Light;
    }
    // .NET 9 / .NET 10 template. (In .NET 8 the template uses: MainPage = new AppShell();)
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}