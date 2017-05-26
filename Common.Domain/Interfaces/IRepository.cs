﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface IRepository : IDisposable
    {
        IRepository<T2> NewInstance<T2>() where T2 : class;
        int ExecuteCommand(string storedProcedure, object parameters = null, CommandType commandType = CommandType.StoredProcedure);
        IEnumerable<dynamic> ExecuteDynamicQuery(string commandText, object parameters = null, CommandType commandType = CommandType.StoredProcedure, bool MARS = false);
        void Commit();

        Task<List<T2>> ToListAsync<T2>(IQueryable<T2> source);
        Task<int> CountAsync<T2>(IQueryable<T2> source);
        Task<decimal> SumAsync<T2>(IQueryable<T2> source, Expression<Func<T2, decimal>> selector);
        Task<T2> SingleOrDefaultAsync<T2>(IQueryable<T2> source);
        Task<int> CommitAsync();
    }

    public interface IRepository<T> : IRepository, IDisposable where T : class
    {
        void LazyLoadingEnabled(bool Enabled);
        void Undo();
        T Get(params object[] keyValues);
        IQueryable<T> GetAllAsTracking();
        IQueryable<T> GetAllAsTracking(params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAllAsTracking(string path);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAll(string path);
        bool Delete(T model);
        bool DeleteRange(IEnumerable<T> model);
        T Add(T model);
        IEnumerable<T> AddRange(IEnumerable<T> models);
        T Update(T modelNew, T modelOld);
        void BulkCopy(IEnumerable<T> models);

    }
}
