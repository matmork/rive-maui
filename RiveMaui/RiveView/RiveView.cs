namespace RiveMaui;

public class RiveView : View
{
    public void Load(string animation)
    {
        if (Handler is RiveViewRenderer renderer)
        {
            renderer.Load(animation);
        }
    }
}