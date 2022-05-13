namespace FPTS.FIT.BDRD.BuildingBlocks.SharedKernel.Interfaces
{
    public interface IKeyValuePairRepository<T, TId> where T : class, IAggregateRoot
    {
        void Add(TId id, T t);
        bool Exist(TId id);
        T Get(TId id);
        void Clear();
    }
}
