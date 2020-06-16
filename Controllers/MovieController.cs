using CovidMovieMadness___Tenta.DAL;
using CovidMovieMadness___Tenta.Models;
using CovidMovieMadness___Tenta.ViewModels;
using PagedList;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace CovidMovieMadness___Tenta.Controllers
{
    public class MovieController : Controller
    {
        private MovieContext db = new MovieContext();

        public PartialViewResult AllRatings(int? ID)
        {
            Post post = db.Post.Where(i => i.ID == i.Movie.ID).FirstOrDefault();
            List<Comment> comments = post.Comment;
            return PartialView("_AvarageRating", comments);
        }

        // GET: Movie
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
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

            var movies = from m in db.Movie
                         select m;
            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Name.Contains(searchString)
                                       || m.Genre.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_asc":
                    movies = movies.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    movies = movies.OrderByDescending(s => s.Name);
                    break;
                case "date_asc":
                    movies = movies.OrderBy(s => s.Year);
                    break;
                case "date_desc":
                    movies = movies.OrderByDescending(s => s.Year);
                    break;
                default:
                    movies = movies.OrderBy(s => s.Genre);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(movies.ToPagedList(pageNumber, pageSize));
        }

        // GET: Movie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            Post post = db.Post.Where(i => i.Movie.ID == id).FirstOrDefault();
            if (post != null)
            {
                MoviePostDetailsView moviePostDetails = new MoviePostDetailsView
                {
                    MovieID = movie.ID,
                    Name = movie.Name,
                    Genre = movie.Genre,
                    Year = movie.Year,
                    PostID = post.ID,
                    PostContent = post.PostContent,
                    PostRating = post.PostRating,
                    PostTitle = post.PostTitle,
                    PostDate = post.PostDate,
                    Comment = post.Comment
                };
                if (movie == null)
                {
                    return HttpNotFound();
                }
                return View(moviePostDetails);
            }
            else
            {
                MoviePostDetailsView movieDetails = new MoviePostDetailsView
                {
                    MovieID = movie.ID,
                    Name = movie.Name,
                    Genre = movie.Genre,
                    Year = movie.Year
                };
                if (movie == null)
                {
                    return HttpNotFound();
                }
                return View(movieDetails);
            }
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Genre,Year")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movie.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? ID, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "ID", "Name", "Genre", "Year", "RowVersion" };
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var movieToUpdate = await db.Movie.FindAsync(ID);
            if (movieToUpdate == null)
            {
                Movie deletedMovie = new Movie();
                TryUpdateModel(deletedMovie, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");
                return View(deletedMovie);
            }
            if (TryUpdateModel(movieToUpdate, fieldsToBind))
            {
                try
                {
                    db.Entry(movieToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Movie)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Movie)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);
                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: "
                                + string.Format("{0:c}", databaseValues.Name));
                        if (databaseValues.Genre != clientValues.Genre)
                            ModelState.AddModelError("Genre", "Current value: "
                                + string.Format("{0:d}", databaseValues.Genre));
                        if (databaseValues.Year != clientValues.Year)
                            ModelState.AddModelError("Year", "Current value: "
                                + string.Format("{0:d}", databaseValues.Year));
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        movieToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(movieToUpdate);
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Movie movie = db.Movie.Find(id);
            if (movie.Post != null)
            {
                List<Comment> comments = movie.Post.Comment.ToList();
                foreach (var item in comments)
                {
                    db.Comment.Remove(item);
                }
                db.Post.Remove(movie.Post);
                db.Movie.Remove(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                db.Movie.Remove(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
