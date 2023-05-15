namespace TT.Deliveries.Application.Shared
{
    public class BaseDeliveryEntity
    {
      
        public string State { get; set; }

        public AccessWindow AccessWindow { get; set; }

        public Recipient Recipient { get; set; }

        public Order Order { get; set; }
    }
}
