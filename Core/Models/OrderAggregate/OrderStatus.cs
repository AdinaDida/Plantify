using System.Runtime.Serialization;

namespace Core.Models.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Payment Received")]
        PaymentReceived,

        [EnumMember(Value = "Payment Failed")]
        PaymentFailed,

        [EnumMember(Value = "Confirmed")]
        Confirmed,

        [EnumMember(Value = "Shipped")]
        Shipped,
    }
}
