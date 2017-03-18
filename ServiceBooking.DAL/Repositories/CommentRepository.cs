using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.DAL.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        private ApplicationContext db;

        public CommentRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public IEnumerable<Comment> GetAll()
        {
            return db.Comments;
        }

        public Comment Get(int id)
        {
            return db.Comments.Find(id);
        }

        public void Create(Comment book)
        {
            db.Comments.Add(book);
        }

        public void Update(Comment book)
        {
            db.Entry(book).State = EntityState.Modified;
        }

        public IEnumerable<Comment> Find(Func<Comment, bool> predicate)
        {
            return db.Comments.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Comment book = db.Comments.Find(id);
            if (book != null)
                db.Comments.Remove(book);
        }
    }
}
