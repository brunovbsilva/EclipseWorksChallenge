using Domain.Entities.Enums;

namespace Domain.Tests.Mocks
{
    public class TaskMock : BaseEntityMock<Domain.Entities.Task>
    {
        public override Domain.Entities.Task GetEntity()
        {
            var title = _faker.Lorem.Sentence();
            var description = _faker.Lorem.Paragraph();
            var dueDate = _faker.Date.Future();
            var status = _faker.Random.Enum<TaskStatusEnum>();
            var priority = _faker.Random.Enum<PriorityEnum>();
            return Domain.Entities.Task.Factory.Create(Guid.NewGuid(), title, description, dueDate, status, priority);
        }
    }
}
