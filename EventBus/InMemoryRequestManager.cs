using FPTS.FIT.BDRD.BuildingBlocks.EventBus.Exceptions;
using System.Collections.Concurrent;

namespace FPTS.FIT.BDRD.BuildingBlocks.EventBus
#nullable disable
{
    public class InMemoryRequestManager : IRequestManager<string>
    {
        private static ConcurrentDictionary<string, object> _store = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// generate ra 1 Guid string
        /// </summary>
        /// <returns></returns>
        public string GenernateRequestId()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gán dữ liệu response vào trong ConcurrentDictionary với requestId
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="response"></param>
        public void SetResponse(string requestId, object response)
        {
            _store.TryAdd(requestId, response);
        }


        /// <summary>
        /// Loop chờ đến khi dữ liệu trong ConcurrentDictionary với key là requestid được gán.
        /// Sau 1 khoảng thời gian millisecondsTimeout mà chưa được gán thì dừng loop.
        /// Nếu được gán thì sẽ xóa khỏi ConcurrentDictionary và trả về dữ liệu ứng vói reuqestId. 
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public async Task<object> GetResponseAsync(string requestId, int millisecondsTimeout = 8000)
        {
            if (millisecondsTimeout < 500)
            {
                throw new EventBusException(nameof(millisecondsTimeout) + " can not less than 500ms");
            }
            var task = Task.Run(() =>
            {
                while (!_store.ContainsKey(requestId))
                {
                }
                return 1;
            });
            var result = await Task.WhenAny(task, Task.Delay(millisecondsTimeout));
            if (result == task)
            {
                _store.Remove(requestId, out object response);
                return response;
            }
            return null;
        }
    }
}
