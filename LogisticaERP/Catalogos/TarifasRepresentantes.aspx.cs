using System;
using System.Web;
using System.Linq;
using System.Web.UI;
using LogisticaERP.Clases;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using LogisticaERP.GrupoPinsaSOA;
using System.Runtime.Serialization;
using LogisticaERP.LogisticaSOA;
using DevExpress.Web;
using System.ServiceModel;
using System.Web.Services;

namespace LogisticaERP.Catalogos
{
    public partial class TarifasRepresentantes : PaginaBase
	{
        private int _numeroRegistro = 0;
        private int _numeroRegistrosTotalUpdate = 0;
        private int _numeroRegistrosTotalInsert = 0;
        private int _numeroRegistrosTotalDelete = 0;

        [WebMethod]
        public static dynamic ObtenerMunicipios()
        {
            try
            {
                List<Tuple<string, decimal, decimal>> listaDatos = new List<Tuple<string, decimal, decimal>>();

                if (HttpContext.Current.Session["TAREPRE_Municipios"] != null)
                {
                    (HttpContext.Current.Session["TAREPRE_Municipios"] as List<Municipio>).ToList().ForEach(t =>
                    {
                        listaDatos.Add(new Tuple<string, decimal, decimal>(t.Nombre_municipio, t.Id_municipio, t.Id_estado));
                    });
                }

                return new { datos = listaDatos, resultado = true, mensaje = "ok" };
            }
            catch (FaultException<GrupoPinsaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                return new { datos = "", resultado = false, mensaje = Faultexc.Message };
            }
            catch (Exception exc)
            {
                return new { datos = "", resultado = false, mensaje = exc.Message };
            }
        }

        [WebMethod]
        public static dynamic ObtenerCiudades()
        {
            try
            {
                List<Tuple<string, decimal, decimal>> listaDatos = new List<Tuple<string, decimal, decimal>>();

                if (HttpContext.Current.Session["TAREPRE_Ciudades"] != null)
                {
                    (HttpContext.Current.Session["TAREPRE_Ciudades"] as List<Ciudad>).ToList().ForEach(t =>
                    {
                        listaDatos.Add(new Tuple<string, decimal, decimal>(t.Nombre_ciudad, t.Id_ciudad, t.Id_municipio));
                    });
                }

                return new { datos = listaDatos, resultado = true, mensaje = "ok" };
            }
            catch (FaultException<GrupoPinsaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                return new { datos = "", resultado = false, mensaje = Faultexc.Message };
            }
            catch (Exception exc)
            {
                return new { datos = "", resultado = false, mensaje = exc.Message };
            }
        }

        protected void Page_Load(object sender, EventArgs e)
		{

			if (!IsPostBack)
			{
                Session["TAREPRE_Estados"] = null;
                Session["TAREPRE_Municipios"] = null;
                Session["TAREPRE_Almacenes"] = null;
                Session["TAR_TarifasRepre"] = null;
                Session["TAREPRE_Ciudades"] = null;

                CargaInicial();
                CargarCombosGrid();
                CargarTarifas();
            }
            else
            {
                CargarGrid();
            }

		}

        protected void gridTarifas_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            e.Editor.ReadOnly = false;

            try
            {
                if (e.Column.FieldName == "Id_estado")
                {

                   /* ASPxComboBox cmb = e.Editor as ASPxComboBox;
                    cmb.DataSource = (Session["TAREPRE_Estados"] as List<Estado>).OrderBy(x => x.Nombre).ToList();
                    cmb.ValueField = "Id_estado";
                    cmb.TextField = "Nombre";
                    cmb.DataBind();*/
                }
            }
            catch (Exception ex)
            {
                gridTarifas.JSProperties["cpMensajeError"] = ex.Message;
            }
        }

        protected void gridTarifas_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            _numeroRegistrosTotalUpdate = e.UpdateValues.Count;

            _numeroRegistrosTotalInsert = e.InsertValues.Count;

            _numeroRegistrosTotalDelete = e.DeleteValues.Count;  
        }

        protected void gridTarifas_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid> ListaTarifas = new List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid>();
                LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid tarifa = new LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid();
                _numeroRegistro = _numeroRegistro + 1;

                if (Session["TAR_TarifasRepre"] != null)
                    ListaTarifas = Session["TAR_TarifasRepre"] as List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid>;

                tarifa.Id_index = ListaTarifas.Count > 0 ? ListaTarifas.OrderBy(x => x.Id_index).Last().Id_index + 1 : 1;
                tarifa.Id_almacen_origen_ebs12 = Convert.ToDecimal(e.NewValues["Id_almacen_origen_ebs12"]);
                tarifa.Id_estado = Convert.ToDecimal(e.NewValues["Id_estado"]);
                tarifa.Id_municipio = Convert.ToDecimal(e.NewValues["Id_municipio"]);
                tarifa.Id_ciudad = Convert.ToDecimal(e.NewValues["Id_ciudad"]);
                tarifa.Zona = Convert.ToString(e.NewValues["Zona"]);
                tarifa.Kilometros = Convert.ToDecimal(e.NewValues["Kilometros"]);
                tarifa.Kilometros_min = Convert.ToDecimal(e.NewValues["Kilometros_min"]);
                tarifa.Cajas = Convert.ToDecimal(e.NewValues["Cajas"]);
                tarifa.Costo_caja = Convert.ToDecimal(e.NewValues["Costo_caja"]);
                tarifa.Activo = Convert.ToBoolean(e.NewValues["Activo"]);
                tarifa.Borrado = false;
                tarifa.Modifico = true;

                ListaTarifas.Add(tarifa);

                Session["TAR_TarifasRepre"] = ListaTarifas;

                if (_numeroRegistro == _numeroRegistrosTotalInsert)
                {
                    if (GrabarTarifas(ListaTarifas.Where(x => x.Modifico == true).ToList()))
                    {
                        gridTarifas.JSProperties["cpExito"] = true;
                        CargarTarifas();
                    }
                }
            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                gridTarifas.JSProperties["cpMensajeAdvertencia"] = Faultexc.Detail.Mensaje;
            }
            catch (Exception ex)
            {
                gridTarifas.JSProperties["cpMensajeError"] = ex.Message;
            }

            e.Cancel = true;
            gridTarifas.CancelEdit();
        }
        
        protected void gridTarifas_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid> ListaTarifas = new List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid>();
                LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid tarifa = new LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid();
                _numeroRegistro = _numeroRegistro + 1;

                ListaTarifas = Session["TAR_TarifasRepre"] as List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid>;
                tarifa = ListaTarifas.Where(x => x.Id_index == Convert.ToDecimal(e.Keys["Id_index"])).FirstOrDefault();

                tarifa.Id_estado = Convert.ToDecimal(e.NewValues["Id_estado"]);
                tarifa.Id_municipio = Convert.ToDecimal(e.NewValues["Id_municipio"]);
                tarifa.Id_ciudad = Convert.ToDecimal(e.NewValues["Id_ciudad"]);
                tarifa.Id_almacen_origen_ebs12 = Convert.ToDecimal(e.NewValues["Id_almacen_origen_ebs12"]);
                tarifa.Zona = Convert.ToString(e.NewValues["Zona"]);
                tarifa.Kilometros = Convert.ToDecimal(e.NewValues["Kilometros"]);
                tarifa.Kilometros_min = Convert.ToDecimal(e.NewValues["Kilometros_min"]);
                tarifa.Cajas = Convert.ToDecimal(e.NewValues["Cajas"]);
                tarifa.Costo_caja = Convert.ToDecimal(e.NewValues["Costo_caja"]);
                tarifa.Activo = Convert.ToBoolean(e.NewValues["Activo"]);
                tarifa.Borrado = false;
                tarifa.Modifico = true;

                ListaTarifas.ToList().Add(tarifa);

                if (_numeroRegistro == _numeroRegistrosTotalUpdate)
                {
                    if (GrabarTarifas(ListaTarifas.Where(x => x.Modifico == true).ToList()))
                    {
                        gridTarifas.JSProperties["cpExito"] = true;
                        CargarTarifas();
                        
                    }
                }
            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                gridTarifas.JSProperties["cpMensajeAdvertencia"] = Faultexc.Detail.Mensaje;
            }
            catch (Exception ex)
            {
                gridTarifas.JSProperties["cpMensajeError"] = ex.Message;
            }

            e.Cancel = true;
            gridTarifas.CancelEdit();
        }

        protected void gridTarifas_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid> listaTarifas = new List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid>();
                LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid tarifa = new LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid();
                _numeroRegistro = _numeroRegistro + 1;

                listaTarifas = Session["TAR_TarifasRepre"] as List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid>;
                tarifa = listaTarifas.Where(x => x.Id_index == Convert.ToDecimal(e.Keys["Id_index"])).FirstOrDefault();
                tarifa.Borrado = true;
                tarifa.Modifico = true;

                listaTarifas.ToList().Add(tarifa);

                if (_numeroRegistro == _numeroRegistrosTotalDelete)
                {
                    if (GrabarTarifas(listaTarifas.Where(x => x.Modifico == true).ToList()))
                    {
                        gridTarifas.JSProperties["cpExito"] = true;
                        CargarTarifas();
                    }
                }
            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                gridTarifas.JSProperties["cpMensajeAdvertencia"] = Faultexc.Detail.Mensaje;
            }
            catch (Exception ex)
            {
                gridTarifas.JSProperties["cpMensajeError"] = ex.Message;
            }

            e.Cancel = true;
            gridTarifas.CancelEdit();
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid> ListaTarifas = Session["TAR_TarifasRepre"] == null ? new List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid>() : Session["TAR_TarifasRepre"] as List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid>;
                if (ListaTarifas != null && ListaTarifas.Count > 0)
                {
                    string nombreArchivo = "Tarifas_" + DateTime.Now.Date.ToString();
                    gridTarifasExportar.WriteXlsxToResponse(nombreArchivo, true);
                }
            }
            catch (Exception ErrorExecion)
            {
                ControladorMensajes.MostrarMensaje(UpdatePanel1, "ExportarGridExcel", ControladorMensajes.TipoMensaje.Advertencia, ErrorExecion.Message, 7000);
            }
        }

        private void CargaInicial()
        {
            try
            {
                // Llamar a API REST de Transportista y Almacenes.
                EBS12_ALMACENES almanceOrigen = new EBS12_ALMACENES();
                if (almanceOrigen.ObtenerAlmacenes(Convert.ToDecimal(Session["EmpresaID"]), 2)) //SessionHelper.Empresa.Id_empresa,2))
                    Session["TAREPRE_Almacenes"] = almanceOrigen.Almacenes.items.ToList();
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, almanceOrigen.Almacenes.mensaje);

                var estados = new GPO_ESTADOS().ObtenerEstados(null, null, null, 1);
                if (estados.Any())
                    Session["TAREPRE_Estados"] = estados;
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe Estados para el país Mexico.");

                var municipios = new GPO_MUNICIPIOS().ObtenerMunicipios();
                if (municipios.Any())
                    Session["TAREPRE_Municipios"] = municipios;
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe Municipios para el país Mexico.");

                var ciudades = new GPO_CIUDADES().ObtenerCiudades();
                if (ciudades.Any())
                    Session["TAREPRE_Ciudades"] = ciudades;
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe Ciudades para el país Mexico.");
            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, Faultexc.Detail.ExcDetalle.Mensaje);
            }
            catch (FaultException<GrupoPinsaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, Faultexc.Detail.ExcDetalle.Mensaje);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Error, ex.Message);
            }
        }

        protected void CargarCombosGrid()
        {
            List<Estado> estados = new List<Estado>();

            if (Session["TAREPRE_Estados"] != null)
                estados = (Session["TAREPRE_Estados"] as List<Estado>);

            GridViewDataComboBoxColumn col = gridTarifas.Columns["Estado"] as GridViewDataComboBoxColumn;
            col.PropertiesComboBox.DataSource = estados.OrderBy(x => x.Nombre).ToList();
            col.PropertiesComboBox.TextField = "Nombre";
            col.PropertiesComboBox.ValueField = "Id_estado";
            col.PropertiesComboBox.ValueType = typeof(Int32);

            List<Municipio> municipio = new List<Municipio>();

            if (Session["TAREPRE_Municipios"] != null)
                municipio = (Session["TAREPRE_Municipios"] as List<Municipio>);

            GridViewDataComboBoxColumn colMun = gridTarifas.Columns["Municipio"] as GridViewDataComboBoxColumn;
            colMun.PropertiesComboBox.DataSource = municipio.OrderBy(x => x.Nombre_municipio).ToList();
            colMun.PropertiesComboBox.TextField = "Nombre_municipio";
            colMun.PropertiesComboBox.ValueField = "Id_municipio";
            colMun.PropertiesComboBox.ValueType = typeof(Int32);

            List<Ciudad> ciudades = new List<Ciudad>();

            if (Session["TAREPRE_Ciudades"] != null)
                ciudades = (Session["TAREPRE_Ciudades"] as List<Ciudad>);

            GridViewDataComboBoxColumn colCiudad = gridTarifas.Columns["Ciudad"] as GridViewDataComboBoxColumn;
            colCiudad.PropertiesComboBox.DataSource = ciudades.OrderBy(x => x.Nombre_ciudad).ToList();
            colCiudad.PropertiesComboBox.TextField = "Nombre_ciudad";
            colCiudad.PropertiesComboBox.ValueField = "Id_ciudad";
            colCiudad.PropertiesComboBox.ValueType = typeof(Int32);


            List<EBS12_ALMACENES.Almacen.ItemsAlmacenes> ListaAlmacenes = new List<EBS12_ALMACENES.Almacen.ItemsAlmacenes>();

            if (Session["TAREPRE_Almacenes"] != null)
                ListaAlmacenes = (Session["TAREPRE_Almacenes"] as List<EBS12_ALMACENES.Almacen.ItemsAlmacenes>);

            GridViewDataComboBoxColumn col4 = gridTarifas.Columns["Almacen Origen"] as GridViewDataComboBoxColumn;
            col4.PropertiesComboBox.DataSource = ListaAlmacenes.ToList();
            col4.PropertiesComboBox.TextField = "almacen";
            col4.PropertiesComboBox.ValueField = "idAlmacen";
            col4.PropertiesComboBox.ValueType = typeof(Int32);
            gridTarifas.DataBind();

        }

        private void CargarGrid()
        {
            CargarCombosGrid();

            List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid> ListaTarifas = new List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid>();

            if (Session["TAR_TarifasRepre"] != null)
                ListaTarifas = Session["TAR_TarifasRepre"] as List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid>;

            gridTarifas.DataSource = ListaTarifas;
            gridTarifas.KeyFieldName = "Id_index";
            gridTarifas.DataBind();

        }

        public void CargarTarifas()
        {
            try
            {
                LOGI_TARIFAS_REPRESENTANTES orden = new LOGI_TARIFAS_REPRESENTANTES();

                List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid> ListaArticulosStock = new List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid>();

                orden.Tarifas.Id_empresa = Convert.ToDecimal(Session["EmpresaID"]);
                orden.Tarifas.Activo = true;

                if (orden.Busqueda())
                {

                    ListaArticulosStock = (from l in orden.ListaTarifas
                                           select new LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid()
                                           {
                                               Id_tarifa_representante = l.Id_tarifa_representante,
                                               Id_index = l.Id_tarifa_representante,
                                               Id_estado = l.Id_estado,
                                               Id_municipio = l.Id_municipio,
                                               Id_ciudad = l.Id_ciudad,
                                               Id_almacen_origen_ebs12 = l.Id_almacen_origen_ebs12,
                                               Zona = l.Zona,
                                               Kilometros = l.Kilometros,
                                               Kilometros_min = l.Kilometros_min,
                                               Cajas = l.Monto_caja,
                                               Costo_caja = l.Monto_costo,
                                               Activo = l.Activo.Value,
                                               CodAlmacen = l.Nombre_almacen_ebs12,
                                               Estado = l.Estado,
                                               Ciudad = l.Ciudad,
                                               Modifico = false,
                                           }).ToList();
                }

                Session["TAR_TarifasRepre"] = ListaArticulosStock;
                CargarGrid();

            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, Faultexc.Detail.ExcDetalle.Mensaje);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Error, ex.Message);
            }
        }

        protected bool GrabarTarifas(List<LOGI_TARIFAS_REPRESENTANTES.TarifasRepresentanteGrid> tarifas)
        {
            bool resultado = false;

            if (!EsUsuarioGuardar)
            {
                throw new Exception("El usuario no tiene los permisos requeridos.");
            }

            try
            {
                LOGI_TARIFAS_REPRESENTANTES tarifa = new LOGI_TARIFAS_REPRESENTANTES();
                List<TarifaRepresentante> listaTarifas = new List<TarifaRepresentante>();

                listaTarifas = (from l in tarifas
                                select new TarifaRepresentante()
                                {
                                    Id_tarifa_representante = l.Id_tarifa_representante,
                                    Id_empresa = Convert.ToDecimal(Session["EmpresaID"]),
                                    Id_estado = l.Id_estado.Value,
                                    Id_municipio = l.Id_municipio.Value,
                                    Id_ciudad = l.Id_ciudad.Value,
                                    Id_almacen_origen_ebs12 = l.Id_almacen_origen_ebs12.Value,
                                    Nombre_almacen_ebs12 = (Session["TAREPRE_Almacenes"] as List<EBS12_ALMACENES.Almacen.ItemsAlmacenes>).Where(x => x.idAlmacen == l.Id_almacen_origen_ebs12).FirstOrDefault().codAlmacen,
                                    Zona = l.Zona,
                                    Kilometros = l.Kilometros,
                                    Kilometros_min = l.Kilometros_min,
                                    Monto_caja = l.Cajas,
                                    Monto_costo = l.Costo_caja,
                                    Borrado = l.Borrado,
                                    Activo = l.Activo
                                }).ToList();


                listaTarifas = tarifa.GrabarListaTarifasRepresentante(listaTarifas);

                if (listaTarifas.Count  > 0)
                {

                    /*EBS12_TARIFAS_REPRESENTANTES tarifaRe = new EBS12_TARIFAS_REPRESENTANTES();

                    tarifaRe.TarifaRepresentante.items = (from l in listaTarifas
                                                          select new EBS12_TARIFAS_REPRESENTANTES.TarifaRepre.ItemsTarifaRepre()
                                           {
                                               idTarifaRepresentante = l.Id_tarifa_representante,
                                               origen = null, //ELIMINAR LOS CRACTERES EXTRAÑOS EN LA CIUDAD ESTADO Y ALMACEN
                                               codOrganizacion = (Session["TAREPRE_Almacenes"] as List<EBS12_ALMACENES.Almacen.ItemsAlmacenes>).Where(x => x.idAlmacen == l.Id_almacen_origen_ebs12).FirstOrDefault().codAlmacen,
                                               destCiudad = (Session["TAREPRE_Ciudades"] as List<Ciudad>).Where(x => x.Id_ciudad == l.Id_ciudad).FirstOrDefault().Nombre_ciudad != "N/A" ? (Session["TAREPRE_Ciudades"] as List<Ciudad>).Where(x => x.Id_ciudad == l.Id_ciudad).FirstOrDefault().Nombre_ciudad : (Session["TAREPRE_Municipios"] as List<Municipio>).Where(x => x.Id_municipio == l.Id_municipio).FirstOrDefault().Nombre_municipio,
                                               destEstado = (Session["TAREPRE_Estados"] as List<Estado>).Where(x=> x.Id_estado == l.Id_estado).FirstOrDefault().Nombre,
                                               zona = l.Zona,
                                               cajas = l.Monto_caja,
                                               kilometros = l.Kilometros,
                                               kilometrosMin = l.Kilometros_min,
                                               costoCaja = l.Monto_costo,
                                               activo = l.Activo.Value ? 1 : 0
                                           }).ToList();


                    string JsonMaestro = "";
                    var Json = tarifaRe; 

                    JsonMaestro = Newtonsoft.Json.JsonConvert.SerializeObject(Json);
                    tarifaRe = tarifaRe.GrabarTarifaRepresentante(JsonMaestro);

                    if (tarifaRe.TarifaRepresentante.resultado == "No")
                    {
                        MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, tarifaRe.TarifaRepresentante.mensaje);
                    }*/

                    resultado = true;
                }
                    
            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                throw Faultexc;
            }
            catch (Exception ex)
            {
                throw ex;

            }

            return resultado;
        }

        private void MostrarMensaje(ControladorMensajes.TipoMensaje tipo, dynamic sms, int? tiempoMostrar = 7000)
        {
            ControladorMensajes.MostrarMensaje(UpdatePanel1, string.Format("TAR_{0}{1}", tipo, new Random().Next(0, 99)), tipo, sms, tiempoMostrar);
        }

	}
}