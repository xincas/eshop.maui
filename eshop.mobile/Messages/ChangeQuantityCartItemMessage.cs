using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Eshop.Mobile.Messeges;

public class ChangeQuantityCartItemMessage : ValueChangedMessage<long>
{
    public ChangeQuantityCartItemMessage(long value) : base(value)
    {
    }
}