namespace Domain.Tests.Mocks
{
    public class BaseEntityMock<T>
    {
        protected Faker _faker = new("pt_BR");
        public virtual T GetEntity()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetEmptyEnumerable()
        {
            return [];
        }

        public IEnumerable<T> GetEnumerableWithEntities(int quantity)
        {
            return Enumerable.Range(0, quantity).Select(_ => GetEntity());
        }
    }
}
