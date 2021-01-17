using System;
namespace DistroCache
{
    public interface ICacheItem
    {
        public string Id { get; set; }
    }

    public class CacheItem : ICacheItem
    {
        public virtual string Id { get; set; }
    }
}
