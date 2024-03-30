namespace RiveMaui;

public class RiveView : ContentView
{
    public void Load(string animation)
    {
        if (Handler is RiveViewHandler handler)
        {
            handler.Load(animation);
        }
    }
}