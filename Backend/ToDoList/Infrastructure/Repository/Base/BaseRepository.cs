﻿using Dapper;
using Domain.Entities.Base;
using Domain.Interfaces.Repository.Base;
using Infrastructure.Context;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.Repository.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : EntityBase
    {
        protected readonly string _tableName;

        private readonly TarefaContext _context;
        private readonly IDbConnection _connection;

        public BaseRepository(TarefaContext context, IConfiguration configuration)
        {
            _tableName = typeof(T).Name;
            _context = context;
            _connection = _connection.AddConnection(configuration);
        }

        public async Task Add(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAll()
        {
            string query = $"SELECT * FROM {_tableName}";

            var result = await _connection.QueryAsync<T>(query);

            return result.ToList();
        }

        public async Task<T> GetById(Guid id)
        {
            string query = $"SELECT * FROM {_tableName} WHERE Id = '{id}'";

            var result = await _connection.QueryFirstOrDefaultAsync<T>(query);

            return result;
        }

    }
}