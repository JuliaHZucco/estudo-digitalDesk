using Capitulo01.Data;
using Capitulo01.Data.DAL.Cadastros;
using Capitulo01.Data.DAL.Docente;
using Capitulo01.Modelo.Cadastros;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Capitulo01.Areas.Docente.Models;
using Modelo.Docente; 
using Newtonsoft.Json;
using System;

namespace Capitulo01.Areas.Docente.Controllers
{
    [Area("Docente")]
    public class ProfessorController : Controller //Define o controlador ProfessorController que herda de Controller
    {   
        //Injeção de dependência do contexto do banco de dados e inicialização dos objetos DAL necessários 
        private readonly IESContext _context;
        private readonly InstituicaoDAL instituicaoDAL;
        private readonly DepartamentoDAL departamentoDAL;
        private readonly CursoDAL cursoDAL;
        private readonly ProfessorDAL professorDAL;

        //Construtor que recebe o contexto do banco e inicializa os objetos DAL 
        public ProfessorController(IESContext context)
        {
            _context = context;
            instituicaoDAL = new InstituicaoDAL(context);
            departamentoDAL = new DepartamentoDAL(context);
            cursoDAL = new CursoDAL(context);
            professorDAL = new ProfessorDAL(context);
        }

        //Retorna a lista de professores classificados por nome e exibe na Index 
        public IActionResult Index()
        {
            return View(professorDAL.ObterProfessoresClassificadosPorNome().ToList());
        }

        //View para criar um novo professor 
        public IActionResult Create()
        {
            return View();
        }

        //Post para criar um professor 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Nome")] Professor professor) //([Bind("Nome")] Professor professor) manipula o envio do form 
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

                    //TempData armazena msg temporárias entre requisições, ex msg de sucesso 
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

        //Detalhes de um professor específico pelo ID 
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

        //Exibe formulário de edição para professor existente 
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

        //Edita dados do professor 

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

        //Exibe confirmação de exclusão de professor 
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

        //Exclui prof do banco de dados e redireciona p Index 

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

        //Preenche os ViewBags com listas de instituições, departamentos, cursos e professores 
        public void PrepararViewBags(
            List<Instituicao> instituicoes,
            List<Departamento> departamentos,
            List<Curso> cursos,
            List<Professor> professores)
        {
            instituicoes.Insert(0, new Instituicao()
            {
                InstituicaoID = 0,
                Nome = "Selecione a instituição"
            });
            ViewBag.Instituicoes = instituicoes;

            departamentos.Insert(0, new Departamento()
            {
                DepartamentoID = 0,
                Nome = "Selecione o departamento"
            });
            ViewBag.Departamentos = departamentos;

            cursos.Insert(0, new Curso()
            {
                CursoID = 0,
                Nome = "Selecione o curso"
            });
            ViewBag.Cursos = cursos;

            professores.Insert(0, new Professor()
            {
                ProfessorID = 0,
                Nome = "Selecione o professor"
            });
            ViewBag.Professores = professores;
        }

        //View para add professores ao curso preenchendo os ViewBags com dados iniciais 
        [HttpGet]
        public IActionResult AdicionarProfessor()
        {
            PrepararViewBags(
                instituicaoDAL.ObterInstituicoesClassificadasPorNome().ToList(),
                new List<Departamento>().ToList(),
                new List<Curso>().ToList(),
                new List<Professor>().ToList()
            );

            return View();
        }

        //Envia os dados do form para add professor ao curso 

        [HttpPost]
        [ValidateAntiForgeryToken]

        //[Bind("InstituicaoID,DepartamentoID,CursoID,ProfessorID")] limita campos que podem ser vinculados
        public IActionResult AdicionarProfessor([Bind("InstituicaoID,DepartamentoID,CursoID,ProfessorID")] AdicionarProfessorViewModel model)
        {   
            //verifica se todos os dados foram selecionados 
            if (model.InstituicaoID == 0 ||
                model.DepartamentoID == 0 ||
                model.CursoID == 0 ||
                model.ProfessorID == 0)
            {
                ModelState.AddModelError("", "É preciso selecionar todos os dados");
            }
            else
            {   
                //chama método que registra associação entre curso e professor no banco de dados 
                cursoDAL.RegistrarProfessor(
                    (long)model.CursoID,
                    (long)model.ProfessorID
                );

                //Chama método que registra curso e professor na sessão http 
                RegistrarProfessorNaSessao(
                    (long)model.CursoID,
                    (long)model.ProfessorID
                   );

                //Prepara as oppções que aparecerão nos dropdowns 
                PrepararViewBags(
                    instituicaoDAL.ObterInstituicoesClassificadasPorNome().ToList(),
                    departamentoDAL.ObterDepartamentosPorInstituicao((long)model.InstituicaoID).ToList(),
                    cursoDAL.ObterCursosPorDepartamento((long)model.DepartamentoID).ToList(),
                    cursoDAL.ObterProfessoresForaDoCurso((long)model.CursoID).ToList()
                );
            }

            return View(model);
        }

        //Armazena na sessão http a associação entre curso e professor 
        public void RegistrarProfessorNaSessao(long cursoID, long professorID)
        {   
        //Cria objeto CursoProfessor com IDs do curso e professor 
            var cursoProfessor = new CursoProfessor()
            {
                ProfessorID = professorID,
                CursoID = cursoID
            };

            List<CursoProfessor> cursosProfessor = new List<CursoProfessor>();

            string cursosProfessoresSession = HttpContext.Session.GetString("cursosProfessores");

            if (cursosProfessoresSession != null)
            {
                cursosProfessor = JsonConvert.DeserializeObject<List<CursoProfessor>>(cursosProfessoresSession);
            }

            cursosProfessor.Add(cursoProfessor);

            //Recupera string do HttpContext.Session
            HttpContext.Session.SetString(
                "cursosProfessores",
                JsonConvert.SerializeObject(cursosProfessor)
            );
        }

        //Exibe os útlimos registros de associação entre curso e professor
        public IActionResult VerificarUltimosRegistros()
        {
            List<CursoProfessor> cursosProfessor = new List<CursoProfessor>();

            //Acessa valor armazenado na sessão sob a chave cursosProfessores 
            string cursosProfessoresSession = HttpContext.Session.GetString("cursosProfessores");

            if (cursosProfessoresSession != null)
            {   
                //Desserializa a string JSON de volta para uma lista de objetos CursoProfessor 
                var cursosProfessorSession = JsonConvert.DeserializeObject<List<CursoProfessor>>(cursosProfessoresSession);

                foreach (var item in cursosProfessorSession)
                {
                    var cursoProfessor = new CursoProfessor()
                    {
                        CursoID = item.CursoID,
                        ProfessorID = item.ProfessorID,
                        Curso = cursoDAL.ObterCursoPorId(item.CursoID),
                        Professor = professorDAL.ObterProfessorPorId(item.ProfessorID)

                    };
                        cursosProfessor.Add(cursoProfessor);

                    }
                }

            return View(cursosProfessor);
        }

        //Método retorna lista de departamentos em formato JSON para uma isntituição com base no ID
        public JsonResult ObterDepartamentosPorInstituicao(long actionID)
        {
            try
            {
                var departamentos = departamentoDAL
                    .ObterDepartamentosPorInstituicao(actionID) //Acessa o DAL para obter os departamentos de uma instituição específica
                    //Projeta os departamentos em um formato anônimo para drodowns
                    .Select(d => new { value = d.DepartamentoID, text = d.Nome })
                    .ToList();

                //Retorna departamentos em formato JSON 
                return Json(departamentos);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        //Retorna lista de cursos em formato JSON 
        public JsonResult ObterCursosPorDepartamento(long actionID)
        {
            try
            {
                var cursos = cursoDAL
                    .ObterCursosPorDepartamento(actionID)
                    .Select(c => new { value = c.CursoID, text = c.Nome })
                    .ToList();

                return Json(cursos);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        //Retorna lista de professores em formato JSON 
        public JsonResult ObterProfessoresForaDoCurso(long actionID)
        {
            try
            {
                var professores = cursoDAL
                    .ObterProfessoresForaDoCurso(actionID)
                    .Select(p => new { value = p.ProfessorID, text = p.Nome })
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