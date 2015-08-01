using Standard;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Standard
{
    public class DB
    {
        public static WebLib.Models.fwUser CurrentUser
        { get { return WebLib.DAL.fwUserDAL.GetCurrentUser(); } }
        public static DB_9CF750_dbEntities Entities
        {
            get
            {
                return new DB_9CF750_dbEntities();
            }
        }
        public class BaseClass<T> where T : class
        {
            public DB_9CF750_dbEntities _db;

            public BaseClass()
            {
                _db = new DB_9CF750_dbEntities();
            }

            public T GetById(object id)
            {
                return _db.Set<T>().Find(id);
            }

            public T GetByUserName(object name)
            {
                return _db.Set<T>().Find(name);
            }

            public void Insert(T obj)
            {
                _db.Set<T>().Add(obj);
                _db.SaveChanges();
            }

            public void Update(T obj)
            {
                _db.Set<T>().Attach(obj);
                _db.Entry(obj).State = EntityState.Modified;
                _db.SaveChanges();
            }

            public void Delete(object id)
            {
                T obj = _db.Set<T>().Find(id);
                _db.Set<T>().Remove(obj);
                _db.SaveChanges();
            }

            public void Delete(T entity)
            {
                _db.Set<T>().Remove(entity);
                _db.SaveChanges();
            }
        }
    }

    public class BaseEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public BaseEntity()
        {
        }

        public BaseEntity(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}