using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EquiMarket.Data;
using EquiMarket.Models;

namespace EquiMarket.Controllers
{
    public class HorseController : Controller
    {
        private readonly ApplicationDbContext _context;


        public HorseController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Alla annonser
        [Route("ads")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Horses.ToListAsync());
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

            return View(horseModel);
        }

        // GET: Horse/Create
        [Route("add-new")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lägg till annons
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("add-new")]
        public async Task<IActionResult> Create(HorseCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Skapa en ny HorseModel från ViewModel
                var horse = new HorseModel
                {
                    Name = model.Name,
                    BirthDate = model.BirthDate,
                    Gender = model.Gender,
                    Breed = model.Breed,
                    AdType = model.AdType,
                    Discipline = model.Discipline,
                    Dam = model.Dam,
                    Sire = model.Sire,
                    DamSire = model.DamSire,
                    GrandSire = model.GrandSire,
                    Title = model.Title,
                    Content = model.Content,
                    Price = model.Price
                };

                // Hantera bilduppladdning om en bild har laddats upp
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    // Skapa ett unikt filnamn
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);

                    // Definiera sökvägen
                    string filePath = Path.Combine("wwwroot/images/horses", fileName);

                    // Spara filen
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    // Spara sökvägen i databasen (relativ sökväg)
                    horse.ImagePath = "/images/horses/" + fileName;
                }

                _context.Horses.Add(horse);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // GET: Horse/Edit/{id}
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var horse = await _context.Horses.FindAsync(id);
            if (horse == null)
            {
                return NotFound();
            }

            var viewModel = new HorseEditViewModel
            {
                Id = horse.Id,
                Name = horse.Name,
                BirthDate = horse.BirthDate,
                Gender = horse.Gender,
                Breed = horse.Breed,
                AdType = horse.AdType,
                Discipline = horse.Discipline,
                Dam = horse.Dam,
                Sire = horse.Sire,
                DamSire = horse.DamSire,
                GrandSire = horse.GrandSire,
                Title = horse.Title,
                Content = horse.Content,
                Price = horse.Price
            };

            return View(viewModel);
        }


        // Spara redigerad annons
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id, HorseEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var horse = await _context.Horses.FindAsync(id);
            if (horse == null)
            {
                return NotFound();
            }

            try
            {
                // Uppdatera egenskaper från modellen
                horse.Name = model.Name;
                horse.BirthDate = model.BirthDate;
                horse.Gender = model.Gender;
                horse.Breed = model.Breed;
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
        public async Task<IActionResult> Delete(int? id)
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

            return View(horseModel);
        }

        //  Radera annons
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horseModel = await _context.Horses.FindAsync(id);
            if (horseModel != null)
            {
                _context.Horses.Remove(horseModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorseModelExists(int id)
        {
            return _context.Horses.Any(e => e.Id == id);
        }
    }
}
