using BookListRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

/// <summary>
/// Razorページだと,Viewを返さないんだな.
/// リクエストの結果をBindPropertyに入れれば
/// ,それを使ってViewが再描画される.
/// </summary>
///

namespace BookListRazor
{
    /// <summary>
    /// 新規作成と更新の両方をまとめたページ
    /// アップサート
    /// </summary>
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public UpsertModel(ApplicationDbContext db)
        {
            this._db = db;
        }

        [BindProperty]
        public Book Book { get; set; }

        // 新規作成の場合はidが無いので,Null許容にしておく
        public async Task<IActionResult> OnGet(int? id)
        {
            Book = new Book();
            if (id == null)
            {
                //For Create
                return Page();
            }

            //For Update
            Book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (Book == null)
            { return NotFound(); }
            Book = await _db.Books.FindAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Book.Id == 0)
                {
                    //For Create.
                    _db.Books.Add(Book);
                }
                else
                {
                    //For Update
                    _db.Books.Update(Book);
                }

                await _db.SaveChangesAsync();

                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}