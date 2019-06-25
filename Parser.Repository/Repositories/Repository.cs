using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.DAL.Entities;
using Parser.DAL;

namespace Parser.Repository.Repositories
{
    public class Repository
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
