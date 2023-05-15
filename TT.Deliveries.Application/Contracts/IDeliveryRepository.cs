using TT.Deliveries.Domain;

namespace TT.Deliveries.Application.Contracts
{
    public interface IDeliveryRepository
    {
        public Task<Guid> CreateAsync(Delivery delivery);
        public Task UpdateAsync(Delivery delivery, List<DeliveryState> lstDeliveryStateFilters);
        public Task DeleteAsync(string orderNumber);
        public Task<List<Delivery>> GetByStateAsync(DeliveryState state);
    }
}
