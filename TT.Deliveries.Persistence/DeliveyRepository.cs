using MongoDB.Driver;
using TT.Deliveries.Application.Contracts;
using TT.Deliveries.Domain;

namespace TT.Deliveries.Persistence
{
    public class DeliveyRepository : IDeliveryRepository
    {
        private readonly IMongoCollection<Delivery> _deliveries;

        public DeliveyRepository(IMongoClient client)
        {
            var database = client.GetDatabase("MyDb");
            var collection = database.GetCollection<Delivery>(nameof(Delivery));

            _deliveries = collection;
        }

        public async Task<Guid> CreateAsync(Delivery delivery)
        {
            await _deliveries.InsertOneAsync(delivery);

            return delivery.Id;
        }
        public async Task UpdateAsync(Delivery delivery, List<DeliveryState> lstDeliveryStateFilters)
        {
            var filter = Builders<Delivery>.Filter.Eq(c => c.Order.OrderNumber, delivery.Order.OrderNumber);

            foreach (var deliveryStateFilter in lstDeliveryStateFilters)
            {
                filter &= Builders<Delivery>.Filter.Eq(s=>s.State, deliveryStateFilter);
            }

            var update = Builders<Delivery>.Update
                .Set(c => c.State, delivery.State);           

            var result = await _deliveries.UpdateManyAsync(filter, update);
        }

        public async Task DeleteAsync(string orderNumber)
        {
            var filter = Builders<Delivery>.Filter.Eq( c=>c.Order.OrderNumber, orderNumber);
            await _deliveries.DeleteManyAsync(filter);
        }
        public async Task<List<Delivery>> GetByStateAsync(DeliveryState state)
        {
            return await _deliveries.Find(r => r.State == state).ToListAsync();  
        }

    }
}