using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TopolkoSeminar.Models;


namespace TopolkoSeminar.Controllers
{
    [Authorize]
    public class ProfilController : Controller
    {
        PredbiljezbedbEntities ent = new PredbiljezbedbEntities();
        public ActionResult Index()
        {

            return View(ent.Zaposleniks.ToList());
        }
        // GET: Profil      
        public ActionResult MojProfil()
        {
            ApplicationDbContext context = new ApplicationDbContext();
           var user= context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
         
            Zaposlenik z = ent.Zaposleniks.Where(x => x.LoginID == user.Id).FirstOrDefault();
            if (z==null)
            {
                z = new Zaposlenik();

            }
            return View(z);
        }
        [HttpPost]
        public ActionResult MojProfil(Zaposlenik z)
        {
            
            try
            {
                ent.Entry(z).State = System.Data.Entity.EntityState.Modified;
                ent.SaveChanges();
            }
            catch
            {
                return View();
            }
            
            return View();
        }
    }
}