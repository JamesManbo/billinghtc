using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Servers;
using MongoDB.Driver.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MongoClusterRepository
{
    public class MongoClusterCollection<TDocument> : IMongoClusterCollection<TDocument>
    {
        private const int MAX_RETRY_TIMES = 3;
        private const int WAITING_RETRY_MILISECONDS = 1000;
        private IMongoDatabase[] _databases { get; set; }
        private Dictionary<IMongoCollection<TDocument>, int> _collections { get; set; }
        private string CollectionName { get; set; }
        public MongoClusterCollection(IEnumerable<IMongoDatabase> cluster)
        {
            this._databases = cluster.ToArray();
            this._collections = new Dictionary<IMongoCollection<TDocument>, int>();
            this.CollectionName = typeof(TDocument).Name;
            foreach (var database in _databases)
            {
                _collections.Add(database.GetCollection<TDocument>(CollectionName), 0);
            }
        }

        private bool PingMongoServer(IMongoCollection<TDocument> collection)
        {
            var currentState = collection.Database.Client.Cluster.Description.Servers
                .FirstOrDefault()?.State ?? ServerState.Disconnected;
            return currentState == ServerState.Connected;
            //await collection.Database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
        }

        bool IsEnumerableType(Type type)
        {
            return type.GetInterface(nameof(IEnumerable)) != null;
        }

        bool IsCollectionType(Type type)
        {
            return type.GetInterface(nameof(ICollection)) != null;
        }

        public void Cleanup()
        {
            foreach (var conn in _databases)
            {
                conn?.DropCollection(CollectionName);
            }
        }

        protected T ExecuteSingle<T>(Func<IMongoCollection<TDocument>, T> command, int tryTimes = 0)
        {
            var result = default(T);
            foreach (var conn in _collections.OrderByDescending(c => c.Value))
            {
                try
                {
                    if (!PingMongoServer(conn.Key))
                    {
                        _collections[conn.Key]++;
                        if (tryTimes < MAX_RETRY_TIMES)
                        {
                            Task.Delay(WAITING_RETRY_MILISECONDS).Wait();
                            return this.ExecuteSingle(command, ++tryTimes);
                        }
                        continue;
                    }

                    result = command(conn.Key);
                    if (result.Equals(default(T)) && conn.Key != _collections.Keys.Last())
                    {
                        _collections[conn.Key]++;
                        continue;
                    }

                    _collections[conn.Key] = 0;
                    return result;
                }
                catch (Exception)
                {
                    _collections[conn.Key]++;
                    continue;
                }
            }

            return result;
        }
        protected async Task<T> ExecuteSingleAsync<T>(Func<IMongoCollection<TDocument>, Task<T>> command, int tryTimes = 0)
        {
            var result = default(T);
            foreach (var conn in _collections.OrderByDescending(c => c.Value))
            {
                try
                {
                    if (!PingMongoServer(conn.Key))
                    {
                        _collections[conn.Key]++;
                        if(tryTimes < MAX_RETRY_TIMES)
                        {
                            await Task.Delay(WAITING_RETRY_MILISECONDS);
                            return await this.ExecuteSingleAsync(command, ++tryTimes);
                        }
                        continue;
                    }
                    result = await command(conn.Key);

                    if (result.Equals(default(T)) && conn.Key != _collections.Keys.Last())
                    {
                        _collections[conn.Key]++;
                        continue;
                    }

                    _collections[conn.Key] = 0;
                    return result;
                }
                catch (Exception)
                {
                    _collections[conn.Key]++;
                    continue;
                }
            }

            return result;
        }

        protected async Task ExecuteAsync(Func<IMongoCollection<TDocument>, Task> act, int tryTimes = 0)
        {
            foreach (var conn in _collections.OrderByDescending(c => c.Value))
            {
                try
                {
                    if (!PingMongoServer(conn.Key))
                    {
                        _collections[conn.Key]++;

                        if (tryTimes < MAX_RETRY_TIMES)
                        {
                            await Task.Delay(WAITING_RETRY_MILISECONDS);
                            await this.ExecuteAsync(act, ++tryTimes);
                            return;
                        }
                        continue;
                    }
                    await act(conn.Key);
                    _collections[conn.Key] = 0;
                }
                catch (Exception)
                {
                    _collections[conn.Key]++;
                    continue;
                }
            }
        }

        protected void Execute(Action<IMongoCollection<TDocument>> act, int tryTimes = 0)
        {
            foreach (var conn in _collections.OrderByDescending(c => c.Value))
            {
                try
                {
                    if (!PingMongoServer(conn.Key))
                    {
                        _collections[conn.Key]++;

                        if (tryTimes < MAX_RETRY_TIMES)
                        {
                            Task.Delay(WAITING_RETRY_MILISECONDS).Wait();
                            this.Execute(act, ++tryTimes);
                            return;
                        }
                        continue;
                    }
                    act(conn.Key);
                    _collections[conn.Key] = 0;
                }
                catch (Exception)
                {
                    _collections[conn.Key]++;
                    continue;
                }
            }
        }

        public async Task<BulkWriteResult<TDocument>> BulkWriteAsync(IClientSessionHandle session, IEnumerable<WriteModel<TDocument>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
        {
            BulkWriteResult<TDocument> result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.BulkWriteAsync(session, requests, options, cancellationToken);
            });

            return result;
        }

        public async Task<BulkWriteResult<TDocument>> BulkWriteAsync(IEnumerable<WriteModel<TDocument>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
        {
            BulkWriteResult<TDocument> result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.BulkWriteAsync(requests, options, cancellationToken);
            });

            return result;
        }

        public async Task<long> CountDocumentsAsync(FilterDefinition<TDocument> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteSingleAsync(async collection =>
            {
                return await collection.CountDocumentsAsync(filter, options, cancellationToken);
            });
        }

        public async Task<long> CountDocumentsAsync(IClientSessionHandle session, FilterDefinition<TDocument> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteSingleAsync(async collection =>
            {
                return await collection.CountDocumentsAsync(session, filter, options, cancellationToken);
            });
        }

        public long EstimatedDocumentCount(EstimatedDocumentCountOptions options = null, CancellationToken cancellationToken = default)
        {
            return ExecuteSingle(collection =>
            {
                return collection.EstimatedDocumentCount(options, cancellationToken);
            });
        }

        public async Task<long> EstimatedDocumentCountAsync(EstimatedDocumentCountOptions options = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteSingleAsync(async collection =>
            {
                return await collection.EstimatedDocumentCountAsync(options, cancellationToken);
            });
        }

        public async Task<IAsyncCursor<TProjection>> FindAsync<TProjection>(IClientSessionHandle session, FilterDefinition<TDocument> filter, FindOptions<TDocument, TProjection> options = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteSingleAsync(async collection =>
            {
                return await collection.FindAsync(session, filter, options, cancellationToken);
            });
        }

        public async Task<IAsyncCursor<TProjection>> FindAsync<TProjection>(FilterDefinition<TDocument> filter, FindOptions<TDocument, TProjection> options = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteSingleAsync(async collection =>
            {
                return await collection.FindAsync(filter, options, cancellationToken);
            });
        }

        public IAsyncCursor<TProjection> FindSync<TProjection>(FilterDefinition<TDocument> filter, FindOptions<TDocument, TProjection> options = null, CancellationToken cancellationToken = default)
        {
            return ExecuteSingle(collection =>
            {
                return collection.FindSync(filter, options, cancellationToken);
            });
        }

        public IAsyncCursor<TProjection> FindSync<TProjection>(IClientSessionHandle session, FilterDefinition<TDocument> filter, FindOptions<TDocument, TProjection> options = null, CancellationToken cancellationToken = default)
        {
            return ExecuteSingle(collection =>
            {
                return collection.FindSync(session, filter, options, cancellationToken);
            });
        }

        public async Task InsertManyAsync(IEnumerable<TDocument> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(async collection =>
            {
                await collection.InsertManyAsync(documents, options, cancellationToken);
            });
        }

        public async Task InsertManyAsync(IClientSessionHandle session, IEnumerable<TDocument> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(async collection =>
            {
                await collection.InsertManyAsync(session, documents, options, cancellationToken);
            });
        }

        public async Task InsertOneAsync(IClientSessionHandle session, TDocument document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(async collection =>
            {
                await collection.InsertOneAsync(session, document, options, cancellationToken);
            });
        }

        public async Task InsertOneAsync(TDocument document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(async collection =>
            {
                await collection.InsertOneAsync(document, options, cancellationToken);
            });
        }

        public async Task<ReplaceOneResult> ReplaceOneAsync(IClientSessionHandle session, FilterDefinition<TDocument> filter, TDocument replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
        {
            ReplaceOneResult result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.ReplaceOneAsync(session, filter, replacement, options, cancellationToken);
            });

            return result;
        }

        public async Task<ReplaceOneResult> ReplaceOneAsync(FilterDefinition<TDocument> filter, TDocument replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
        {
            ReplaceOneResult result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.ReplaceOneAsync(filter, replacement, options, cancellationToken);
                Console.WriteLine(result);
            });

            return result;
        }
        public async Task<UpdateResult> UpdateManyAsync(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            UpdateResult result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.UpdateManyAsync(filter, update, options, cancellationToken);
            });

            return result;
        }

        public async Task<UpdateResult> UpdateManyAsync(IClientSessionHandle session, FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            UpdateResult result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.UpdateManyAsync(session, filter, update, options, cancellationToken);
            });

            return result;
        }

        public async Task<UpdateResult> UpdateOneAsync(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            UpdateResult result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.UpdateOneAsync(filter, update, options, cancellationToken);
            });

            return result;
        }

        public async Task<UpdateResult> UpdateOneAsync(IClientSessionHandle session, FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            UpdateResult result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.UpdateOneAsync(session, filter, update, options, cancellationToken);
            });

            return result;
        }

        public IMongoQueryable<TDocument> AsQueryable(AggregateOptions aggregateOptions = null)
        {
            return ExecuteSingle(collection =>
            {
                return collection.AsQueryable(aggregateOptions);
            });
        }

        public IMongoQueryable<TDocument> AsQueryable(IClientSessionHandle session, AggregateOptions aggregateOptions = null)
        {
            return ExecuteSingle(collection =>
            {
                return collection.AsQueryable(session, aggregateOptions);
            });
        }

        public async Task<DeleteResult> DeleteManyAsync(FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default)
        {
            DeleteResult result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.DeleteManyAsync(filter, cancellationToken);
            });

            return result;
        }

        public async Task<DeleteResult> DeleteManyAsync(FilterDefinition<TDocument> filter, DeleteOptions options, CancellationToken cancellationToken = default)
        {
            DeleteResult result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.DeleteManyAsync(filter, options, cancellationToken);
            });

            return result;
        }

        public async Task<DeleteResult> DeleteManyAsync(IClientSessionHandle session, FilterDefinition<TDocument> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            DeleteResult result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.DeleteManyAsync(session, filter, options, cancellationToken);
            });

            return result;
        }

        public async Task<DeleteResult> DeleteOneAsync(FilterDefinition<TDocument> filter, DeleteOptions options, CancellationToken cancellationToken = default)
        {
            DeleteResult result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.DeleteOneAsync(filter, options, cancellationToken);
            });

            return result;
        }

        public async Task<DeleteResult> DeleteOneAsync(IClientSessionHandle session, FilterDefinition<TDocument> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            DeleteResult result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.DeleteOneAsync(session, filter, options, cancellationToken);
            });

            return result;
        }

        public async Task<DeleteResult> DeleteOneAsync(FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default)
        {
            DeleteResult result = null;
            await ExecuteAsync(async collection =>
            {
                result = await collection.DeleteOneAsync(filter, cancellationToken);
            });

            return result;
        }

        public IEnumerable<IMongoCollection<TDocument>> GetCollections()
        {
            return this._collections.Select(c => c.Key).ToList();
        }
    }
}
