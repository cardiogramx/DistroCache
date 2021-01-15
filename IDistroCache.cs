using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace DistroCache
{
    public interface IDistroCache
    {
        /// <summary>
        /// Gets the underlying <see cref="IDistributedCache"/> implementation.
        /// </summary>
        public IDistributedCache DistributedCache { get; }

        /// <summary>
        /// Asynchronously updates an item in an existing cache dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayKey"></param>
        /// <param name="newItem"></param>
        /// <returns></returns>
        ValueTask UpdateAsync<T>(string arrayKey, T newItem) where T : CacheItem;

        /// <summary>
        /// Asynchronously finds an item in an existing cache dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayKey"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        ValueTask<T> FindAsync<T>(string arrayKey, string Id) where T : CacheItem;

        /// <summary>
        /// Asynchronously adds an element to cache dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        ValueTask AddAsync<T>(string key, T item) where T : CacheItem;

        /// <summary>
        /// Asynchronously adds elements to cache dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayKey"></param>
        /// <param name="items"></param>
        ValueTask AddRangeAsync<T>(string arrayKey, IEnumerable<T> items) where T : CacheItem;

        /// <summary>
        /// Asynchronously replaces elements in an existing cache dictionary with new elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayKey"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        ValueTask ReplaceAsync<T>(string arrayKey, IEnumerable<T> items) where T : CacheItem;

        /// <summary>
        /// Asynchronously list all items in an existing cache dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        ValueTask<List<T>> ListAsync<T>(string key) where T : CacheItem;

        /// <summary>
        /// Asynchronously removes an item from an existing cache dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayKey"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        ValueTask RemoveAsync<T>(string arrayKey, T item) where T : CacheItem;

        /// <summary>
        /// Asynchronously removes an existing cache dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default);


        /// <summary>
        /// Updates an item in an existing cache dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayKey"></param>
        /// <param name="newItem"></param>
        /// <returns></returns>
        T Update<T>(string arrayKey, T newItem) where T : CacheItem;

        /// <summary>
        /// Removes an item from an existing cache dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayKey"></param>
        /// <param name="newItem"></param>
        void Remove<T>(string arrayKey, T newItem) where T : CacheItem;

        /// <summary>
        /// Removes an existing cache dictionary.
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);


        /// <summary>
        /// Finds an item in an existing cache dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayKey"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        T Find<T>(string arrayKey, string Id) where T : CacheItem;

        /// <summary>
        /// Adds an element to cache dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        void Add<T>(string key, T item) where T : CacheItem;

        /// <summary>
        /// Adds elements to cache dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayKey"></param>
        /// <param name="items"></param>
        void AddRange<T>(string arrayKey, IEnumerable<T> items) where T : CacheItem;

        /// <summary>
        /// Replaces elements in an existing cache dictionary with new elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayKey"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        void Replace<T>(string arrayKey, IEnumerable<T> items) where T : CacheItem;

        /// <summary>
        /// List all items in an existing cache dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> List<T>(string key) where T : CacheItem;
    }
}
