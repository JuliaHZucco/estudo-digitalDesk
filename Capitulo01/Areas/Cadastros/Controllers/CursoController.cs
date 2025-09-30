using Capitulo01.Data;
using Capitulo01.Data.DAL.Cadastros;
using Capitulo01.Modelo.Cadastros;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Capitulo01.Areas.Cadastros.Controllers
{
    [Area("Cadastros")]
    public class CursoController : Controller
    {
        private readonly IESContext _context;
        private readonly CursoDAL cursoDAL;
        private readonly DepartamentoDAL departamentoDAL;

        public CursoController(IESContext context)
        {
            _context = context;
            cursoDAL = new CursoDAL(context);
            departamentoDAL = new DepartamentoDAL(context);
        }

        public IActionResult Index()
        {
            return View(cursoDAL.ObterCursosClassificadosPorNome().ToList());
        }

        public IActionResult Create()
        {
            try
            {
                var departamentos = departamentoDAL.ObterDepartamentosClassificadosPorNome().ToList();
                ViewBag.Departamentos = new SelectList(departamentos, "DepartamentoID", "Nome");

                Console.WriteLine($"Departamentos carregados: {departamentos.Count}");
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar departamentos: {ex.Message}");
                ViewBag.Departamentos = new SelectList(new List<Departamento>(), "DepartamentoID", "Nome");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Nome,DepartamentoID")] Curso curso)
        {
            Console.WriteLine("=== CREATE POST CHAMADO ===");
            Console.WriteLine($"Nome recebido: '{curso.Nome}'");
            Console.WriteLine($"DepartamentoID recebido: {curso.DepartamentoID}");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState inválido:");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Campo: {error.Key}");
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($"  Erro: {err.ErrorMessage}");
                    }
                }
            }

            try
            {
                if (ModelState.IsValid)
                {
                    Console.WriteLine("Tentando inserir curso...");

                    cursoDAL.Inserir(curso);

                    Console.WriteLine("Curso inserido com sucesso!");
                    TempData["Message"] = "Curso cadastrado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO ao inserir curso: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                ModelState.AddModelError("", $"Erro ao cadastrar curso: {ex.Message}");
            }

            try
            {
                var departamentos = departamentoDAL.ObterDepartamentosClassificadosPorNome().ToList();
                ViewBag.Departamentos = new SelectList(departamentos, "DepartamentoID", "Nome", curso.DepartamentoID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao recarregar departamentos: {ex.Message}");
                ViewBag.Departamentos = new SelectList(new List<Departamento>(), "DepartamentoID", "Nome");
            }

            return View(curso);
        }

        public IActionResult Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = cursoDAL.ObterCursoPorId((long) id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = cursoDAL.ObterCursoPorId((long)id);
            if (curso == null)
            {
                return NotFound();
            }

            ViewBag.Departamentos = new SelectList(departamentoDAL.ObterDepartamentosClassificadosPorNome(), "DepartamentoID", "Nome", curso.DepartamentoID);
            return View(curso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, [Bind("CursoID,Nome,DepartamentoID")] Curso curso)
        {
            if (id != curso.CursoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    cursoDAL.Atualizar(curso);
                    TempData["Message"] = "Curso atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao atualizar o curso: {ex.Message}");
                }
            }

            ViewBag.Departamentos = new SelectList(departamentoDAL.ObterDepartamentosClassificadosPorNome(), "DepartamentoID", "Nome", curso.DepartamentoID);
            return View(curso);
        }

        public IActionResult Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = cursoDAL.ObterCursoPorId((long)id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            try
            {
                cursoDAL.Excluir(id);
                TempData["Message"] = "Curso excluído com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Erro ao excluir o curso: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}