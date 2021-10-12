using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudWebMvc.Data;
using CrudWebMvc.Models;
using Microsoft.AspNetCore.Authorization;

namespace CrudWebMvc.Controllers
{
    [Authorize] //Obrigar a fazer login
    public class ProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Produtos
        public async Task<IActionResult> Index()
        {

            _context.LogAuditoria.Add(
             new LogAuditoria
             {
                 EmailUsuario = User.Identity.Name,
                 DetalhesAuditoria = "Entrou na tela de listagem de produtos"
             });

            return View(await _context.Produto.ToListAsync());
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.LogAuditoria.Add(
               new LogAuditoria
               {
                   EmailUsuario = User.Identity.Name,
                   DetalhesAuditoria = String.Concat("Entrou na tela de detalhes do produto: ",
                      produto.Id, " - ", produto.Nome)
               });

            return View(produto);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {

            _context.LogAuditoria.Add(
                new LogAuditoria
                {
                    EmailUsuario = User.Identity.Name,
                    DetalhesAuditoria = "Entrou na tela de Cadastro"
                });

            _context.SaveChanges();

            return View();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();


                _context.LogAuditoria.Add(
                new LogAuditoria
                {
                      EmailUsuario = User.Identity.Name,
                       DetalhesAuditoria = string.Concat("Cadastrou o Produto: ",
                       produto.Nome, "Data de cadastro: ", DateTime.Now.ToLongDateString())
                 });

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.LogAuditoria.Add(
               new LogAuditoria
               {
                   EmailUsuario = User.Identity.Name,
                   DetalhesAuditoria = String.Concat("Entrou na tela de edição do produto: ",
                      produto.Id, " - ", produto.Nome)
               });

            _context.SaveChanges();
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();

                    _context.LogAuditoria.Add(
               new LogAuditoria
               {
                   EmailUsuario = User.Identity.Name,
                   DetalhesAuditoria = string.Concat("Atualizou o Produto: ",
                      produto.Nome, "Data de atualização: ", DateTime.Now.ToLongDateString())
               });

                    _context.SaveChanges();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produto.FindAsync(id);
            _context.Produto.Remove(produto);
            await _context.SaveChangesAsync();

            _context.LogAuditoria.Add(
               new LogAuditoria
               {
                   EmailUsuario = User.Identity.Name,
                   DetalhesAuditoria = string.Concat("Deletou o Produto: ",
                      produto.Nome, "Data de exclusão: ", DateTime.Now.ToLongDateString())
               });

            _context.SaveChanges();



            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produto.Any(e => e.Id == id);
        }
    }
}
