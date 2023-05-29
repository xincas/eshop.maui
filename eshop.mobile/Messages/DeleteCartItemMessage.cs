using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Eshop.Mobile.Messeges;

public class DeleteCartItemMessage : ValueChangedMessage<long>
{
    public DeleteCartItemMessage(long value) : base(value)
    {
    }
}