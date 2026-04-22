using System.Collections.Generic;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb.Services
{
    public class FavoritesService
    {
        private FavoritesManager _manager = new FavoritesManager();

        public List<object> GetFavorites(List<string> favoriteIds)
        {
            if (favoriteIds == null || favoriteIds.Count == 0)
                return new List<object>();

            return _manager.GetFavoriteProducts(favoriteIds);
        }

        public decimal GetFinalPrice(decimal price, int sale)
        {
            return _manager.GetDiscountPrice(price, sale);
        }
    }
}