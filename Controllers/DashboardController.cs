using Microsoft.AspNetCore.Mvc;
using MVCTutorial.Data;
using MVCTutorial.Interfaces;
using MVCTutorial.ViewModel;

namespace MVCTutorial.Controllers
{
    public class DashboardController : Controller
    {
      
        private readonly IDashboardRepository _dashboard;
        public DashboardController(IDashboardRepository dashboard)
        {
            _dashboard = dashboard;
        }

        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboard.GetAllUserRaces();
            var userClubs = await _dashboard.GetAllUserClubs();

            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs,
            };
            return View(dashboardViewModel);
        }





    }
}
