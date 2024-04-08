namespace Rive.Maui;

public class Rive : View
{
    public static readonly BindableProperty ArtboardNameProperty = BindableProperty.Create(
        nameof(ArtboardName),
        typeof(string),
        typeof(Rive),
        defaultValue: null,
        propertyChanged: OnArtboardNameChanged
    );

    public static readonly BindableProperty AnimationNameProperty = BindableProperty.Create(
        nameof(AnimationName),
        typeof(string),
        typeof(Rive),
        defaultValue: null,
        propertyChanged: OnAnimationNameChanged
    );

    public static readonly BindableProperty StateMachineNameProperty = BindableProperty.Create(
        nameof(StateMachineName),
        typeof(string),
        typeof(Rive),
        defaultValue: null,
        propertyChanged: OnStateMachineNameChanged
    );

    public string? ArtboardName
    {
        get => (string?)GetValue(ArtboardNameProperty);
        set => SetValue(ArtboardNameProperty, value);
    }

    public string? AnimationName
    {
        get => (string?)GetValue(AnimationNameProperty);
        set => SetValue(AnimationNameProperty, value);
    }

    public string? StateMachineName
    {
        get => (string?)GetValue(StateMachineNameProperty);
        set => SetValue(StateMachineNameProperty, value);
    }

    public required string ResourceName { get; init; }

    public bool Autoplay { get; set; } = true;

    private static void OnArtboardNameChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is Rive { Handler: RiveRenderer renderer })
        {
            // TODO: Call platform method in renderer to update
        }
    }

    private static void OnAnimationNameChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is Rive { Handler: RiveRenderer renderer })
        {
            // TODO: Call platform method in renderer to update
        }
    }

    private static void OnStateMachineNameChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is Rive { Handler: RiveRenderer renderer })
        {
            // TODO: Call platform method in renderer to update
        }
    }
}