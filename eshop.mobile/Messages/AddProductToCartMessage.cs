using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Eshop.Mobile.Messeges;

public class AddProductToCartMessage : ValueChangedMessage<long>
{
    public AddProductToCartMessage(long value) : base(value)
    {
    }
}