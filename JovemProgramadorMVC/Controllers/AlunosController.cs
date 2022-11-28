using JovemProgramadorMVC.Data.Repositorio.Interface;
using JovemProgramadorMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace JovemProgramadorMVC.Controllers
{
    public class AlunosController : Controller
    {
        private readonly IAlunoRepositorio _alunoRepositorio;
        private readonly IConfiguration _configuration;

        public AlunosController(IAlunoRepositorio alunoRepositorio, IConfiguration configuration)
        {
            _alunoRepositorio = alunoRepositorio;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                var alunos = _alunoRepositorio.BuscarAlunos();
                return View(alunos);
            }
            catch (System.Exception)
            {
                TempData["ErroAlunos"] = "Não foi encontrar os alunos cadastrados.";
                return View();
            }
        }

        public IActionResult AdicionarAluno()
        {
            return View();
        }

        public IActionResult InserirAluno(AlunoModel alunos)
        {
            try
            {
                _alunoRepositorio.InserirAluno(alunos);
            
                TempData["MensagemSucesso"] = "Aluno adicionado com sucesso!";
            }
            catch (System.Exception)
            {
                TempData["MensagemErro"] = "Não foi possível adicionar o aluno.";
            }
            return RedirectToAction("Index");
        }

        public IActionResult AlterarAluno(AlunoModel aluno)
        {
            _alunoRepositorio.EditarAluno(aluno);
            return RedirectToAction("Index");
        }        
        
        public IActionResult EditarAluno(int id)
        {
            try
            {
                var aluno = _alunoRepositorio.BuscarId(id);

                TempData["SucessoEdicao"] = "Aluno editado com sucesso!";

                return View(aluno);
            }
            catch (System.Exception)
            {
                TempData["ErroEdicao"] = "Não foi possível editar os dados do aluno.";
                return View();
            }
        }

        public IActionResult ExcluirAluno(AlunoModel aluno)
        {
            try
            {
                _alunoRepositorio.ExcluirAluno(aluno);

                TempData["SucessoExclusao"] = "Aluno excluído com sucesso!";

                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {
                TempData["ErroExclusao"] = "Não foi possível excluir o aluno.";
                return View();
            }
        }

        public async Task<IActionResult> BuscarEndereco(string cep)
        {
            try
            {
                cep = cep.Replace("-", "");

                EnderecoModel enderecoModel = new();

                using var client = new HttpClient();

                var result = await client.GetAsync(_configuration.GetSection("ApiCep")["BaseUrl"] + cep + "/json");

                if(result.IsSuccessStatusCode)
                {
                    enderecoModel = JsonSerializer.Deserialize<EnderecoModel>(
                        await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { });
                }

                return View("Endereco", enderecoModel);
            }
            catch (System.Exception)
            {
                TempData["ErroBuscaEndereco"] = "Não encontrar o endereço solicitado.";
                return View();
            }
        }
    }
}
