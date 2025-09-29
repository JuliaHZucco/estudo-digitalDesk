using Capitulo01.Data;
using Capitulo01.Data.DAL.Cadastros;
using Capitulo01.Modelo.Cadastros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;



namespace Capitulo01.Areas.Cadastros.Controllers
{
    [Area("Cadastros")]
    [Authorize]
    public class DepartamentoController : Controller
    {
        private readonly IESContext _context;
        private readonly DepartamentoDAL departamentoDAL;
        private readonly InstituicaoDAL instituicaoDAL;

        public DepartamentoController(IESContext context)
        {
            _context = context;
            departamentoDAL = new DepartamentoDAL(context);
            instituicaoDAL = new InstituicaoDAL(context);
        }

        public async Task<IActionResult> Index()
        {
            return View(await departamentoDAL.ObterDepartamentosClassificadosPorNome().ToListAsync());
        }

        public IActionResult Create()
        {
            var instituicoes = instituicaoDAL.ObterInstituicoesClassificadasPorNome().ToList();
            instituicoes.Insert(0, new Instituicao { InstituicaoID = 0, Nome = "Selecione a instituição" });
            ViewBag.Instituicoes = new SelectList(instituicoes, "InstituicaoID", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,InstituicaoID")] Departamento departamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await departamentoDAL.GravarDepartamento(departamento);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }

            var instituicoes = instituicaoDAL.ObterInstituicoesClassificadasPorNome().ToList();
            instituicoes.Insert(0, new Instituicao { InstituicaoID = 0, Nome = "Selecione a instituição" });
            ViewBag.Instituicoes = new SelectList(instituicoes, "InstituicaoID", "Nome", departamento.InstituicaoID);

            return View(departamento);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var departamento = await departamentoDAL.ObterDepartamentoPorId((long)id);
            if (departamento == null) return NotFound();

            var instituicoes = instituicaoDAL.ObterInstituicoesClassificadasPorNome().ToList();
            instituicoes.Insert(0, new Instituicao { InstituicaoID = 0, Nome = "Selecione a instituição" });
            ViewBag.Instituicoes = new SelectList(instituicoes, "InstituicaoID", "Nome", departamento.InstituicaoID);

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
                var instituicoes = instituicaoDAL.ObterInstituicoesClassificadasPorNome().ToList();
                instituicoes.Insert(0, new Instituicao { InstituicaoID = 0, Nome = "Selecione a instituição" });
                ViewBag.Instituicoes = new SelectList(instituicoes, "InstituicaoID", "Nome", departamento.InstituicaoID);

                return View(departamento);
            }

            try
            {
                await departamentoDAL.GravarDepartamento(departamento);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DepartamentoExists(departamento.DepartamentoID))
                    return NotFound();
                else
                    throw;
            }
        }

        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoDepartamentoPorId(id);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoDepartamentoPorId(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var departamento = await departamentoDAL.EliminarDepartamentoPorId((long)id);
            TempData["Message"] = "Departamento " + departamento.Nome.ToUpper() + " foi removido";
            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> ObterVisaoDepartamentoPorId(long? id)
        {
            if (id == null) return NotFound();

            var departamento = await departamentoDAL.ObterDepartamentoPorId((long)id);
            if (departamento == null) return NotFound();

            return View(departamento);
        }

        private async Task<bool> DepartamentoExists(long? id)
        {
            return await departamentoDAL.ObterDepartamentoPorId((long)id) != null;
        }
    }
}