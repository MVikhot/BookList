using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BookList.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BookList.Controllers
{
    public class BookController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "CVn1cKIqKn3eK0RD9D0SyMrgY6CzJZ0f5y8WsLEb",
            BasePath = "https://booklist-467ac-default-rtdb.firebaseio.com/"

        };
        IFirebaseClient client; 

        public PushResponse PushResponce { get; private set; }

        // GET: Book
        public ActionResult Index()
        {

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Books");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Book>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Book>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create (Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AddBookToFirebase(book);
                    ModelState.AddModelError(string.Empty, "Added Successfully");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                return View();
            }   
                return View();
        }

        private void AddBookToFirebase(Book book)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = book;
            PushResponse response = client.Push("Books/", data);
            data.book_id = response.Result.name;
            SetResponse setResponse = client.Set("Books/" + data.book_id, data);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Books/" + id);
            Book data = JsonConvert.DeserializeObject<Book>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                client = new FireSharp.FirebaseClient(config);
                SetResponse response = client.Set("Books/" + book.book_id, book);
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Delete(string id)

        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Books/" + id);

            return RedirectToAction("Index");
        }


    }
}