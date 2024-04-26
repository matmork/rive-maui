namespace Example.Pages;

public partial class ControlPlaybackPage : ContentPage
{
    public ControlPlaybackPage()
    {
        InitializeComponent();
    }

    private void ControlPlaybackPage_OnUnloaded(object? sender, EventArgs e)
    {
        RivePlayer.Handler?.DisconnectHandler();
    }
}