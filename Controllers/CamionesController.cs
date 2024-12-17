using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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

        //GET: EDITAR CAMION (ID)

        public ActionResult Editar_Camion(int id)
        {
            if (id > 0)
            {
                Camiones_DTO camion = new Camiones_DTO();
                using (TransportesEntities context = new TransportesEntities())
                {
                    var camion_aux = context.Camiones.Where(x => x.ID_Camion == id).FirstOrDefault();

                    camion.Matricula = camion_aux.Matricula;
                    camion.Marca = camion_aux.Marca;
                    camion.Modelo= camion_aux.Modelo;
                    camion.Capacidad = camion_aux.Capacidad;
                    camion.Kilometraje= camion_aux.Kilometraje;
                    camion.Tipo_Camion= camion_aux.Tipo_Camion;
                    camion.Disponibilidad= camion_aux.Disponibilidad;
                    camion.UrlFoto= camion_aux.UrlFoto;


                    camion=(from c in context.Camiones where c.ID_Camion== id select new Camiones_DTO()
                    {
                        ID_Camion=c.ID_Camion,
                        Matricula=c.Matricula,
                        Marca=c.Marca,
                        Modelo=c.Modelo,
                        Capacidad=c.Capacidad,
                        Kilometraje=c.Kilometraje,
                        Tipo_Camion=c.Tipo_Camion,
                        Disponibilidad=c.Disponibilidad,
                        UrlFoto=c.UrlFoto

                    }).FirstOrDefault();

                }
                if (camion == null)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Titulo = $"Editar Camion#{camion.ID_Camion}";

                cargarDDL();

                return View(camion);
            }
            else
            {
                //sweetalert
                return RedirectToAction("Index");
            }
        }

        //POST: EDITAR CAMION

        //POST: Editar_Camion
        [HttpPost]
        public ActionResult Editar_Camion(Camiones_DTO model, HttpPostedFileBase imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (TransportesEntities context = new TransportesEntities())
                    {
                        var camion = new Camiones();

                        camion.ID_Camion = model.ID_Camion;
                        camion.Matricula = model.Matricula;
                        camion.Marca = model.Marca;
                        camion.Modelo = model.Modelo;
                        camion.Capacidad = model.Capacidad;
                        camion.Tipo_Camion = model.Tipo_Camion;
                        camion.Disponibilidad = model.Disponibilidad;
                        camion.Kilometraje = model.Kilometraje;

                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);
                            string pathdir = Server.MapPath("~/Assets/Imagenes/Camiones/");
                            if (model.UrlFoto.Length == 0)
                            {
                                //la imagen en la BD es null y hay que darle la imagen
                                if (!Directory.Exists(pathdir))
                                {
                                    Directory.CreateDirectory(pathdir);
                                }

                                imagen.SaveAs(pathdir + filename);
                                camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;
                            }
                            else
                            {
                                //validar si es la misma o es nueva
                                if (model.UrlFoto.Contains(filename))
                                {
                                    //es la misma
                                    camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;
                                }
                                else
                                {
                                    //es diferente
                                    if (!Directory.Exists(pathdir))
                                    {
                                        Directory.CreateDirectory(pathdir);
                                    }

                                    //Borro la imagen anterios
                                    //valido si existe

                                    try
                                    {
                                        string pathdir_old = Server.MapPath("~" + model.UrlFoto); //busco la imagen que catualmente tiene el camión
                                        if (System.IO.File.Exists(pathdir_old)) //valido si existe dicho archivo
                                        {
                                            //procedo a eliminarlo
                                            System.IO.File.Delete(pathdir_old);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //Sweet Alert
                                    }

                                    imagen.SaveAs(pathdir + filename);
                                    camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;
                                }
                            }
                        }
                        else //si no hya una nueva imagen, paso la misma
                        {
                            camion.UrlFoto = model.UrlFoto;
                        }

                        //Guardar cambios, validar excepciones, redirigir
                        //actualizar el estado de nuestro elemento
                        //.Entry() registrar la entrada de nueva información al contexto y notificar un cambio de estado usando System.Data.Entity.EntityState.Modified
                        context.Entry(camion).State = System.Data.Entity.EntityState.Modified;
                        //impactamos la BD
                        try
                        {
                            context.SaveChanges();
                        }
                        //agregar using desde 'using System.Data.Entity.Validation;'
                        catch (DbEntityValidationException ex)
                        {
                            string resp = "";
                            //recorro todos los posibles errores de la Entidad Referencial
                            foreach (var error in ex.EntityValidationErrors)
                            {
                                //recorro los detalles de cada error
                                foreach (var validationError in error.ValidationErrors)
                                {
                                    resp += "Error en la Entidad: " + error.Entry.Entity.GetType().Name;
                                    resp += validationError.PropertyName;
                                    resp += validationError.ErrorMessage;
                                }
                            }
                            //Sweet Alert
                        }
                        //Sweet Alert
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    //Sweet Alert
                    cargarDDL();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                //Sweet Alert
                cargarDDL();
                return View(model);
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