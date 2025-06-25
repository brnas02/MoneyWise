using DWebProjetoFinal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DWebProjetoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransacaoApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransacaoApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Transacao
        [HttpGet]
        public async Task<IActionResult> GetTransacoes()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var transacoes = await _context.UserTransacao
                .Where(ut => ut.UserAccountId == userId)
                .Select(ut => ut.Transacao)
                .ToListAsync();

            return Ok(transacoes);
        }

        // GET: api/Transacao/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransacao(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var transacao = await _context.UserTransacao
                .Where(ut => ut.UserAccountId == userId && ut.TransacaoId == id)
                .Select(ut => ut.Transacao)
                .FirstOrDefaultAsync();

            if (transacao == null) return NotFound();

            return Ok(transacao);
        }

        // POST: api/Transacao
        [HttpPost]
        public async Task<IActionResult> PostTransacao([FromBody] Transacao model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Transacoes.Add(model);
            await _context.SaveChangesAsync();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            _context.UserTransacao.Add(new UserTransacao
            {
                UserAccountId = userId,
                TransacaoId = model.Id
            });

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTransacao), new { id = model.Id }, model);
        }

        // PUT: api/Transacao/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransacao(int id, [FromBody] Transacao model)
        {
            if (id != model.Id) return BadRequest();

            var exists = await _context.Transacoes.AnyAsync(t => t.Id == id);
            if (!exists) return NotFound();

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Transacao/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransacao(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var transacao = await _context.UserTransacao
                .Where(ut => ut.UserAccountId == userId && ut.TransacaoId == id)
                .Select(ut => ut.Transacao)
                .FirstOrDefaultAsync();

            if (transacao == null) return NotFound();

            _context.Transacoes.Remove(transacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}