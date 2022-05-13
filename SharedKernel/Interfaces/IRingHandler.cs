using Disruptor;
namespace FPTS.FIT.BDRD.BuildingBlocks.SharedKernel.Interfaces
{
    public interface IRingHandler<T> : IEventHandler<T> where T : class, IRingData
    {
    }
}
