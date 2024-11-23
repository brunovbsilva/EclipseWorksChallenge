using Domain.Entities;

namespace Domain.Tests.Mocks
{
    public class LogMock : BaseEntityMock<Log>
    {
        public override Log GetEntity()
        {
            return Log.Factory.Create(
                Guid.NewGuid(),
                _faker.Lorem.Paragraph(),
                _faker.Date.Past(),
                _faker.Date.Future()
            );
        }
    }
}
