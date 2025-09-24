using Capitulo01.Data;
using Capitulo01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Capitulo01.Controllers
{
    public class DepartamentoController : Controller
    {
        private readonly IESContext _context;

        public DepartamentoController(IESContext context)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> Index()
        {
            var departamentos = await _context.Departamentos
                                              .Include(d => d.Instituicao)
                                              .OrderBy(d => d.Nome)
                                              .ToListAsync();
            return View(departamentos);
        }

        public async Task<IActionResult> Create()
        {
            await PopularInstituicoesDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,InstituicaoID")] Departamento departamento)
        {
            if (!departamento.InstituicaoID.HasValue || departamento.InstituicaoID == 0)
                ModelState.AddModelError("InstituicaoID", "Selecione uma instituição válida.");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState inválido:");
                foreach (var key in ModelState.Keys)
                    foreach (var error in ModelState[key].Errors)
                        Console.WriteLine($"{key}: {error.ErrorMessage}");

                await PopularInstituicoesDropDown(departamento.InstituicaoID);
                return View(departamento);
            }

            _context.Add(departamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null) return NotFound();

            await PopularInstituicoesDropDown(departamento.InstituicaoID);
            return View(departamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("DepartamentoID,Nome,InstituicaoID")] Departamento departamento)
        {
            if (id != departamento.DepartamentoID) return NotFound();

            if (!departamento.InstituicaoID.HasValue || departamento.InstituicaoID == 0)
                ModelState.AddModelError("InstituicaoID", "Selecione uma instituição válida.");

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(departamento);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Departamentos.Any(d => d.DepartamentoID == id)) return NotFound();
                    else throw;
                }
            }

            await PopularInstituicoesDropDown(departamento.InstituicaoID);
            return View(departamento);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var departamento = await _context.Departamentos
                                             .Include(d => d.Instituicao)
                                             .FirstOrDefaultAsync(d => d.DepartamentoID == id);

            if (departamento == null) return NotFound();
            return View(departamento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento != null)
            {
                _context.Departamentos.Remove(departamento);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task PopularInstituicoesDropDown(object selectedId = null)
        {
            var instituicoes = await _context.Instituicoes.OrderBy(i => i.Nome).ToListAsync();
            instituicoes.Insert(0, new Instituicao { InstituicaoID = 0, Nome = "Selecione a instituição" });
            ViewBag.Instituicoes = new SelectList(instituicoes, "InstituicaoID", "Nome", selectedId);
        }
    }
}
