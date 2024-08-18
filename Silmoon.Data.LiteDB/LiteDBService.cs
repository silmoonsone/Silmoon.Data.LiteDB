using LiteDB;
using Silmoon.Data.MongoDB;
using Silmoon.Data.MongoDB.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Data.LiteDB
{
    public class LiteDBService : ILiteDBService, IDisposable
    {
        public ILiteDatabase Database { get; set; }

        public LiteDBService()
        {

        }
        public LiteDBService(string connectionString) => Database = new LiteDatabase(connectionString);
        public LiteDBService(ILiteDatabase liteDatabase) => Database = liteDatabase;

        public ILiteCollection<T> GetCollection<T>() where T : IIdObject => Database.GetCollection<T>(MongoService.MakeTableName<T>());

        public void Add<T>(T obj) where T : IIdObject
        {
            if (obj is not null) GetCollection<T>().Insert(obj);
        }

        public void Adds<T>(T[] objs) where T : IIdObject
        {
            if (objs is not null) GetCollection<T>().Insert(objs);
        }


        public int Deletes<T>(Expression<Func<T, bool>> whereFunc) where T : IIdObject => GetCollection<T>().DeleteMany(whereFunc);

        public T Get<T>(Expression<Func<T, bool>> whereFunc) where T : IIdObject => GetCollection<T>().FindOne(whereFunc);

        public T Get<T, TOrderKey>(Expression<Func<T, bool>> whereFunc, Expression<Func<T, TOrderKey>> orderFunc, bool? ascending) where T : IIdObject => GetsQuery(whereFunc, orderFunc, ascending).FirstOrDefault();

        public T[] Gets<T>(Expression<Func<T, bool>> whereFunc, int? offset = null, int? count = null) where T : IIdObject => GetsQuery(whereFunc, offset, count).ToArray();

        public T[] Gets<T, TOrderKey>(Expression<Func<T, bool>> whereFunc, Expression<Func<T, TOrderKey>> orderFunc, bool? ascending, int? offset = null, int? count = null) where T : IIdObject => GetsQuery(whereFunc, orderFunc, ascending, offset, count).ToArray();

        public ILiteQueryableResult<T> GetsQuery<T>(Expression<Func<T, bool>> whereFunc, int? offset = null, int? count = null) where T : IIdObject
        {
            var query = GetCollection<T>().Query();
            query = query.Where(whereFunc);
            ILiteQueryableResult<T> result = query;
            if (offset.HasValue) result = query.Skip(offset.Value);
            if (count.HasValue) result = query.Limit(count.Value);
            return result;
        }

        public ILiteQueryableResult<T> GetsQuery<T, TOrderKey>(Expression<Func<T, bool>> whereFunc, Expression<Func<T, TOrderKey>> orderFunc, bool? ascending, int? offset = null, int? count = null) where T : IIdObject
        {
            var query = GetCollection<T>().Query();
            query = query.Where(whereFunc);
            if (ascending ?? true) query = query.OrderBy(orderFunc);
            else query = query.OrderByDescending(orderFunc);
            ILiteQueryableResult<T> result = query;
            if (offset.HasValue) result = query.Skip(offset.Value);
            if (count.HasValue) result = query.Limit(count.Value);
            return result;
        }

        public bool Set<T>(T obj) where T : IIdObject => GetCollection<T>().Update(obj);

        public int Sets<T>(IEnumerable<T> objs) where T : IIdObject => GetCollection<T>().Update(objs);

        public int Sets<T>(Expression<Func<T, T>> obj, Expression<Func<T, bool>> whereFunc = null) where T : IIdObject => GetCollection<T>().UpdateMany(obj, whereFunc);

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
