using System;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace MongoDB.Repository
{
    public interface IDBClient : IDisposable
    {
        /// <summary>
        /// database name
        /// </summary>
        string DBName { get; }

        /// <summary>
        /// return MongoCollection
        /// </summary>
        MongoCollection Collection { get; }

        MongoGridFS GridFs { get; }

        /// <summary>
        /// dispose resources
        /// </summary>
        void Close();
    }
}
