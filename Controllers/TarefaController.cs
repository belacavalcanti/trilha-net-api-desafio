using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        // Injeção de dependência para acessar o contexto do banco de dados
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        // Método para obter uma tarefa pelo ID
        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Busca a tarefa no banco de dados pelo ID
            var tarefa = _context.Tarefas.Find(id);

            // Se a tarefa não for encontrada, retorna status 404
            if (tarefa == null)
                return NotFound();

            // Se a tarefa for encontrada, retorna com status 200
            return Ok(tarefa);
        }

        // Método para obter todas as tarefas
        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Busca todas as tarefas no banco utilizando o EF
            var tarefas = _context.Tarefas.ToList();
            return Ok(tarefas);
        }

        // Método para obter tarefas pelo título
        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Busca as tarefas no banco que contenham o título recebido por parâmetro
            var tarefas = _context.Tarefas.Where(x => x.Titulo.Contains(titulo)).ToList();

            // Retorna as tarefas encontradas com status 200
            return Ok(tarefas);
        }

        // Método para obter tarefas por data
        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            // Busca as tarefas no banco que tenham a data recebida por parâmetro
            var tarefas = _context.Tarefas.Where(x => x.Data.Date == data.Date).ToList();

            // Retorna as tarefas encontradas com status 200
            return Ok(tarefas);
        }

        // Método para obter tarefas pelo status
        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Busca as tarefas no banco que contenham o status recebido por parâmetro
            var tarefas = _context.Tarefas.Where(x => x.Status == status).ToList();
            
            // Retorna as tarefas encontradas com status 200
            return Ok(tarefas);
        }

        // Método para criar uma nova tarefa
        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            // Valida se a tarefa recebida é nula
            if (tarefa == null)
                return BadRequest(new { Erro = "A tarefa não pode ser nula" });

            // Valida se a data da tarefa não está vazia
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Adiciona a nova tarefa no banco de dados e salva as mudanças
            _context.Add(tarefa);
            _context.SaveChanges();

            // Retorna a tarefa criada com status 201
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        // Método para atualizar uma tarefa existente
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            // Busca a tarefa no banco de dados pelo ID
            var tarefaBanco = _context.Tarefas.Find(id);

            // Se a tarefa não for encontrada, retorna status 404
            if (tarefaBanco == null)
                return NotFound();

            // Valida se a tarefa recebida é nula
            if (tarefa == null)
                return BadRequest(new { Erro = "A tarefa não pode ser nula" });

            // Valida se a data da tarefa não está vazia
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Atualiza as informações da tarefa no banco de dados
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            // Salva as mudanças no banco de dados
            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            // Retorna status 200 após a atualização
            return Ok(tarefaBanco);
        }

        // Método para deletar uma tarefa existente
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            // Busca a tarefa no banco de dados pelo ID
            var tarefaBanco = _context.Tarefas.Find(id);

            // Se a tarefa não for encontrada, retorna status 404
            if (tarefaBanco == null)
                return NotFound();

            // Remove a tarefa encontrada e salva as mudanças no banco de dados
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            // Retorna status 204 após a deleção
            return NoContent();
        }
    }
}
