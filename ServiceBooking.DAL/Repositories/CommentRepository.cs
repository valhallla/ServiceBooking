using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.DAL.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        public ApplicationContext Db { get; set; }

        public CommentRepository(ApplicationContext context)
        {
            Db = context;
        }

        public IEnumerable<Comment> GetAll() => Db.Comments;

        public Comment Get(int id) => Db.Comments.Find(id);

        public void Create(Comment comment) => Db.Comments.Add(comment);

        public void Update(Comment comment)
            => Db.Entry(comment).State = EntityState.Modified;

        public IEnumerable<Comment> Find(Func<Comment, bool> predicate)
            => Db.Comments.Where(predicate).ToList();
        
        public void Delete(int id)
        {
            Comment comment = Db.Comments.Find(id);
            if (!ReferenceEquals(comment, null))
                Db.Comments.Remove(comment);
        }
    }
}
