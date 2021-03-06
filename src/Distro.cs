﻿using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace DistroCache
{
    public class Distro : IDistro
    {
        public IDistributedCache DistributedCache { get; }

        private readonly DistributedCacheEntryOptions entryOptions;


        public Distro(IDistributedCache cache, IOptions<DistributedCacheEntryOptions> options)
        {
            DistributedCache = cache;
            entryOptions = options.Value;
        }


        public T Find<T>(string arrayKey, string id) where T : CacheItem => List<T>(arrayKey).FirstOrDefault(c => c.Id == id);

        public T Update<T>(string arrayKey, T newItem) where T : CacheItem
        {
            var existingItems = List<T>(arrayKey);

            if (existingItems is not null && existingItems.Count > 0)
            {
                for (int i = 0; i < existingItems.Count; i++)
                {
                    if (existingItems[i].Id == newItem.Id)
                    {
                        existingItems[i] = newItem;
                        break;
                    }
                }
            }

            Replace<T>(arrayKey, existingItems);

            return newItem;
        }

       
        public void AddRange<T>(string arrayKey, IEnumerable<T> items) where T : CacheItem
        {
            if (items?.Count() > 0)
            {
                var existingItems = List<T>(arrayKey);

                if (existingItems is null || existingItems.Count < 1)
                {
                    existingItems = new List<T>(items);
                }

                Replace<T>(arrayKey, existingItems);
            }
        }

        public void Replace<T>(string arrayKey, IEnumerable<T> newItems) where T : CacheItem
        {
            var json = JsonSerializer.Serialize(newItems);

            DistributedCache.SetString(arrayKey, json, entryOptions);
        }

        public List<T> List<T>(string arrayKey) where T : CacheItem
        {
            if (string.IsNullOrWhiteSpace(arrayKey))
            {
                return new List<T> { };
            }

            var json = DistributedCache.GetString(arrayKey);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<T> { };
            }

            return JsonSerializer.Deserialize<List<T>>(json);
        }

        public void Remove<T>(string arrayKey, string id) where T : CacheItem
        {
            var existingItems = List<T>(arrayKey);

            if (existingItems?.Count > 0)
            {
                for (int i = 0; i < existingItems.Count; i++)
                {
                    if (existingItems[i].Id == id)
                    {
                        existingItems.RemoveAt(i);
                        break;
                    }
                }

                Replace<T>(arrayKey, existingItems);
            }
        }

        public void Remove<T>(string arrayKey, T item) where T : CacheItem => Remove<T>(arrayKey, item.Id);

        public void RemoveAt<T>(string arrayKey, int index) where T : CacheItem
        {
            var existingItems = List<T>(arrayKey);

            if (existingItems?.Count > 0)
            {
                existingItems.RemoveAt(index);

                Replace<T>(arrayKey, existingItems);
            }
        }

        public void Remove(string key) => DistributedCache.Remove(key);


        public T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return default;
            }

            var json = DistributedCache.GetString(key);

            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            var response = JsonSerializer.Deserialize<T>(json);

            return response;
        }

        public T Set<T>(string key, T item, DistributedCacheEntryOptions entryOptions = default)
        {
            var json = (typeof(T) == typeof(string)) ? item as string : JsonSerializer.Serialize(item);

            DistributedCache.SetString(key, json, entryOptions is not null && entryOptions != default ? entryOptions : this.entryOptions);

            return item;
        }


        public async ValueTask<T> FindAsync<T>(string arrayKey, string id) where T : CacheItem
            => (await ListAsync<T>(arrayKey)).Find(c => c.Id == id);

        public async ValueTask UpdateAsync<T>(string arrayKey, T newItem) where T : CacheItem
        {
            var existingItems = await ListAsync<T>(arrayKey);

            if (existingItems is not null && existingItems.Count > 0)
            {
                for (int i = 0; i < existingItems.Count; i++)
                {
                    if (existingItems[i].Id == newItem.Id)
                    {
                        existingItems[i] = newItem;
                        break;
                    }
                }
            }

            await ReplaceAsync<T>(arrayKey, existingItems);
        }

        public async ValueTask AddRangeAsync<T>(string arrayKey, IEnumerable<T> items) where T : CacheItem
        {
            if (items?.Count() > 0)
            {
                var existingItems = await ListAsync<T>(arrayKey);

                if (existingItems is null || existingItems.Count < 1)
                {
                    existingItems = new List<T>(items);
                }

                await ReplaceAsync<T>(arrayKey, existingItems);
            }
        }

        public async ValueTask ReplaceAsync<T>(string arrayKey, IEnumerable<T> newItems) where T : CacheItem
        {
            Replace<T>(arrayKey, newItems);

            await Task.CompletedTask;
        }

        public async ValueTask<List<T>> ListAsync<T>(string arrayKey) where T : CacheItem
        {
            if (string.IsNullOrWhiteSpace(arrayKey))
            {
                return new List<T> { };
            }

            var json = DistributedCache.GetString(arrayKey);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<T> { };
            }

            return await Task.FromResult(JsonSerializer.Deserialize<List<T>>(json));
        }

        public async ValueTask RemoveAsync<T>(string arrayKey, string id) where T : CacheItem
        {
            var existingItems = await ListAsync<T>(arrayKey);

            if (existingItems?.Count > 0)
            {
                for (int i = 0; i < existingItems.Count; i++)
                {
                    if (existingItems[i].Id == id)
                    {
                        existingItems.RemoveAt(i);
                        break;
                    }
                }

                await ReplaceAsync<T>(arrayKey, existingItems);
            }
        }

        public async ValueTask RemoveAsync<T>(string arrayKey, T item) where T : CacheItem => await RemoveAsync<T>(arrayKey, item.Id);

        public async ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default) =>
            await DistributedCache.RemoveAsync(key, cancellationToken);


        public async ValueTask<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return default;
            }

            var json = await DistributedCache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(json);
        }

        public async ValueTask<T> SetAsync<T>(string key, T item, DistributedCacheEntryOptions entryOptions = default, CancellationToken cancellationToken = default)
        {
            var json = (typeof(T) == typeof(string)) ? item as string : JsonSerializer.Serialize(item);

            await DistributedCache.SetStringAsync(key, json, entryOptions is not null && entryOptions != default ? entryOptions : this.entryOptions, cancellationToken);

            return item;
        }
    }
}
