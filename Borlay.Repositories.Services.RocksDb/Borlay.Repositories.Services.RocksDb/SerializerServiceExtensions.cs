using Borlay.Protocol;
using Borlay.Repositories;
using Borlay.Repositories.RocksDb;
using Borlay.Repositories.Services;
using RocksDbSharp;
using System;
using System.IO;

namespace Borlay.Serialization
{
    public static class SerializerServiceExtensions
    {
        public static PrimaryRepositoryService<T> CreatePrimaryService<T>(this ISerializer serializer, string path, bool databaseSync = false) where T : class, IEntity
        {
            var entityName = typeof(T).Name;

            path = Path.Combine(path, "primary");
            path = Path.Combine(path, entityName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var dbOptions = new DbOptions();
            dbOptions.SetCreateIfMissing();
            var rocksDb = RocksDbSharp.RocksDb.Open(dbOptions, path);

            var repository = new RocksPrimaryRepository(rocksDb, entityName);
            repository.WriteOptions.SetSync(databaseSync);

            var service = new PrimaryRepositoryService<T>(repository, serializer);
            return service;
        }

        public static SecondaryRepositoryService<T> CreateSecondaryService<T>(this ISerializer serializer, string path, bool databaseSync = false) where T : class, IEntity
        {
            var entityName = typeof(T).Name;

            path = Path.Combine(path, "secondary");
            path = Path.Combine(path, entityName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var dbOptions = new DbOptions();
            dbOptions.SetCreateIfMissing();
            var rocksDb = RocksDbSharp.RocksDb.Open(dbOptions, path);

            var repository = new RocksSecondaryRepository(rocksDb, entityName);
            repository.WriteOptions.SetSync(databaseSync);

            var service = new SecondaryRepositoryService<T>(repository, serializer);
            return service;
        }
    }
}
