using LiteDB;
using Silmoon.Data.MongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Data.LiteDB
{
    public interface ILiteDBService
    {
        ILiteDatabase Database { get; set; }

        void Add<T>(T obj) where T : IIdObject;
        void Adds<T>(T[] objs) where T : IIdObject;

        T[] Gets<T>(Expression<Func<T, bool>> whereFunc, int? offset = null, int? count = null) where T : IIdObject;
        T[] Gets<T, TOrderKey>(Expression<Func<T, bool>> whereFunc, Expression<Func<T, TOrderKey>> orderFunc, bool? ascending, int? offset = null, int? count = null) where T : IIdObject;
        ILiteQueryableResult<T> GetsQuery<T>(Expression<Func<T, bool>> whereFunc, int? offset = null, int? count = null) where T : IIdObject;
        ILiteQueryableResult<T> GetsQuery<T, TOrderKey>(Expression<Func<T, bool>> whereFunc, Expression<Func<T, TOrderKey>> orderFunc, bool? ascending, int? offset = null, int? count = null) where T : IIdObject;

        T Get<T>(Expression<Func<T, bool>> whereFunc) where T : IIdObject;
        T Get<T, TOrderKey>(Expression<Func<T, bool>> whereFunc, Expression<Func<T, TOrderKey>> orderFunc, bool? ascending) where T : IIdObject;

        bool Set<T>(T obj) where T : IIdObject;
        int Sets<T>(IEnumerable<T> objs) where T : IIdObject;
        int Sets<T>(Expression<Func<T, T>> obj, Expression<Func<T, bool>> whereFunc = null) where T : IIdObject;

        int Deletes<T>(Expression<Func<T, bool>> whereFunc) where T : IIdObject;
    }
}
