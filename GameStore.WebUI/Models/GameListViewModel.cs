using System.Collections.Generic;
using GameStore.Domain.Entities;

namespace GameStore.WebUI.Models
{
    public class GameListViewModel
    {
        public IEnumerable<Game> Games { get; set; } 
        public PagingInfo PagingInfo { get; set; }
    }
}