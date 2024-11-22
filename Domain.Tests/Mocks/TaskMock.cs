using Bogus.DataSets;

namespace Domain.Tests.Mocks
{
    public class TaskMock : BaseEntityMock<Domain.Entities.Task>
    {
        public override Domain.Entities.Task GetEntity()
            => Domain.Entities.Task.Factory.Create(
                _faker.Lorem.Lines(1),
                _faker.Lorem.Lines(3),
                _faker.Date.Future(),
                _faker.Random.Enum<TaskStatus>()
            );
    }
}
