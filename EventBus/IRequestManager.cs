namespace FPTS.FIT.BDRD.BuildingBlocks.EventBus
{
    public interface IRequestManager<TId>
    {
        TId GenernateRequestId();
        Task<object> GetResponseAsync(TId requestId, int millisecondsTimeout = 8000);
        void SetResponse(TId requestId, object response);
    }
}
