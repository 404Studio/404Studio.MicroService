namespace YH.Etms.Settlement.Api.Domain.Entities
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        //Task<bool> Delete(T model, bool isSoft);
    }
}
