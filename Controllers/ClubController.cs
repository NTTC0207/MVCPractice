using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVCTutorial.Data;
using MVCTutorial.Interfaces;
using MVCTutorial.Models;
using MVCTutorial.ViewModel;
using System.Security.Claims;

namespace MVCTutorial.Controllers
{
    public class ClubController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly ApplicationDbContext _context;
        private readonly IClubRepository _club;
        private readonly IHttpContextAccessor _contextAccessor;
        public ClubController(ApplicationDbContext context,IClubRepository club, IPhotoService photoService, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _club = club;
            _photoService = photoService;
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index()
        {

            IEnumerable<Club> club=  await _club.GetAll(); 
      

            return View(club);
        }

        public async Task<IActionResult> Detail(int id)
        {
            /*   var detail = await _context.Clubs.Where(a=> a.Id == id).SingleOrDefaultAsync();*/
            
            var details = await _club.GetByIdAsync(id);// this has slightly better performance

            return View(details);
        }

        public  IActionResult Create()
        {
            var curUserId = _contextAccessor.HttpContext?.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel
            {
                AppUserId = curUserId
            };



            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create (CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);
                    var club = new Club
                    {
                        Title = clubVM.Title,
                        Description = clubVM.Description,
                        Image = result.Url.ToString(),
                        AppUserId = clubVM.AppUserId,
                        Address = new Address
                        {
                            City = clubVM.Address.City,
                            State = clubVM.Address.State,
                            Street = clubVM.Address.Street
                        }
                    };
                _club.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
          
            return View(clubVM);

        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _club.GetByIdAsync(id);
            if(club == null) { return View("Error"); }
            var clubVM = new EditClubViewModel
            {
               Title = club.Title,
               Description = club.Description,
               AddressId= club.AddressId,
               Address= club.Address,
               URL = club.Image,
               ClubCategory = club.ClubCategory,
            };

            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit");
                return View(clubVM);
            }

            var userClub = await _club.GetByIdAsyncNoTracking(id);

            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete the image");
                    return View(clubVM);
                }

                var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address,
                };

                _club.Update(club);

                return RedirectToAction("");
            }
            else
            {
                return View(clubVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var clubs = await _club.GetByIdAsync(id);

            if(clubs == null)
            {
                return View("Error");
            }
         
                return View(clubs);
           
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var club = await _club.GetByIdAsync(id);
            if(club == null)
            {
                return View("Error");
            }

            _club.Delete(club);
            return RedirectToAction("Index");

        }


    }
}
