using Newtonsoft.Json;

namespace Domain.Entities
{
    public class Log : BaseEntity
    {
        protected Log() { }
        string Type { get; init; }
        string ObjectFrom { get; init; }
        string ObjectTo { get; init; }
        DateTime Date { get; init; }
        Guid UserId { get; init; }
        public static class Factory
        {
            public static Log Create(string Type, object from, object to) => new Log
            {
                Type = Type,
                ObjectFrom = JsonConvert.SerializeObject(from),
                ObjectTo = JsonConvert.SerializeObject(to),
                Date = DateTime.Now,
            };
        }
    }
}
