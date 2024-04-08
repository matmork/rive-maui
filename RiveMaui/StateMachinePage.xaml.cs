namespace RiveMaui;

public partial class StateMachinePage : ContentPage
{
    public StateMachinePage()
    {
        InitializeComponent();
    }

    private void HandsUp_OnToggled(object? sender, ToggledEventArgs e)
    {
        RiveAnimation.SetInput("Login Machine", "isHandsUp", e.Value);
    }

    private void Checking_OnToggled(object? sender, ToggledEventArgs e)
    {
        RiveAnimation.SetInput("Login Machine", "isChecking", e.Value);
    }

    private void Success_OnClicked(object? sender, EventArgs e)
    {
        RiveAnimation.TriggerInput("Login Machine", "trigSuccess");
    }

    private void Failure_OnClicked(object? sender, EventArgs e)
    {
        RiveAnimation.TriggerInput("Login Machine", "trigFail");
    }

    private void Slider_OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        RiveAnimation.SetInput("Login Machine", "numLook", (float)e.NewValue);
    }
}