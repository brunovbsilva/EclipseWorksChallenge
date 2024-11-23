using Newtonsoft.Json;

namespace Domain.Entities
{
    public class Log : BaseEntity
    {
        protected Log() { }
        public string Type { get; init; }
        public string? ObjectFrom { get; init; }
        public string? ObjectTo { get; init; }
        public DateTime Date { get; init; }
        public Guid UserId { get; init; }
        public virtual User User { get; init; }
        public static class Factory
        {
            public static Log Create(Guid userId, string Type, object? from, object? to) => new Log
            {
                Type = Type,
                ObjectFrom = from == null ? null : JsonConvert.SerializeObject(from),
                ObjectTo = to == null ? null : JsonConvert.SerializeObject(to),
                Date = DateTime.Now,
                UserId = userId
            };
        }
    }
}
