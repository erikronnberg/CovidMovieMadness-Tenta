using CovidMovieMadness___Tenta.DAL;
using CovidMovieMadness___Tenta.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
                    post = post.OrderBy(s => s.ID);
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
                post.PostDate = DateTime.Now;
                db.Post.Add(post);
                post.Comment = new List<Comment>();
                db.SaveChanges();
                return RedirectToAction("Details", "Movie", new { id = ID });
            }

            ViewBag.ID = new SelectList(db.Movie, "ID", "Name", post.ID);
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
            ViewBag.ID = new SelectList(db.Movie, "ID", "Name", post.ID);
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PostTitle,PostRating,PostContent")] Post post, int? ID)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Movie", new { id = ID });
            }
            ViewBag.ID = new SelectList(db.Movie, "ID", "Name", post.ID);
            return View(post);
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
