using Borlay.Protocol;
using Borlay.Repositories;
using Borlay.Repositories.RocksDb;
using Borlay.Repositories.Services;
using Borlay.Serialization.Converters;
using RocksDbSharp;
using System;
using System.IO;

namespace Borlay.Serialization.Converters
{
    public static class SerializerServiceExtensions
    {
        public static PrimaryRepositoryService<T> CreatePrimaryService<T>(this ISerializer serializer, string path, bool databaseSync = false) where T : class, IEntity
        {
            path = Path.Combine(path, "primary");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var entityName = typeof(T).Name;
            var dbOptions = new DbOptions();
            dbOptions.SetCreateIfMissing();
            var rocksDb = RocksDbSharp.RocksDb.Open(dbOptions, $@"{path}\{entityName}");

            var repository = new RocksPrimaryRepository(rocksDb, entityName);
            repository.WriteOptions.SetSync(databaseSync);

            var service = new PrimaryRepositoryService<T>(repository, serializer);
            return service;
        }

        public static SecondaryRepositoryService<T> CreateSecondaryService<T>(this ISerializer serializer, string path, bool databaseSync = false) where T : class, IEntity
        {
            path = Path.Combine(path, "secondary");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var entityName = typeof(T).Name;
            var dbOptions = new DbOptions();
            dbOptions.SetCreateIfMissing();
            var rocksDb = RocksDbSharp.RocksDb.Open(dbOptions, $@"{path}\{entityName}");

            var repository = new RocksSecondaryRepository(rocksDb, entityName);
            repository.WriteOptions.SetSync(databaseSync);

            var service = new SecondaryRepositoryService<T>(repository, serializer);
            return service;
        }
    }
}
