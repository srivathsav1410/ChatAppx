﻿using System;
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
            optionsBuilder.UseSqlServer("");

            return new ChatAppDbContext(optionsBuilder.Options);
        }
    }
}
