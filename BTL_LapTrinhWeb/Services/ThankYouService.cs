using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb.Services
{
    public class ThankYouService
    {
        private ThankYouManager _manager = new ThankYouManager();

        public Order GetOrder(string orderCode, string username)
        {
            return _manager.GetLastOrder(orderCode, username);
        }
    }
}