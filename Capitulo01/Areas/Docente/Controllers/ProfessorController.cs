using Capitulo01.Data;
using Capitulo01.Data.DAL.Cadastros;
using Microsoft.AspNetCore.Mvc;
using Capitulo01.Data.DAL.Docente;
using Capitulo01.Modelo.Cadastros;
using Modelo.Docente;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Capitulo01.Areas.Docente.Controllers
{
    [Area("Docente")]
    public class ProfessorController : Controller
    {
        private readonly IESContext _context;
        private readonly InstituicaoDAL instituicaoDAL;
        private readonly DepartamentoDAL departamentoDAL;
        private readonly CursoDAL cursoDAL;
        private readonly ProfessorDAL professorDAL;

        public ProfessorController(IESContext context)
        {
            _context = context;
            instituicaoDAL = new InstituicaoDAL(context);
            departamentoDAL = new DepartamentoDAL(context);
            cursoDAL = new CursoDAL(context);
            professorDAL = new ProfessorDAL(context);
        }

        public IActionResult Index()
        {
            return View(professorDAL.ObterProfessoresClassificadosPorNome().ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Nome")] Professor professor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(professor.Nome))
                    {
                        ModelState.AddModelError("Nome", "O nome do professor é obrigatório.");
                        return View(professor);
                    }

                    professorDAL.Inserir(professor);
                    TempData["Message"] = "Professor cadastrado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao cadastrar professor: {ex.Message}");
            }

            return View(professor);
        }

        public IActionResult Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professor = professorDAL.ObterProfessorPorId((long)id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professor = professorDAL.ObterProfessorPorId((long)id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, [Bind("ProfessorID,Nome")] Professor professor)
        {
            if (id != professor.ProfessorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    professorDAL.Atualizar(professor);
                    TempData["Message"] = "Professor atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao atualizar o professor: {ex.Message}");
                }
            }

            return View(professor);
        }

        public IActionResult Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professor = professorDAL.ObterProfessorPorId((long)id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            try
            {
                professorDAL.Excluir(id);
                TempData["Message"] = "Professor excluído com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Erro ao excluir o professor: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public void PrepararViewBags(List<Instituicao> instituicoes, List<Departamento> departamentos, List<Curso> cursos, List<Professor> professores)
        {
            instituicoes.Insert(0, new Instituicao() { InstituicaoID = 0, Nome = "Selecione a instituição" });
            ViewBag.Instituicoes = instituicoes;

            departamentos.Insert(0, new Departamento() { DepartamentoID = 0, Nome = "Selecione o departamento" });
            ViewBag.Departamentos = departamentos;

            cursos.Insert(0, new Curso() { CursoID = 0, Nome = "Selecione o curso" });
            ViewBag.Cursos = cursos;

            professores.Insert(0, new Professor() { ProfessorID = 0, Nome = "Selecione o professor" });
            ViewBag.Professores = professores;
        }
        [HttpGet]
        public IActionResult AdicionarProfessor()
        {
            PrepararViewBags(
                instituicaoDAL.ObterInstituicoesClassificadasPorNome().ToList(),
                new List<Departamento>(),
                new List<Curso>(),
                professorDAL.ObterProfessoresClassificadosPorNome().ToList()
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdicionarProfessor([Bind("InstituicaoID, DepartamentoID, CursoID, ProfessorID")] AdicionarProfessorViewModel model)
        {
            if (model.InstituicaoID == 0 || model.DepartamentoID == 0 || model.CursoID == 0 || model.ProfessorID == 0)
            {
                ModelState.AddModelError("", "É preciso selecionar todos os dados");
            }
            else
            {
                try
                {
                    cursoDAL.RegistrarProfessor((long)model.CursoID, (long)model.ProfessorID);
                    ViewBag.Mensagem = "Professor adicionado ao curso com sucesso!";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao registrar professor: {ex.Message}");
                }

                PrepararViewBags(
                    instituicaoDAL.ObterInstituicoesClassificadasPorNome().ToList(),
                    departamentoDAL.ObterDepartamentosPorInstituicao((long)model.InstituicaoID).ToList(),
                    cursoDAL.ObterCursosPorDepartamento((long)model.DepartamentoID).ToList(),
                    cursoDAL.ObterProfessoresForaDoCurso((long)model.CursoID).ToList()
                );
            }

            return View(model);
        }

        public JsonResult ObterDepartamentosPorInstituicao(long actionID)
        {
            try
            {
                var departamentos = departamentoDAL
                    .ObterDepartamentosPorInstituicao(actionID)
                    .Select(d => new { Value = d.DepartamentoID, Text = d.Nome })
                    .ToList();
                return Json(departamentos);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public JsonResult ObterCursosPorDepartamento(long actionID)
        {
            try
            {
                var cursos = cursoDAL
                    .ObterCursosPorDepartamento(actionID)
                    .Select(c => new { Value = c.CursoID, Text = c.Nome })
                    .ToList();
                return Json(cursos);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public JsonResult ObterProfessoresForaDoCurso(long actionID)
        {
            try
            {
                var professores = cursoDAL
                    .ObterProfessoresForaDoCurso(actionID)
                    .Select(p => new { Value = p.ProfessorID, Text = p.Nome })
                    .ToList();
                return Json(professores);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}