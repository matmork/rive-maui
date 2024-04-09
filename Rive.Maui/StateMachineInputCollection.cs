using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Rive.Maui;

// Modified version of: https://github.com/jsuarezruiz/rive-maui/blob/main/src/RiveSharp.Views.MAUI/StateMachineInputCollection.cs
// Thank you, @jsuarezruiz

// Manages a collection of StateMachineInput objects for RivePlayer. The [ContentProperty] tag
// on RivePlayer instructs the XAML engine automatically route nested inputs through this
// collection:
//
//   <rive:RivePlayer Source="...">
//       <rive:BoolInput Target=... />
//   </rive:RivePlayer>
//
public sealed class StateMachineInputCollection : ObservableCollection<StateMachineInput>
{
    private readonly WeakReference<Rive?> _rive;

    public StateMachineInputCollection(Rive rive)
    {
        _rive = new WeakReference<Rive?>(rive);
        CollectionChanged += InputsVectorChanged;
    }

    ~StateMachineInputCollection()
    {
        CollectionChanged -= InputsVectorChanged;
    }

    private void InputsVectorChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
            case NotifyCollectionChangedAction.Replace:
            {
                if (sender is ObservableCollection<StateMachineInput> collection)
                {
                    collection[e.NewStartingIndex].SetRive(_rive);
                }

                break;
            }
            case NotifyCollectionChangedAction.Remove:
            {
                if (sender is ObservableCollection<StateMachineInput> collection)
                {
                    collection[e.NewStartingIndex].SetRive(new WeakReference<Rive?>(null));
                }

                break;
            }
            case NotifyCollectionChangedAction.Reset:
            {
                if (sender is ObservableCollection<StateMachineInput> collection)
                {
                    foreach (var input in collection)
                    {
                        input.SetRive(new WeakReference<Rive?>(null));
                    }
                }

                break;
            }
        }
    }
}