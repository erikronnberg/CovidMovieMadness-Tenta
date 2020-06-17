using CovidMovieMadness___Tenta.DAL;
using CovidMovieMadness___Tenta.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CovidMovieMadness___Tenta.Controllers
{
    public class CommentController : Controller
    {
        private MovieContext db = new MovieContext();

        // GET: Comment
        public ActionResult Index()
        {
            return View(db.Comment.ToList());
        }

        // GET: Comment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Username,CommentContent,UserRating,")] Comment comment, int? ID)
        {
            if (ModelState.IsValid)
            {
                Post post = db.Post.Where(p => p.ID == ID).FirstOrDefault();
                post.Comment.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "Movie", new { id = post.Movie.ID });
            }

            return View(comment);
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comment.Find(ID);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<ActionResult> Edit(int? ID, byte[] rowVersion, int? postID)
        {
            string[] fieldsToBind = new string[] { "ID", "Username", "CommentContent", "UserRating", "RowVersion" };
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var commentToUpdate = await db.Comment.FindAsync(ID);
            if (commentToUpdate == null)
            {
                Comment deletedComment = new Comment();
                TryUpdateModel(deletedComment, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The comment was deleted by another user.");
                return View(deletedComment);
            }
            if (TryUpdateModel(commentToUpdate, fieldsToBind))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(commentToUpdate).OriginalValues["RowVersion"] = rowVersion;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Details", "Movie", new { id = postID});
                    }
                    return View(commentToUpdate);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Comment)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The comment was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Comment)databaseEntry.ToObject();

                        if (databaseValues.Username != clientValues.Username)
                            ModelState.AddModelError("Username", "Current value: "
                                + databaseValues.Username);
                        if (databaseValues.CommentContent != clientValues.CommentContent)
                            ModelState.AddModelError("CommentContent", "Current value: "
                                + string.Format("{0:c}", databaseValues.CommentContent));
                        if (databaseValues.UserRating != clientValues.UserRating)
                            ModelState.AddModelError("UserRating", "Current value: "
                                + string.Format("{0:d}", databaseValues.UserRating));
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        commentToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(commentToUpdate);
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ID, int? postID)
        {
            Comment comment = db.Comment.Find(ID);
            db.Comment.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "Movie", new { id = postID });
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
