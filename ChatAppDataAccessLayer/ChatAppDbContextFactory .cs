using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ChatAppDataAccessLayer
{
    public class ChatAppDbContextFactory : IDesignTimeDbContextFactory<ChatAppDbContext>
    {
        public ChatAppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatAppDbContext>();
            optionsBuilder.UseSqlServer("\"Server=tcp:chatappx.database.windows.net,1433;Initial Catalog=chatapp;Persist Security Info=False;User ID=chatappadmin;Password=Vaishu@1432;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;\"");

            return new ChatAppDbContext(optionsBuilder.Options);
        }
    }
}
