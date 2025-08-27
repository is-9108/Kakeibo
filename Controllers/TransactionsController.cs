using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kakeibo.Models;

namespace Kakeibo.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly KakeiboContext _context;

        public TransactionsController(KakeiboContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index(int? page, string filter)
        {
            if(filter == "すべて")
                filter = null;

            var categorys = from m in _context.Categorys select m;
            if (page == null)
                page = 0;
            int max = 6;

            var transactions = from m in _context.Transactions select m;
            transactions = transactions.Skip((int)page * max).Take(max).Include(t => t.Category);
            if(!string.IsNullOrEmpty(filter))
            {
                transactions = transactions.Where(t => t.Category.Name.Contains(filter));
                ViewData["filter"] = filter;
            }
            if (page > 0)
                ViewData["Prev"] = (int)page - 1;
            if (transactions.Count() >= max)
            {
                ViewData["Next"] = (int)page + 1;
                if(_context.Transactions.Skip(max * ((int)page + 1)).Take(max).Count() == 0)
                    ViewData["Next"] = null;
            }
            ViewData["Categories"] = new SelectList(categorys, "Id", "Name");
            return View(await transactions.ToListAsync());
        }


        // GET: Transactions/Create/{bool}
        public IActionResult Create(bool isExp)
        {
            DateTime now = DateTime.Now;
            DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var Transaction = new Transaction
            {
                Date = dateTime
                //IsExpense = isExpenseCheck
            };
            ViewData["IsExpense"] = isExp;
            ViewData["CategoryId"] = new SelectList(_context.Categorys.Where(c => c.IsExpense == isExp), "Id", "Name");
            return View(Transaction);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Title,Amount,Date,IsExpense")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categorys, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id, bool isExp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categorys.Where(c => c.IsExpense == isExp), "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Title,Amount,Date,IsExpense")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
