using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Concrete
{
    public class EFGameRepository : IGameRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Game> Games
        {
            get { return context.Games; }
        }
        public void SaveGame(Game game)
        {
            if(game.GameId == 0)
                context.Games.Add(game);
            else
            {
                Game dbEntry = context.Games.Find(game.GameId);
                if(dbEntry != null)
                {
                    dbEntry.Name = game.Name;
                    dbEntry.Description = game.Description;
                    dbEntry.Price = game.Price;
                    dbEntry.Category = game.Category;
                }
            }
            context.SaveChanges();
        }

        public Game DeleteGame(int gameId)
        {
            Game dbEntry = context.Games.Find(gameId);
            if (dbEntry != null)
            {
                context.Games.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
