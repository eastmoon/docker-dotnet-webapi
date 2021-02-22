using System;
using Microsoft.EntityFrameworkCore;

namespace WebService.Entities.Context
{
    public partial class QueryDBContext : DbContext
    {
        public QueryDBContext(DbContextOptions<QueryDBContext> options)
            : base(options)
        {
        }
    }
}
