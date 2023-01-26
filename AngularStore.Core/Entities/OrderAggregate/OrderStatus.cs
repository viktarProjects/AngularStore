using System.Runtime.Serialization;

namespace AngularStore.Core.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Panding")]
        Pending,

        [EnumMember(Value = "Payment received")]
        PaymentReceived,

        [EnumMember(Value = "Payment failed")]
        PaymentFailed,

        [EnumMember(Value = "Your order has been sent")]
        Sent,

        [EnumMember(Value = "Completed")]
        Completed
    }
}
