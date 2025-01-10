using Kuse_App.Models;
using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Helpers;

namespace Kuse_App.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Ootan sind minu peole! Palun tule!!!";
            var currentHour = DateTime.Now.Hour;

            // Определяем время суток и задаем приветствие
            if (currentHour >= 5 && currentHour < 12)
            {
                ViewBag.Greeting = "Tere hommikust";
            }
            else if (currentHour >= 12 && currentHour < 18)
            {
                ViewBag.Greeting = "Tere päevast";
            }
            else if (currentHour >= 18 && currentHour < 22)
            {
                ViewBag.Greeting = "Tere õhtust";
            }
            else
            {
                ViewBag.Greeting = "Tere ööst";
            }

            int currentMonth = DateTime.Now.Month;

            switch (currentMonth)
            {
                case 12:
                    ViewBag.Message = "Jõulud on tulemas! Ootame sind meie jõulupidu!";
                    ViewBag.Image = "~/Images/joulud.jpg";
                    break;
                case 1:
                    ViewBag.Message = "Uus aasta, uus algus! Tere tulemast aastasse 2025!";
                    ViewBag.Image = "~/Images/uus_aasta.jpg";
                    break;
                case 2:
                    ViewBag.Message = "Jaanipäev on siin! Ootame sind meie jaanipäeva peole!";
                    ViewBag.Image = "~/Images/jaanipaev.jpg";
                    break;
                default:
                    ViewBag.Message = "Olete oodatud meie üritusele! Õhtul saab olema palju huvitavat!";
                    ViewBag.Image = "~/Images/kutse.jpg";
                    break;
            }

            return View();
        }

        [HttpGet]
        public ViewResult Ankeet()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Ankeet(Guest guest)
        {
            if (ModelState.IsValid)
            {
                E_mail(guest);
                if (ModelState.IsValid)
                {
                    return View("Thanks", guest);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View(guest);
            }
        }

        public void E_mail(Guest guest)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "allikvaleria@gmail.com";
                WebMail.Password = "ndnm ipny bgxd aopo";
                WebMail.From = "allikvaleria@gmail.com";
                WebMail.Send("allikvaleria@gmail.com", "Vastus kutsele", guest.Name + " vastas " + (guest.WillAttend.HasValue && guest.WillAttend.Value ? "tuleb peole" : "ei tule peole"));
                ViewBag.Message = "Kiri on saatnud!";
            }
            catch (Exception)
            {
                ViewBag.Message = "Mul on kahju! Ei saa kirja saada!!!";
            }
        }

        [HttpPost]
        public ActionResult Meeldetuletus(Guest guest, string Meeldetuletus)
        {
            if (!string.IsNullOrEmpty(Meeldetuletus))
            {
                try
                {
                    WebMail.SmtpServer = "smtp.gmail.com";
                    WebMail.SmtpPort = 587;
                    WebMail.EnableSsl = true;
                    WebMail.UserName = "allikvaleria@gmail.com";
                    WebMail.Password = "ndnm ipny bgxd aopo";
                    WebMail.From = "allikvaleria@gmail.com";

                    string filePath = Path.Combine(Server.MapPath("~/Images/"), "kutse.jpg");

                    WebMail.Send(guest.Email, "Meeldetuletus", guest.Name + ", ara unusta. Pidu toimub 25.01.25! Sind ootavad väga!",
                    null, guest.Email,
                    filesToAttach: new String[] { Path.Combine(Server.MapPath("~/Images/"), Path.GetFileName("kutse.jpg ")) }
                   );


                    ViewBag.Message = "Kutse saadetud!";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Tekkis viga kutse saatmisel: " + ex.Message;
                }
            }

            return View("Thanks", guest);
        }
    }
}
