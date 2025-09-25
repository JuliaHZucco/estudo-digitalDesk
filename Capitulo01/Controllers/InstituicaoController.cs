using Capitulo01.Data;
using Capitulo01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Capitulo01.Controllers
{
    public class InstituicaoController : Controller
    {
        private readonly IESContext _context;

        public InstituicaoController(IESContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IActionResult> Index()
        {
            var instituicoes = await _context.Instituicoes.OrderBy(i => i.Nome).ToListAsync();
            return View(instituicoes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instituicao instituicao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instituicao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instituicao);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();
            var instituicao = await _context.Instituicoes.FindAsync(id);
            if (instituicao == null) return NotFound();
            return View(instituicao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Instituicao instituicao)
        {
            if (id != instituicao.InstituicaoID) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(instituicao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instituicao);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            var instituicao = await _context.Instituicoes.FindAsync(id);
            if (instituicao == null) return NotFound();
            return View(instituicao);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();
            var instituicao = await _context.Instituicoes.FindAsync(id);
            if (instituicao == null) return NotFound();
            return View(instituicao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var instituicao = await _context.Instituicoes.FindAsync(id);
            if (instituicao != null)
            {
                _context.Instituicoes.Remove(instituicao);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Instituição " + instituicao.Nome.ToUpper() + " foi removida";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
