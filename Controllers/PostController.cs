using CovidMovieMadness___Tenta.DAL;
using CovidMovieMadness___Tenta.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CovidMovieMadness___Tenta.Controllers
{
    public class PostController : Controller
    {
        private MovieContext db = new MovieContext();

        // GET: Post
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = sortOrder == "title_asc" ? "title_desc" : "title_asc";
            ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var post = from m in db.Post
                       select m;
            if (!string.IsNullOrEmpty(searchString))
            {
                post = post.Where(m => m.PostTitle.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_asc":
                    post = post.OrderBy(s => s.PostTitle);
                    break;
                case "title_desc":
                    post = post.OrderByDescending(s => s.PostTitle);
                    break;
                case "date_asc":
                    post = post.OrderBy(s => s.PostDate);
                    break;
                case "date_desc":
                    post = post.OrderByDescending(s => s.PostDate);
                    break;
                default:
                    post = post.OrderByDescending(s => s.PostDate);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var movie = db.Movie.Include(p => p.Post);
            return View(post.ToPagedList(pageNumber, pageSize));
        }

        // GET: Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.Movie, "ID", "Name", db.Movie);
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PostTitle,PostRating,PostContent,Movie")] Post post, int? ID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    post.PostDate = DateTime.Now;
                    db.Post.Add(post);
                    post.Comment = new List<Comment>();
                    db.SaveChanges();
                    return RedirectToAction("Details", "Movie", new { id = ID });
                } catch
                {

                }
            }
            return View(post);
        }

        // GET: Post/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? ID, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "ID", "PostTitle", "PostRating", "PostContent", "RowVersion" };

            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var postToUpdate = await db.Post.FindAsync(ID);
            if (postToUpdate == null)
            {
                Post deletedPost = new Post();
                TryUpdateModel(deletedPost, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The post was deleted by another user.");
                ViewBag.ID = new SelectList(db.Movie, "ID", "Name", postToUpdate.ID);
                return View(deletedPost);
            }

            if (TryUpdateModel(postToUpdate, fieldsToBind))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(postToUpdate).OriginalValues["RowVersion"] = rowVersion;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Details", "Movie", new { id = ID });
                    }
                    ViewBag.ID = new SelectList(db.Movie, "ID", "Name", postToUpdate.ID);
                    return View(postToUpdate);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Post)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The post was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Post)databaseEntry.ToObject();

                        if (databaseValues.PostTitle != clientValues.PostTitle)
                            ModelState.AddModelError("PostTitle", "Current value: "
                                + databaseValues.PostTitle);
                        if (databaseValues.PostContent != clientValues.PostContent)
                            ModelState.AddModelError("PostContent", "Current value: "
                                + string.Format("{0:c}", databaseValues.PostContent));
                        if (databaseValues.PostRating != clientValues.PostRating)
                            ModelState.AddModelError("PostRating", "Current value: "
                                + string.Format("{0:d}", databaseValues.PostRating));
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        postToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            ViewBag.ID = new SelectList(db.Movie, "ID", "Name", postToUpdate.ID);
            return View(postToUpdate);
        }

        // GET: Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ID)
        {
            //It dont be work
            Post post = db.Post.Find(ID);
            List<Comment> comments = post.Comment.ToList();
            foreach (var item in comments)
            {
                db.Comment.Remove(item);
            }
            db.Post.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Details", "Movie", new { id = ID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
