using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCTutorial.Data;
using MVCTutorial.Interfaces;
using MVCTutorial.Models;
using MVCTutorial.Repository;
using MVCTutorial.ViewModel;

namespace MVCTutorial.Controllers
{
    public class RaceController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IRaceRepository _race;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;


        public RaceController(ApplicationDbContext context, IRaceRepository race, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _race = race;
            _context = context;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {

            IEnumerable<Race> races =await _race.GetAll();

            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _race.GetByIdAsync(id);
            return View(race);
        }
        public IActionResult Create(int id)
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createClubViewModel = new CreateRaceViewModel
            {
                AppUserId = curUserId
            };



            return View(createClubViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var race = await _race.GetByIdAsync(id);
            if (race == null) return View("Error");
            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);
                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId = raceVM.AppUserId,
                    Address = new Address
                    {
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                        Street = raceVM.Address.Street
                    }
                };
                _race.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceVM);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit");
                return View(raceVM);
            }

            var userClub = await _race.GetByIdAsyncNoTracking(id);

            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete the image");
                    return View(raceVM);
                }

                var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);

                var race = new Race
                {
                    Id = id,
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = raceVM.AddressId,
                    Address = raceVM.Address,
                };

                _race.Update(race);

                return RedirectToAction("");
            }
            else
            {
                return View(raceVM);
            }
        }


        public async Task<IActionResult> Delete(int id)
        {

            var race = await _race.GetByIdAsync(id);

            if (race == null)
            {
                return View("Error");
            }

            return View(race);

        }


        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var race = await _race.GetByIdAsync(id);
            
            if(race== null)
            {
                return View("Error");
            }

            _race.Delete(race);
            return RedirectToAction("Index");


        }





    }
}
