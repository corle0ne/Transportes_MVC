using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTO;
using Transportes_MVC.Models;

namespace Transportes_MVC.Controllers
{
    public class CamionesController : Controller
    {
        // GET: Camiones
        public ActionResult Index()
        {
            List<Camiones> lista_camiones = new List<Camiones>();

            using (TransportesEntities context = new TransportesEntities())
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

        //GET Nuevo_Camion

        public ActionResult Nuevo_Camion()
        {
            ViewBag.Titulo = "Nuevo Camion";
            //cargo el ddl con las opciones del tipo camion
            cargarDDL();
            return View();
        }

        //POST: Nuevo_Camion

        [HttpPost]
        public ActionResult Nuevo_Camion(Camiones_DTO model, HttpPostedFileBase imagen)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    using (TransportesEntities context = new TransportesEntities())
                    {
                        var camion = new Camiones();

                        camion.Matricula = model.Matricula;
                        camion.Marca = model.Marca;
                        camion.Modelo = model.Modelo;
                        camion.Tipo_Camion = model.Tipo_Camion;
                        camion.Capacidad = model.Capacidad;
                        camion.Kilometraje = model.Kilometraje;
                        camion.Disponibilidad = model.Disponibilidad;

                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename=Path.GetFileName(imagen.FileName);
                            string pathdir = Server.MapPath("~/Assets/Imagenes/Camiones/");
                            if (!Directory.Exists(pathdir))
                            {
                                Directory.CreateDirectory(pathdir);
                            }



                            imagen.SaveAs(pathdir + filename);
                            camion.UrlFoto = "/Assets/Imagenes/Camiones" + filename;

                            context.Camiones.Add(camion);
                            context.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            cargarDDL();
                            return View(model);
                        }
                    }
                }
                else
                {
                    cargarDDL();
                    return View(model);
                }
            }
            catch
            {
                //en caso de que suceda un error, voy a mostrar un mensaje con error sweet alert, voy a devolverle a la vista el modelo que causo el confilcto (return View(model)) y vuelvo a cargar el ddl para que esten disponibles esas opciones
                cargarDDL();
                return View(model);
                throw;
            }
        }

        #region Auxiliares
        private class Opciones
        {
            public string Numero { get; set; }
            public string Descripcion { get; set; }
        }

        public void cargarDDL()
        {
            List<Opciones> lista_opciones = new List<Opciones>()
            {
                new Opciones () { Numero="0", Descripcion="Seleccione una opcion"},
                new Opciones () { Numero="1", Descripcion="Volteo"},
                new Opciones () { Numero="2", Descripcion="Redilas"},
                new Opciones () { Numero="3", Descripcion="Transporte"}
            };
            ViewBag.ListaTipos = lista_opciones;
        }
        #endregion
    }
}