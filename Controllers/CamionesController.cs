using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transportes_MVC.Models;

namespace Transportes_MVC.Controllers
{
    public class CamionesController : Controller
    {
        // GET: Camiones
        public ActionResult Index()
        {
            List<Camiones> lista_camiones =new List<Camiones>();

            using (TransportesEntities context=new TransportesEntities())
            {
                //lleno mi lista directamente usando linq
                //lista_camiones=(from camion in context.Camiones select camion).ToList();
                //otra forma
                lista_camiones = context.Camiones.ToList();
            }

            //viewbag(forma parte de razor) se caracteriza por hacer uso de un propiedad arbitraria que sirve para pasar informacion desde el controladora ala vista
            ViewBag.Titulo = "Lista de Camiones";
            ViewBag.Subtitulo = "Utilizando ASP.NET MVC";

            //viewdata se caracteriza por haver uso de un atributo arbitrario y tiene el mismo funcionamiento que el viewbag
            ViewData["Titulo2"] = "Segundo titulo";
            
            //tempdata se caracteriza por permitir crear variables temporales que existen durante la ejecucion del runtime de asp
            //ademas, los temdata me permite compartir informacion no solo del controlador a la vista, sino tambien entre otras vistas y otros controladores
            //TempData.Add("Clave", "valor");

            //retorno la vista con los datos del modelo 
            return View(lista_camiones);
        }
    }
}