using System;
using System.Diagnostics;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace MongoDB.Repository
{
    public class DBClient : IDBClient
    {
        private MongoClient _client;
        private string _dbName;
        Type _type;
        public DBClient(MongoUrl url, Type type)
        {
            _dbName = url.DatabaseName;
            _type = type;
            _client = new MongoClient(url);
        }

        public DBClient(MongoUrl url)
        {
            _dbName = url.DatabaseName;
            _client = new MongoClient(url);
        }

        /// <summary>
        /// database name
        /// </summary>
        public string DBName
        {
            get
            {
                return _dbName;
            }
        }
        /// <summary>
        /// MongoCollection
        /// </summary>
        public MongoCollection Collection
        {
            get
            {
                if (_type == null)
                {
                    throw new NotSupportedException("Wrong Init Function，If you want use this property,you should instantiated by DBClient(MongoUrl,Type)");
                }
                return _client.GetServer().GetDatabase(DBName).GetCollection(_type.Name);
            }
        }

        public MongoGridFS GridFs
        {
            get
            {
                return _client.GetServer().GetDatabase(DBName).GridFS;
            }
        }

        #region 资源回收
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Close()
        {
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_mDisposed)
            {
                if (disposing)
                {
                    _client = null;
                    _type = null;
                    _dbName = null;
                }
                // Release unmanaged resources
                _mDisposed = true;
            }
        }
        ~DBClient()
        {
            Dispose(false);
        }

        private bool _mDisposed;
        #endregion
    }
}
