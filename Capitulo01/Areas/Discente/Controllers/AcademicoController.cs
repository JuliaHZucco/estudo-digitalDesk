using Capitulo01.Data;
using Capitulo01.Data.DAL.Discente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelo.Discente;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Capitulo01.Areas.Discente.Controllers
{
    [Area("Discente")]
    [Authorize]
    public class AcademicoController : Controller
    {
        private readonly IESContext _context;
        private readonly AcademicoDAL academicoDAL;

        public AcademicoController(IESContext context)
        {
            _context = context;
            academicoDAL = new AcademicoDAL(context);
        }

        public async Task<IActionResult> Index()
        {
            return View(await academicoDAL.ObterAcademicosClassificadosPorNome().ToListAsync());
        }

        private async Task<IActionResult> ObterVisaoAcademicoPorId(long? id)
        {
            if (id == null)
                return NotFound();

            var academico = await academicoDAL.ObterAcademicoPorId((long)id);
            if (academico == null)
                return NotFound();

            return View(academico);
        }

        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoAcademicoPorId(id);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            return await ObterVisaoAcademicoPorId(id);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoAcademicoPorId(id);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,RegistroAcademico,Nascimento")] Academico academico, IFormFile foto)
        {
            try
            {

                ModelState.Remove("FotoMimeType");
                ModelState.Remove("Foto");
                ModelState.Remove("formFile");

                if (ModelState.IsValid)
                {
                    if (foto != null && foto.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await foto.CopyToAsync(stream);
                            academico.Foto = stream.ToArray();
                            academico.FotoMimeType = foto.ContentType;
                        }
                    }

                    await academicoDAL.GravarAcademico(academico);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados. Erro: " + ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro inesperado: " + ex.Message);
            }

            return View(academico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("AcademicoID,Nome,RegistroAcademico,Nascimento")] Academico academico, IFormFile foto)
        {
            if (id != academico.AcademicoID)
            {
                return NotFound();
            }

            ModelState.Remove("FotoMimeType");
            ModelState.Remove("Foto");
            ModelState.Remove("formFile");

            if (ModelState.IsValid)
            {
                try
                {
  
                    var academicoExistente = await academicoDAL.ObterAcademicoPorId(academico.AcademicoID.Value);
                    if (academicoExistente == null)
                    {
                        return NotFound();
                    }

                    academicoExistente.Nome = academico.Nome;
                    academicoExistente.RegistroAcademico = academico.RegistroAcademico;
                    academicoExistente.Nascimento = academico.Nascimento;

                    if (foto != null && foto.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await foto.CopyToAsync(stream);
                            academicoExistente.Foto = stream.ToArray();
                            academicoExistente.FotoMimeType = foto.ContentType;
                        }
                    }

                    await academicoDAL.GravarAcademico(academicoExistente);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AcademicoExists(academico.AcademicoID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao salvar: {ex.Message}");
                }
            }

            return View(academico);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            if (id == null)
                return NotFound();

            var academico = await academicoDAL.EliminarAcademicoPorId(id.Value);
            TempData["Message"] = "Acadêmico " + academico.Nome.ToUpper() + " foi removido";
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AcademicoExists(long? id)
        {
            return await academicoDAL.ObterAcademicoPorId((long)id) != null;
        }

        public async Task<FileContentResult> GetFoto(long id)
        {
            Academico academico = await academicoDAL.ObterAcademicoPorId(id);
            if (academico != null && academico.Foto != null)
            {
                return File(academico.Foto, academico.FotoMimeType);
            }
            return null;
        }
    }
}
