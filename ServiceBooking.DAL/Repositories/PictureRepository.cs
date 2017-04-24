using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.DAL.Repositories
{
    public class PictureRepository : IRepository<Picture>
    {
        public ApplicationContext Db { get; set; }

        public PictureRepository(ApplicationContext context)
        {
            Db = context;
        }

        public IEnumerable<Picture> GetAll() => Db.Pictures;

        public Picture Get(int id) => Db.Pictures.Find(id);

        public void Create(Picture picture) => Db.Pictures.Add(picture);

        public void Update(Picture picture)
            => Db.Entry(picture).State = EntityState.Modified;

        public IEnumerable<Picture> Find(Func<Picture, bool> predicate) 
            => Db.Pictures.Where(predicate).ToList();

        public void Delete(int id)
        {
            Picture picture = Db.Pictures.Find(id);
            if (!ReferenceEquals(picture, null))
                Db.Pictures.Remove(picture);
        }
    }
}
