using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamBuilderPkmnASP.Data;
using TeamBuilderPkmnASP.Models;
using SendGrid.Helpers.Mail;
using System.Security.Cryptography;

namespace TeamBuilderPkmnASP.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserContext _context;

        public LoginController(UserContext context)
        {
            _context = context;
        }

        public ActionResult Signin()
        {
            string userMail = HttpContext.Request.Form["mail"].ToString();
            string pwd = HttpContext.Request.Form["pwd"].ToString();
            IQueryable<User> users = _context.User.Where(user => user.Mail == userMail);
            if (users.Any())
            {
                TempData["Error"] = "Mail déjà enregistré";
                return RedirectToAction("Login", "Pokemons");
            }
            if(HttpContext.Request.Form["validPwd"].ToString() != pwd)
            {
                TempData["Error"] = "Les mots de passe ne sont pas identiques";
                return RedirectToAction("Signin", "Pokemons");
            }
            else
            {
                User user = new User();
                user.Mail = HttpContext.Request.Form["mail"];
                user.Pseudo = HttpContext.Request.Form["pseudo"];
                user.IsPwdInputHashed = false;
                user.Password = HttpContext.Request.Form["pwd"];
                user.VerificationToken = Guid.NewGuid().ToString();
                user.IsVerified = false;
                try
                {
                    _context.User.Add(user);

                    var link = "https://localhost:44392/Login/VerifyMail/" + user.VerificationToken;
                    var message = new SendGridMessage();
                    message.AddTo(user.Mail);
                    message.From = new EmailAddress("verification@pkmnteambuilder.com");
                    message.Subject = "Vérifiez votre compte PokemonTeamBuilder !";
                    message.HtmlContent = "<p>Veuillez cliquer sur le lien ci-après pour valider votre compte !</p>" +
                        "<a href=\"" + link + "\">Valider votre compte</a>";
                    _context.SaveChanges();
                    DatabaseConnection.MailClient.SendEmailAsync(message);
                }
                catch (Exception ex)
                {
                    ///TODO: Gestion propre d'exception
                    var log = ex.Message;
                }
                return RedirectToAction("Login", "Pokemons");
            }
        }

        public ActionResult Login()
        {
            string mail = HttpContext.Request.Form["Mail"];
            IQueryable<User> users = _context.User.Where(user => user.Mail == mail);
            User user = users.FirstOrDefault();
            string hashedEntry = "";
            using (var sha = new SHA256Managed())
            {
                string transpPwd = HttpContext.Request.Form["pwd"].ToString();
                byte[] array = Encoding.Default.GetBytes(HttpContext.Request.Form["pwd"].ToString());
                array = sha.ComputeHash(array);
                StringBuilder sb = new StringBuilder();
                foreach (var b in array)
                {
                    sb.Append(b.ToString("x2"));
                }
                hashedEntry = sb.ToString();
            }
            if (user != null && user.Password == hashedEntry)
            {
                if (!user.IsVerified)
                {
                    TempData["Error"] = "Compte non validé, veuillez vérifier vos mails";
                    return RedirectToAction("Signin", "Pokemons");
                }
                ISession session = HttpContext.Session;
                session.SetString("user", user.Mail);
                return RedirectToAction("Index", "Pokemons");
            }
            else
            {
                TempData["Error"] = "Identifiants invalides";
                return RedirectToAction("Login", "Pokemons");
            }
        }

        public ActionResult Logoff()
        {
            ISession session = HttpContext.Session;
            session.Clear();
            return RedirectToAction("Index","Pokemons");
        }

        public ActionResult VerifyMail(string id)
        {
            
            User user = _context.User.Where(user => user.VerificationToken == id).FirstOrDefault();
            ViewData["Title"] = "Signin";
            if (user != null)
            {
                user.IsVerified = true;
                _context.SaveChanges();
                ViewData["Title"] = "Login";
            }
            return View("UserForm");

        }
    }
}
