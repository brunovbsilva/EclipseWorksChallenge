using Domain.Entities;

namespace Domain.Tests.Mocks
{
    public class ProjectMock : BaseEntityMock<Project>
    {
        public override Project GetEntity() => Project.Factory.Create();
    }
}
