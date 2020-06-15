using CovidMovieMadness___Tenta.DAL;
using CovidMovieMadness___Tenta.Migrations;
using CovidMovieMadness___Tenta.Models;
using CovidMovieMadness___Tenta.ViewModels;
using Microsoft.Ajax.Utilities;
using PagedList;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CovidMovieMadness___Tenta.Controllers
{
    public class MovieController : Controller
    {
        private MovieContext db = new MovieContext();

        // GET: Movie
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

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
                case "name_desc":
                    movies = movies.OrderByDescending(s => s.Name);
                    break;
                case "Date":
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
            Post post = db.Post.Where(i => i.ID == id).FirstOrDefault();
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
        public ActionResult Edit([Bind(Include = "ID,Name,Genre,Year")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
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
        public ActionResult DeleteConfirmed(int id)
        {
            //It dont be work
            Movie movie = db.Movie.Find(id);
            if (movie.Post != null)
            {
                Post post = db.Post.Where(i => i.ID == movie.Post.ID)?.SingleOrDefault();
                List<Comment> comments = db.Comment.Where(i => post.ID == post.ID)?.ToList();
                foreach (var item in comments)
                {
                    db.Comment.Remove(item);
                }
                db.Post.Remove(post);
                db.Movie.Remove(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            } else
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
