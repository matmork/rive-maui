namespace Example.Pages;

public partial class StateMachinePage : ContentPage
{
    public StateMachinePage()
    {
        InitializeComponent();
    }

    private void StateMachinePage_OnUnloaded(object? sender, EventArgs e)
    {
        RivePlayer.Handler?.DisconnectHandler();
    }
}