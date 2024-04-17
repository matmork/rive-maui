namespace Example.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void StateMachine_OnClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new StateMachinePage());
    }

    private void TouchInput_OnClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new TouchInputPage());
    }

    private void ControlPlayback_OnClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new ControlPlaybackPage());
    }
}