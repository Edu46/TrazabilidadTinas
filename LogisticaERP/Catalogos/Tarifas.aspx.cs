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
    public partial class Tarifas : PaginaBase
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

                if (HttpContext.Current.Session["TAR_Municipios"] != null)
                {
                    (HttpContext.Current.Session["TAR_Municipios"] as List<Municipio>).ToList().ForEach(t =>
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

                if (HttpContext.Current.Session["TAR_Ciudades"] != null)
                {
                    (HttpContext.Current.Session["TAR_Ciudades"] as List<Ciudad>).ToList().ForEach(t =>
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
                Session["TAR_CodigosTransporte"] = null;
                Session["TAR_Estados"] = null;
                Session["TAR_Municipios"] = null;
                Session["TAR_Tarifas"] = null;
                Session["TAR_Transportista"] = null;
                Session["TAR_Almacenes"] = null;
                Session["TAR_Ciudades"] = null;

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
                    cmb.DataSource = (Session["TAR_Estados"] as List<Estado>).OrderBy(x => x.Nombre).ToList();
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

        private void CargaInicial()
        {
            try
            {

                // Llamar a API REST de Transportista y Almacenes.


                var estados = new GPO_ESTADOS().ObtenerEstados(null, null, null, 1);
                if (estados.Any())
                    Session["TAR_Estados"] = estados;
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe Estados para el país Mexico.");

                var municipios = new GPO_MUNICIPIOS().ObtenerMunicipios();
                if (municipios.Any())
                    Session["TAR_Municipios"] = municipios;
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe Municipios para el país Mexico.");

                var ciudades = new GPO_CIUDADES().ObtenerCiudades();
                if (ciudades.Any())
                    Session["TAR_Ciudades"] = ciudades;
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe Ciudades para el país Mexico.");

                EBS12_TRANSPORTISTAS transportista = new EBS12_TRANSPORTISTAS();
                if (transportista.ObtenerTransportistas())
                    Session["TAR_Transportista"] = transportista.Transportistas.items.ToList();
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, transportista.Transportistas.mensaje);

                EBS12_ALMACENES almanceOrigen = new EBS12_ALMACENES();
                if (almanceOrigen.ObtenerAlmacenes(Convert.ToDecimal(Session["EmpresaID"]), 0))  //(SessionHelper.Empresa.Id_empresa,1))
                    Session["TAR_Almacenes"] = almanceOrigen.Almacenes.items.ToList();
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, almanceOrigen.Almacenes.mensaje);

                LOGI_CODIGOS_TRANSPORTES codigo = new LOGI_CODIGOS_TRANSPORTES();
                codigo.CodigosTarifas.Id_empresa = Convert.ToDecimal(Session["EmpresaID"]);
                codigo.CodigosTarifas.Activo = true;

                if (codigo.Busqueda())
                    Session["TAR_CodigosTransporte"] = codigo.ListaCodigosTarifas;
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existen codigos de Transporte para la empresa seleccionada.");


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

            if (Session["TAR_Estados"] != null)
                estados = (Session["TAR_Estados"] as List<Estado>);

            GridViewDataComboBoxColumn col = gridTarifas.Columns["Estado"] as GridViewDataComboBoxColumn;
            col.PropertiesComboBox.DataSource = estados.OrderBy(x => x.Nombre).ToList();
            col.PropertiesComboBox.TextField = "Nombre";
            col.PropertiesComboBox.ValueField = "Id_estado";
            col.PropertiesComboBox.ValueType = typeof(Int32);

            List<Municipio> municipio = new List<Municipio>();

            if (Session["TAR_Municipios"] != null)
                municipio = (Session["TAR_Municipios"] as List<Municipio>);

            GridViewDataComboBoxColumn colMun = gridTarifas.Columns["Municipio"] as GridViewDataComboBoxColumn;
            colMun.PropertiesComboBox.DataSource = municipio.OrderBy(x => x.Nombre_municipio).ToList();
            colMun.PropertiesComboBox.TextField = "Nombre_municipio";
            colMun.PropertiesComboBox.ValueField = "Id_municipio";
            colMun.PropertiesComboBox.ValueType = typeof(Int32);

            List<Ciudad> ciudades = new List<Ciudad>();

            if (Session["TAR_Ciudades"] != null)
                ciudades = (Session["TAR_Ciudades"] as List<Ciudad>);

            GridViewDataComboBoxColumn colCiudad = gridTarifas.Columns["Ciudad"] as GridViewDataComboBoxColumn;
            colCiudad.PropertiesComboBox.DataSource = ciudades.OrderBy(x => x.Nombre_ciudad).ToList();
            colCiudad.PropertiesComboBox.TextField = "Nombre_ciudad";
            colCiudad.PropertiesComboBox.ValueField = "Id_ciudad";
            colCiudad.PropertiesComboBox.ValueType = typeof(Int32);

            List<CodigoTransporte> codigos = new List<CodigoTransporte>();

            if (Session["TAR_CodigosTransporte"] != null)
                codigos = (Session["TAR_CodigosTransporte"] as List<CodigoTransporte>);

            GridViewDataComboBoxColumn colPrioridad = gridTarifas.Columns["Cod. Transporte"] as GridViewDataComboBoxColumn;
            colPrioridad.PropertiesComboBox.DataSource = codigos.ToList();
            colPrioridad.PropertiesComboBox.TextField = "Codigo_transporte";
            colPrioridad.PropertiesComboBox.ValueField = "Id_codigo_transporte";
            colPrioridad.PropertiesComboBox.ValueType = typeof(Int32);
            gridTarifas.DataBind();

            List<EBS12_TRANSPORTISTAS.Transportista.ItemsTransportistas> trans = new List<EBS12_TRANSPORTISTAS.Transportista.ItemsTransportistas>();

            if (Session["TAR_Transportista"] != null)
                trans = (Session["TAR_Transportista"] as List<EBS12_TRANSPORTISTAS.Transportista.ItemsTransportistas>);

            GridViewDataComboBoxColumn col3 = gridTarifas.Columns["Proveedor"] as GridViewDataComboBoxColumn;
            col3.PropertiesComboBox.DataSource = trans.ToList();
            col3.PropertiesComboBox.TextField = "nombreTransportista";
            col3.PropertiesComboBox.ValueField = "idTransportista";
            col3.PropertiesComboBox.ValueType = typeof(Int32);
            gridTarifas.DataBind();


            List<EBS12_ALMACENES.Almacen.ItemsAlmacenes> ListaAlmacenes = new List<EBS12_ALMACENES.Almacen.ItemsAlmacenes>();

            if (Session["TAR_Almacenes"] != null)
                ListaAlmacenes = (Session["TAR_Almacenes"] as List<EBS12_ALMACENES.Almacen.ItemsAlmacenes>);

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

            List<LOGI_TARIFAS.TarifasGrid> ListaTarifas = new List<LOGI_TARIFAS.TarifasGrid>();

            if (Session["TAR_Tarifas"] != null)
                ListaTarifas = Session["TAR_Tarifas"] as List<LOGI_TARIFAS.TarifasGrid>;

            gridTarifas.DataSource = ListaTarifas;
            gridTarifas.KeyFieldName = "Id_index";
            gridTarifas.DataBind();

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
                List<LOGI_TARIFAS.TarifasGrid> ListaTarifas = new List<LOGI_TARIFAS.TarifasGrid>();
                LOGI_TARIFAS.TarifasGrid tarifa = new LOGI_TARIFAS.TarifasGrid();
                _numeroRegistro = _numeroRegistro + 1;

                if (Session["TAR_Tarifas"] != null)
                    ListaTarifas = Session["TAR_Tarifas"] as List<LOGI_TARIFAS.TarifasGrid>;

                tarifa.Id_index = ListaTarifas.Count > 0 ? ListaTarifas.OrderBy(x => x.Id_index).Last().Id_index + 1 : 1;
                tarifa.Id_estado = Convert.ToDecimal(e.NewValues["Id_estado"]);
                tarifa.Id_municipio = Convert.ToDecimal(e.NewValues["Id_municipio"]);
                tarifa.Id_ciudad = Convert.ToDecimal(e.NewValues["Id_ciudad"]);
                tarifa.Id_tipo_tarifa = Convert.ToDecimal(e.NewValues["Id_tipo_tarifa"]);
                tarifa.Id_codigo_transporte = Convert.ToDecimal(e.NewValues["Id_codigo_transporte"]);
                tarifa.Id_proveedor_ebs12 = Convert.ToDecimal(e.NewValues["Id_proveedor_ebs12"]);
                tarifa.Monto_tarifa = Convert.ToDecimal(e.NewValues["Monto_tarifa"]);
                tarifa.Monto_caseta = Convert.ToDecimal(e.NewValues["Monto_caseta"]);
                tarifa.Fecha_inicio = Convert.ToDateTime(e.NewValues["Fecha_inicio"]);
                tarifa.Fecha_fin = Convert.ToDateTime(e.NewValues["Fecha_fin"]);
                tarifa.Id_almacen_origen_ebs12 = Convert.ToDecimal(e.NewValues["Id_almacen_origen_ebs12"]);
                tarifa.Backhaul = Convert.ToBoolean(e.NewValues["Backhaul"]);
                tarifa.Activo = Convert.ToBoolean(e.NewValues["Activo"]);
                tarifa.Borrado = false;
                tarifa.Modifico = true;

                ListaTarifas.Add(tarifa);

                Session["TAR_Tarifas"] = ListaTarifas;

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
                List<LOGI_TARIFAS.TarifasGrid> ListaTarifas = new List<LOGI_TARIFAS.TarifasGrid>();
                LOGI_TARIFAS.TarifasGrid tarifa = new LOGI_TARIFAS.TarifasGrid();
                _numeroRegistro = _numeroRegistro + 1;

                ListaTarifas = Session["TAR_Tarifas"] as List<LOGI_TARIFAS.TarifasGrid>;
                tarifa = ListaTarifas.Where(x => x.Id_index == Convert.ToDecimal(e.Keys["Id_index"])).FirstOrDefault();

                tarifa.Id_estado = Convert.ToDecimal(e.NewValues["Id_estado"]);
                tarifa.Id_municipio = Convert.ToDecimal(e.NewValues["Id_municipio"]);
                tarifa.Id_ciudad = Convert.ToDecimal(e.NewValues["Id_ciudad"]);
                tarifa.Id_tipo_tarifa = Convert.ToDecimal(e.NewValues["Id_tipo_tarifa"]);
                tarifa.Id_codigo_transporte = Convert.ToDecimal(e.NewValues["Id_codigo_transporte"]);
                tarifa.Id_proveedor_ebs12 = Convert.ToDecimal(e.NewValues["Id_proveedor_ebs12"]);
                tarifa.Monto_tarifa = Convert.ToDecimal(e.NewValues["Monto_tarifa"]);
                tarifa.Monto_caseta = Convert.ToDecimal(e.NewValues["Monto_caseta"]);
                tarifa.Fecha_inicio = Convert.ToDateTime(e.NewValues["Fecha_inicio"]);
                tarifa.Fecha_fin = Convert.ToDateTime(e.NewValues["Fecha_fin"]);
                tarifa.Id_almacen_origen_ebs12 = Convert.ToDecimal(e.NewValues["Id_almacen_origen_ebs12"]);
                tarifa.Backhaul = Convert.ToBoolean(e.NewValues["Backhaul"]);
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


        protected bool GrabarTarifas(List<LOGI_TARIFAS.TarifasGrid> tarifas)
        {
            bool resultado = false;

            if (!EsUsuarioGuardar)
            {
                throw new Exception("El usuario no tiene los permisos requeridos.");
            }

            try
            {
                LOGI_TARIFAS tarifa = new LOGI_TARIFAS();
                List<Tarifa> listaTarifas = new List<Tarifa>();

                listaTarifas = (from l in tarifas
                                    select new Tarifa()
                                    {
                                        Id_tarifa = l.Id_tarifa,
                                        Id_empresa = Convert.ToDecimal(Session["EmpresaID"]),
                                        Id_estado = l.Id_estado.Value,
                                        Id_municipio = l.Id_municipio.Value,
                                        Id_ciudad = l.Id_ciudad.Value,
                                        Id_tipo_tarifa = l.Id_tipo_tarifa,
                                        Id_codigo_transporte = l.Id_codigo_transporte.Value,
                                        Id_proveedor_ebs12 = l.Id_proveedor_ebs12,
                                        Monto_tarifa = l.Monto_tarifa,
                                        Monto_caseta = l.Monto_caseta,
                                        Fecha_inicio = l.Fecha_inicio,
                                        Fecha_fin = l.Fecha_fin,
                                        Id_almacen_origen_ebs12 = l.Id_almacen_origen_ebs12,
                                        Nombre_proveedor_ebs12 = (Session["TAR_Transportista"] as List<EBS12_TRANSPORTISTAS.Transportista.ItemsTransportistas>).Where(x=> x.idTransportista == l.Id_proveedor_ebs12).FirstOrDefault().nombreTransportista,
                                        Centro_costo_almacen = (Session["TAR_Almacenes"] as List<EBS12_ALMACENES.Almacen.ItemsAlmacenes>).Where(x => x.idAlmacen == l.Id_almacen_origen_ebs12).FirstOrDefault().centroCosto,
                                        Nombre_almacen_ebs12 =(Session["TAR_Almacenes"] as List<EBS12_ALMACENES.Almacen.ItemsAlmacenes>).Where(x=> x.idAlmacen == l.Id_almacen_origen_ebs12).FirstOrDefault().codAlmacen,
                                        Backhaul = l.Backhaul,
                                        Borrado = l.Borrado,
                                        Activo = l.Activo,
                                    }).ToList();


                if (tarifa.GrabarListaTarifas(listaTarifas))
                    resultado = true;
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

        protected void gridTarifas_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                List<LOGI_TARIFAS.TarifasGrid> listaTarifas = new List<LOGI_TARIFAS.TarifasGrid>();
                LOGI_TARIFAS.TarifasGrid tarifa = new LOGI_TARIFAS.TarifasGrid();
                _numeroRegistro = _numeroRegistro + 1;

                listaTarifas = Session["TAR_Tarifas"] as List<LOGI_TARIFAS.TarifasGrid>;
                tarifa = listaTarifas.Where(x => x.Id_index == Convert.ToDecimal(e.Keys["Id_index"])).FirstOrDefault();

                if (tarifa.Id_tarifa > 0)
                {
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
                else
                {
                    listaTarifas.RemoveAll(x => x.Id_index == Convert.ToDecimal(e.Keys["Id_index"]));

                    //Mandar a grabar aquellos registros que tienen tarita -1. Esto se agregara porque se pierde el control de los registros editados en la edicion Batch edit

                    if (listaTarifas.Count > 0 && listaTarifas.Where(x => x.Id_tarifa <= 0).ToList().Count > 0)
                    {
                        if (GrabarTarifas(listaTarifas.Where(x => x.Id_tarifa <= 0).ToList()))
                        {
                            gridTarifas.JSProperties["cpExito"] = true;
                            CargarTarifas();

                        }
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

        protected void gridTarifas_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {

            List<LOGI_TARIFAS.TarifasGrid> ListaTarifas = new List<LOGI_TARIFAS.TarifasGrid>();
            LOGI_TARIFAS.TarifasGrid tarifa = new LOGI_TARIFAS.TarifasGrid();

            ListaTarifas = Session["TAR_Tarifas"] as List<LOGI_TARIFAS.TarifasGrid>;
            tarifa = ListaTarifas.Where(x => x.Id_index == Convert.ToDecimal(e.Keys["Id_index"])).FirstOrDefault();

            List<LOGI_TARIFAS.TarifasGrid> ListaTarifasValidar = new List<LOGI_TARIFAS.TarifasGrid>();


            if (Convert.ToDateTime(e.NewValues["Fecha_inicio"]) > Convert.ToDateTime(e.NewValues["Fecha_fin"]))
            {
                e.RowError = "La Fecha Inicio debe ser menor a la Fecha Fin.";
            }
            else
            {

                if (e.IsNewRow)
                {
                    ListaTarifasValidar = ListaTarifas.Where(x => x.Id_almacen_origen_ebs12 == Convert.ToDecimal(e.NewValues["Id_almacen_origen_ebs12"]) && x.Id_ciudad == Convert.ToDecimal(e.NewValues["Id_ciudad"]) && x.Id_proveedor_ebs12 == Convert.ToDecimal(e.NewValues["Id_proveedor_ebs12"]) && x.Backhaul == (Convert.ToBoolean(e.NewValues["Backhaul"])) && x.Id_codigo_transporte == Convert.ToDecimal(e.NewValues["Id_codigo_transporte"]) && x.Id_tipo_tarifa == Convert.ToDecimal(e.NewValues["Id_tipo_tarifa"]) && x.Borrado == false).ToList();

                    if (ListaTarifasValidar.Exists(x => x.Fecha_fin >= (Convert.ToDateTime(e.NewValues["Fecha_inicio"])) && x.Fecha_inicio <= (Convert.ToDateTime(e.NewValues["Fecha_fin"]))))
                    {
                        e.RowError = "La Tarifa ya se encuentra registrada.";
                    }
                }
                else
                {

                    ListaTarifasValidar = ListaTarifas.Where(x => x.Id_almacen_origen_ebs12 == Convert.ToDecimal(e.NewValues["Id_almacen_origen_ebs12"]) && x.Id_ciudad == Convert.ToDecimal(e.NewValues["Id_ciudad"]) && x.Id_proveedor_ebs12 == Convert.ToDecimal(e.NewValues["Id_proveedor_ebs12"]) && x.Backhaul == Convert.ToBoolean(e.NewValues["Backhaul"]) && x.Id_codigo_transporte == Convert.ToDecimal(e.NewValues["Id_codigo_transporte"]) && x.Id_tipo_tarifa == Convert.ToDecimal(e.NewValues["Id_tipo_tarifa"]) && x.Borrado == false && x.Id_tarifa != tarifa.Id_tarifa).ToList();

                    if (ListaTarifasValidar.Exists(x => x.Fecha_fin >= (Convert.ToDateTime(e.NewValues["Fecha_inicio"])) && x.Fecha_inicio <= (Convert.ToDateTime(e.NewValues["Fecha_fin"]))))
                    {
                        e.RowError = "La Tarifa ya se encuentra registrada.";
                    }
                }

                    //if (ListaTarifas.Exists(x => x.Id_almacen_origen_ebs12 == Convert.ToDecimal(e.NewValues["Id_almacen_origen_ebs12"]) && x.Id_municipio == Convert.ToDecimal(e.NewValues["Id_municipio"]) && x.Id_proveedor_ebs12 == Convert.ToDecimal(e.NewValues["Id_proveedor_ebs12"]) && ((Convert.ToDateTime(e.NewValues["Fecha_inicio"]) >= x.Fecha_inicio && Convert.ToDateTime(e.NewValues["Fecha_inicio"]) <= x.Fecha_fin) || (Convert.ToDateTime(e.NewValues["Fecha_fin"]) >= x.Fecha_inicio && Convert.ToDateTime(e.NewValues["Fecha_fin"]) <= x.Fecha_fin)) && ((Convert.ToDateTime(e.NewValues["Fecha_inicio"]) > x.Fecha_fin || (Convert.ToDateTime(e.NewValues["Fecha_fin"]) < x.Fecha_inicio )) && x.Borrado == false && x.Id_tarifa != tarifa.Id_tarifa))
                    //  e.RowError = "La Tarifa ya se encuentra registrada.";

                    /*if (ListaTarifasValidar.Exists(x => (Convert.ToDateTime(e.NewValues["Fecha_inicio"]) >= x.Fecha_inicio && Convert.ToDateTime(e.NewValues["Fecha_inicio"]) <= x.Fecha_fin) || (Convert.ToDateTime(e.NewValues["Fecha_fin"]) >= x.Fecha_inicio && Convert.ToDateTime(e.NewValues["Fecha_fin"]) <= x.Fecha_fin)))
                    {

                        e.RowError = "La Tarifa ya se encuentra registrada.";

                    }
                    else
                    {
                        if (ListaTarifasValidar.Exists(x =>  x.Fecha_fin > Convert.ToDateTime(e.NewValues["Fecha_fin"])))
                        {
                            var listaFechas = ListaTarifasValidar.Where(x => x.Fecha_fin > Convert.ToDateTime(e.NewValues["Fecha_fin"])).ToList();

                            var xxx = listaFechas.Where(x => x.Fecha_fin > (Convert.ToDateTime(e.NewValues["Fecha_inicio"])) || x.Fecha_inicio > Convert.ToDateTime(e.NewValues["Fecha_fin"]) ).ToList();

                            if (listaFechas.Exists(x => x.Fecha_fin > (Convert.ToDateTime(e.NewValues["Fecha_inicio"])) || x.Fecha_inicio > Convert.ToDateTime(e.NewValues["Fecha_fin"])))
                            {

                                e.RowError = "La Tarifa ya se encuentra registrada.";
                            }

                        }*/
                        /* else
                         {
                             e.RowError = "La Tarifa ya se encuentra registrada.";
                         }*/
                    //}

                //}
            }

        }

        public void CargarTarifas()
        {
            try
            {
                LOGI_TARIFAS orden = new LOGI_TARIFAS();

                List<LOGI_TARIFAS.TarifasGrid> ListaTarifas = new List<LOGI_TARIFAS.TarifasGrid>();

                orden.Tarifas.Id_empresa = Convert.ToDecimal(Session["EmpresaID"]);
                orden.Tarifas.Activo = false;

                if (orden.Busqueda())
                {

                    ListaTarifas = (from l in orden.ListaTarifas
                                           select new LOGI_TARIFAS.TarifasGrid()
                                           {
                                               Id_tarifa = l.Id_tarifa,
                                               Id_index = l.Id_tarifa,
                                               Id_estado = l.Id_estado,
                                               Id_municipio = l.Id_municipio,
                                               Id_ciudad = l.Id_ciudad,
                                               Id_tipo_tarifa = l.Id_tipo_tarifa,
                                               Id_codigo_transporte = l.Id_codigo_transporte,
                                               Id_proveedor_ebs12 = l.Id_proveedor_ebs12,
                                               Id_almacen_origen_ebs12 = l.Id_almacen_origen_ebs12,
                                               Fecha_fin = l.Fecha_fin,
                                               Fecha_inicio = l.Fecha_inicio,   
                                               Monto_tarifa = l.Monto_tarifa,
                                               Monto_caseta = l.Monto_caseta,
                                               Backhaul = l.Backhaul,
                                               Activo = l.Activo.Value,
                                               Modifico = false,
                                           }).ToList();
                }

                Session["TAR_Tarifas"] = ListaTarifas;
                CargarGrid();

                _numeroRegistro = 0;
                _numeroRegistrosTotalUpdate = 0;
                _numeroRegistrosTotalInsert = 0;
                _numeroRegistrosTotalDelete = 0;

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

        private void MostrarMensaje(ControladorMensajes.TipoMensaje tipo, dynamic sms, int? tiempoMostrar = 7000)
        {
            ControladorMensajes.MostrarMensaje(UpdatePanel1, string.Format("TAR_{0}{1}", tipo, new Random().Next(0, 99)), tipo, sms, tiempoMostrar);
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                List<LOGI_TARIFAS.TarifasGrid> ListaTarifas = Session["TAR_Tarifas"] == null ? new List<LOGI_TARIFAS.TarifasGrid>() : Session["TAR_Tarifas"] as List<LOGI_TARIFAS.TarifasGrid>;
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

        protected void gridTarifas_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["Backhaul"] = false;
            e.NewValues["Activo"] = true;
        }



	}
}