namespace CutelCaptureThePhone.Web.Bruteforce
{
    public class AntiBruteforceStore
    {
        private readonly SemaphoreSlim _lock = new(1, 1);

        private readonly List<BruteforceLog> _store = new();

        public void LogFailedAttempt(string key)
        {
            try
            {
                _lock.Wait();
                
                _store.Add(new BruteforceLog
                {
                    Key = key,
                    Created = DateTime.UtcNow
                });
            }
            finally
            {
                _lock.Release();
            }
        }

        public bool IsBlocked(string key)
        {
            try
            {
                _lock.Wait();

                DateTime fifteenMinutesAgo = DateTime.UtcNow - TimeSpan.FromMinutes(15);

                //Clean up expired attempts
                _store.RemoveAll(l => l.Created < fifteenMinutesAgo);

                //If there are 3 or more failed attempts within the last 15 minutes, the key is blocked
                return _store.Count(l => l.Key == key && l.Created >= fifteenMinutesAgo) >= 3;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}