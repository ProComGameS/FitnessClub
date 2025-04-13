using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FitnessClub.DAL
{
    /// <summary>
    /// Інтерфейс репозиторію для абстрагування CRUD операцій.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
