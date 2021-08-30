# EventHandler
EventHandler is an event bus library in C#, features as follow.
* A connector between components
* Reduces dependencies
## Usage
`IEventBus` is an interface to publish events, subscribe and unsubscribe actions. `EventBus` implements this interface.
```c#
private void main()
{
    IEventBus eventBus = new EventBus();
    eventBus.Subscribe<OnCustomEvent>(DoAction);
    
    eventBus.Publish(new OnCustomEvent(“Hello”));
    
    eventBus.Unsubscribe<OnCustomEvent>(DoAction);
}

private void DoAction(OnCustomEvent customEvent)
{
    Console.WriteLine(customEvent.Message);
}

public readonly struct OnCustomEvent : IEventBase
{
    public OnCustomEvent(string message)
    {
        Message = message;
    }
    
    string Message { get; }
}
```
Since we subscribe `DoAction` to `OnCustomEvent`, the action will be invoked when the event is published. To avoid memory leaks, the action should be unsubscribed if we won’t need it anymore.
If you subscribe an action with an argument which is derived from `IEventBase`, you can use properties of this struct. On the other hand, you cannot subscribe this action to other events. If you want to subscribe an action to multiple events, the action should contain the argument `IEventBase`, but it will not have any specific properties.
```c#
private void main()
{
    IEventBus eventBus = new EventBus();
    eventBus.Subscribe<OnCustomEvent>(DoAction);
    
    eventBus.Publish(new OnCustomEvent(“Hello”));
    
    eventBus.Unsubscribe<OnCustomEvent>(DoAction);
}

private void DoAction(IEventBase eventBase)
{
    Console.WriteLine(“There are no messages.”);
}
```
You can also subscribe and unsubscribe actions in run time by using `Subscribe(Type type, Action<IEventBase> action)`. Because it is not a generic method, only an action with the argument `IEventBase` can be subscribed.
```c#
private void main()
{
    IEventBus eventBus = new EventBus();
    SubscribeAction(eventBus, typeof(OnCustomEvent));
    
    eventBus.Publish(new OnCustomEvent(“Hello”));
    
    UnsubscribeAction(eventBus, typeof(OnCustomEvent));
}

private void SubscribeAction(IEventBus eventBus, Type type)
{
    eventBus.Subscribe(type, DoAction);
}

private void UnsubscribeAction(IEventBus eventBus, Type type)
{
    eventBus.Unsubscribe(type, DoAction);
}

private void DoAction(IEventBase eventBase)
{
    Console.WriteLine(“There are no messages.”);
}
```
