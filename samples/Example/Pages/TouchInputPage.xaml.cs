namespace Example.Pages;

public partial class TouchInputPage : ContentPage
{
    public TouchInputPage()
    {
        InitializeComponent();
    }

    private void TouchInputPage_OnUnloaded(object? sender, EventArgs e)
    {
        RivePlayer.Handler?.DisconnectHandler();
    }
}