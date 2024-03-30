namespace RiveMaui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        Button.IsVisible = false;
        RiveView.Load("runner");
    }
}