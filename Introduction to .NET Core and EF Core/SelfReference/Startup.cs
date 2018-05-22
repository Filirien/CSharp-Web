using System;

namespace SelfReference
{
   public class Startup
    {
        public static void Main()
        {
            var db = new MyDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
