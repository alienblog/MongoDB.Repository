using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    public abstract class Entity : IEntity
    {
        [BsonId]
        public string Id
        {
            get
            {
                if (_id == ObjectId.Empty)
                    _id = ObjectId.GenerateNewId(DateTime.Now);
                return _id.ToString();
            }
            set
            {
                ObjectId.TryParse(value, out __id);
                if (__id != ObjectId.Empty)
                    _id = __id; ;
            }
        }
        private ObjectId _id;
        private ObjectId __id;


        public void Save()
        {
            this.DBSave();
            //EntityOperationExtensions.DBSave(this.GetType(), this);
        }

        public void Remove()
        {
            this.DBRemove();
        }

        public MongoDBRef ToDBRef()
        {
            return new MongoDBRef(this.GetType().Name, this.Id);
        }

        public override string ToString()
        {
            return this.ToJson();
        }
    }

    public class Entity<T> : Entity where T:IEntity
    {
        public static T Find(string id)
        {
            return EntityOperationExtensions.DBFind<T>(id);
        }

        public static T Find(Expression<Func<T, bool>> condition)
        {
            return EntityOperationExtensions.DBFind(condition);
        }

        public static bool Remove(string id)
        {
            return EntityOperationExtensions.DBRemove<T>(id);
        }

        public static long RemoveAll(Expression<Func<T, bool>> condition)
        {
            return EntityOperationExtensions.DBRemoveAll(condition);
        }

        public static IQueryable<T> Select(Expression<Func<T, bool>> condition)
        {
            return EntityOperationExtensions.DBSelect(condition);
        }

        public static IQueryable<T> Select(Expression<Func<T, bool>> condition, Expression<Func<T, object>> orderby,
            int pageIndex, int pageSize, out int pageCount, out int allCount)
        {
            return EntityOperationExtensions.DBSelect(condition, @orderby, pageIndex, pageSize, out pageCount,
                out allCount);
        }

        public static IEnumerable<T> FindAll()
        {
            return EntityOperationExtensions.DBFindAll<T>();
        }

        public static long Count()
        {
            return EntityOperationExtensions.Count<T>();
        }

        public static long Count(Expression<Func<T, bool>> condition)
        {
            return EntityOperationExtensions.Count(condition);
        }
    }
}
