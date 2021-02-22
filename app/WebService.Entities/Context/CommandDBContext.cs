using System;
using Microsoft.EntityFrameworkCore;

namespace WebService.Entities.Context
{
    public partial class CommandDBContext : DbContext
    {
        public CommandDBContext(DbContextOptions<CommandDBContext> options)
            : base(options)
        {
        }
    }
}
