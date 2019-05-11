using Borlay.Protocol;
using Borlay.Repositories;
using Borlay.Repositories.RocksDb;
using Borlay.Repositories.Services;
using Borlay.Serialization;
using RocksDbSharp;
using System;
using System.IO;

namespace Borlay.Protocol
{
    public static class ProtocolServiceExtensions
    {
        public static PrimaryRepositoryService<T> RegisterPrimaryService<T>(this ProtocolHost host, string path, bool databaseSync = false) where T : class, IEntity
        {
            var service = host.Serializer.CreatePrimaryService<T>(path, databaseSync);
            host.RegisterHandler(service, true);
            return service;
        }

        public static SecondaryRepositoryService<T> RegisterSecondaryService<T>(this ProtocolHost host, string path, bool databaseSync = false) where T : class, IEntity
        {
            var service = host.Serializer.CreateSecondaryService<T>(path, databaseSync);
            host.RegisterHandler(service, true);
            return service;
        }
    }
}
