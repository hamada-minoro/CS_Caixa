using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CS_Caixa.Repositorios
{
    public class RepositorioBase<TEntity> where TEntity : class
    {

        protected CS_CAIXA_DBContext Db = new CS_CAIXA_DBContext();

        public TEntity Adicionar(TEntity obj)
        {
            Db = new CS_CAIXA_DBContext();
            Db.Set<TEntity>().Add(obj);
            Db.SaveChanges();
            return obj;
        }

        public TEntity ObterPorId(int id)
        {
            Db = new CS_CAIXA_DBContext();
            return Db.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> ObterTodos()
        {
            Db = new CS_CAIXA_DBContext();
            return Db.Set<TEntity>().ToList();
        }

        public TEntity Update(TEntity obj)
        {

            Db = new CS_CAIXA_DBContext();
            Db.Entry(obj).State = EntityState.Modified;
            Db.SaveChanges();
            return obj;

        }

        public void Remove(TEntity obj)
        {
            Db.Set<TEntity>().Remove(obj);
            Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}
