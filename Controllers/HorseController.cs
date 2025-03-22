using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EquiMarket.Data;
using EquiMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace EquiMarket.Controllers
{
    public class HorseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; //För att koppla ihop med användare


        public HorseController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        // GET: Alla annonser
        [Route("ads")]
        public async Task<IActionResult> Index(HorseSearchViewModel model, int pageNumber = 1, int pageSize = 10)
        {
            //Om null, skicka bara tom lista
            if (model == null)
            {
                return View(new List<HorseModel>());
            }

            //Hämta alla annonser
            var query = _context.Horses.AsQueryable();

            //Filtrering på titel
            if (!string.IsNullOrEmpty(model.Title))
            {
                query = query.Where(h => h.Title!.Contains(model.Title));
            }

            //Minimiålder
            if (model.MinAge.HasValue)
            {
                query = query.Where(h => h.BirthDate <= DateTime.Today.AddYears(-model.MinAge.Value));
            }

            //Maxålder
            if (model.MaxAge.HasValue)
            {
                query = query.Where(h => h.BirthDate >= DateTime.Today.AddYears(-model.MaxAge.Value));
            }

            //Kön
            if (model.Gender.HasValue)
            {
                query = query.Where(h => h.Gender == model.Gender.Value);
            }

            //Ras
            if (!string.IsNullOrEmpty(model.Breed))
            {
                query = query.Where(h => h.Breed!.Contains(model.Breed));
            }

            //Minhöjd
            if (model.MinHeight.HasValue)
            {
                query = query.Where(h => h.Height >= model.MinHeight.Value);
            }

            //Maxhöjd
            if (model.MaxHeight.HasValue)
            {
                query = query.Where(h => h.Height <= model.MaxHeight.Value);
            }

            //Köpes eller säljes
            if (model.AdType.HasValue)
            {
                query = query.Where(h => h.AdType == model.AdType.Value);
            }

            //Disciplin
            if (!string.IsNullOrEmpty(model.Discipline))
            {
                query = query.Where(h => h.Discipline!.Contains(model.Discipline));
            }

            //Minpris
            if (model.MinPrice.HasValue)
            {
                query = query.Where(h => h.Price >= model.MinPrice.Value);
            }

            //Maxpris
            if (model.MaxPrice.HasValue)
            {
                query = query.Where(h => h.Price <= model.MaxPrice.Value);
            }

            //Räkna hästar och hämta sida av resultat
            int totalHorses = await query.CountAsync();
            var horses = await query
                .OrderByDescending(h => h.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new HorseListViewModel
            {
                SearchModel = model,
                Horses = horses,
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(totalHorses / (double)pageSize)
            };
            return View(viewModel);
        }

        // GET: Enskild annons
        [Route("ads/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horseModel = await _context.Horses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horseModel == null)
            {
                return NotFound();
            }

            // Hämta säljarens information
            var seller = await _userManager.FindByIdAsync(horseModel.UserId!);
            ViewBag.Seller = seller;

            return View(horseModel);
        }

        [Authorize]
        // GET: Horse/Create
        [Route("add-new")]
        public IActionResult Create()
        {
            //Hitta användarens id
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            //Skapa ny instans och skicka med ID till vyn
            var model = new HorseCreateViewModel
            {
                UserId = userId,
                BirthDate = DateTime.Today
            };

            return View(model);
        }

        // POST: Lägg till annons
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("add-new")]
        [Authorize]
        public async Task<IActionResult> Create(HorseCreateViewModel model)
        {
            //Hämta användarens ID
            var userId = _userManager.GetUserId(User);

            //Kolla så ID från formuläret stämmer med inloggad användare
            if (model.UserId != userId)
            {
                return Unauthorized();
            }

            //Validering har gått bra
            if (ModelState.IsValid)
            {

                //Skapa ny hästmodell av inkommande data
                var horse = new HorseModel
                {
                    UserId = userId,
                    Name = model.Name,
                    BirthDate = model.BirthDate,
                    Gender = model.Gender,
                    Breed = model.Breed,
                    Height = model.Height,
                    AdType = model.AdType,
                    Discipline = model.Discipline,
                    Dam = model.Dam,
                    Sire = model.Sire,
                    DamSire = model.DamSire,
                    GrandSire = model.GrandSire,
                    Title = model.Title,
                    Content = model.Content,
                    Price = model.Price,
                    CreatedDate = DateTime.Now
                }
            ;

                //Kolla om bild har laddats upp
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    //Skapa nytt filnamn
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);

                    //Skapa sökväg
                    string filePath = Path.Combine("wwwroot/images/horses", fileName);

                    //Spara filen
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    //Spara sökvägen 
                    horse.ImagePath = "/images/horses/" + fileName;
                }

                //Lägg till häst i databas
                _context.Horses.Add(horse);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            //Stanna kvar på vyn annars
            return View(model);
        }


        // GET: Horse/Edit/{id}
        [Route("edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var horse = await _context.Horses.FindAsync(id);
            if (horse == null)
            {
                return NotFound();
            }

            //Kolla inloggad användare
            var userId = _userManager.GetUserId(User);
            //Om det inte är samma som är inloggad får hen inte ändra
            if (horse.UserId != userId)
            {
                return Forbid();
            }

            var viewModel = new HorseEditViewModel
            {
                Id = horse.Id,
                Name = horse.Name,
                BirthDate = horse.BirthDate,
                Gender = horse.Gender,
                Breed = horse.Breed,
                Height = horse.Height,
                AdType = horse.AdType,
                Discipline = horse.Discipline,
                Dam = horse.Dam,
                Sire = horse.Sire,
                DamSire = horse.DamSire,
                GrandSire = horse.GrandSire,
                Title = horse.Title,
                Content = horse.Content,
                HasImage = !string.IsNullOrEmpty(horse.ImagePath),
                Price = horse.Price
            };

            return View(viewModel);
        }


        // Spara redigerad annons
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(int id, HorseEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            //Hitta häst att redigera
            var horse = await _context.Horses.FindAsync(id);
            if (horse == null)
            {
                return NotFound();
            }

            //Kolla användare
            var userId = _userManager.GetUserId(User);
            if (horse.UserId != userId)
            {
                return Forbid();
            }


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Uppdatera egenskaper från modellen
                horse.Name = model.Name;
                horse.BirthDate = model.BirthDate;
                horse.Gender = model.Gender;
                horse.Breed = model.Breed;
                horse.Height = model.Height;
                horse.AdType = model.AdType;
                horse.Discipline = model.Discipline;
                horse.Dam = model.Dam;
                horse.Sire = model.Sire;
                horse.DamSire = model.DamSire;
                horse.GrandSire = model.GrandSire;
                horse.Title = model.Title;
                horse.Content = model.Content;
                horse.Price = model.Price;

                // Hantera bilduppladdning
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    // Skapa unikt filnamn
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    string filePath = Path.Combine("wwwroot/images/horses", fileName);

                    // Spara filen
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    // Ta bort gammal bild om den finns
                    if (!string.IsNullOrEmpty(horse.ImagePath))
                    {
                        string oldFilePath = Path.Combine("wwwroot", horse.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Spara den nya sökvägen
                    horse.ImagePath = "/images/horses/" + fileName;
                }

                // Uppdatera databasen
                _context.Update(horse);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorseModelExists(horse.Id))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError("", "Det uppstod ett fel vid uppdatering av databasen. Försök igen.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ett oväntat fel inträffade: " + ex.Message);
            }

            return View(model);
        }



        // GET: annons för radering
        [Route("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horse = await _context.Horses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horse == null)
            {
                return NotFound();
            }

            //Användare
            var userId = _userManager.GetUserId(User);
            if (horse.UserId != userId)
            {
                return Forbid();
            }

            return View(horse);
        }

        //  Radera annons
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horse = await _context.Horses.FindAsync(id);

            if (horse == null)
            {
                return NotFound();
            }

            //Användare
            var userId = _userManager.GetUserId(User);
            if (horse.UserId != userId)
            {
                return Forbid();
            }

            //Spara
            _context.Horses.Remove(horse);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorseModelExists(int id)
        {
            return _context.Horses.Any(e => e.Id == id);
        }
    }
}
