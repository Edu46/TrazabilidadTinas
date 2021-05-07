using LogisticaERP.Clases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogisticaERP.Catalogos.CancelacionMasiva
{
    public partial class CancelacionMasiva : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ListaPedidosVenta"] = new List<EBS12_PEDIDOS_VENTA.PedidoVentaGrid>();
                ObtenerEmpresaIntegracion();
                CargarAlmacenes();
            }
            CargarGridPedidosVenta();
        }

        #region Eventos

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ObtenerPedidosVenta();
            CargarGridPedidosVenta();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            dtpFechaInicio.Date = DateTime.Now.Date;
            dtpFechaFin.Date = DateTime.Now.Date;
            hdCodigoMotivo.Value = string.Empty;
            txtOrdenCompra.Text = string.Empty;
            txtCliente.Text = string.Empty;
            cmbAlmacen.SelectedValue = "0";
            Session["ListaPedidosVenta"] = new List<EBS12_PEDIDOS_VENTA.PedidoVentaGrid>();
            CargarGridPedidosVenta();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            CancelarPedidosVenta();
            CargarGridPedidosVenta();
        }

        protected void gridPedidosVenta_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data)
                return;

            if (e.GetValue("Resultado") == null)
                e.Row.ForeColor = System.Drawing.Color.Black;
            else if (e.GetValue("Resultado").ToString() == "NO")
                e.Row.ForeColor = System.Drawing.Color.Red;
            else if (e.GetValue("Resultado").ToString() == "SI")
                e.Row.ForeColor = System.Drawing.Color.Green;
            else
                e.Row.ForeColor = System.Drawing.Color.Black;
        }

        #endregion

        #region Metodos

        private void CargarAlmacenes()
        {
            try
            {
                EBS12_ALMACENES barcos = new EBS12_ALMACENES();
                decimal idEmpresaEBS12 = hdIdEmpresaEBS12.Value == string.Empty ? 0 : Convert.ToDecimal(hdIdEmpresaEBS12.Value);
                
                if (idEmpresaEBS12 > 0)
                {
                    if (barcos.ObtenerAlmacenesUnidadOperativa(idEmpresaEBS12))
                    {
                        cmbAlmacen.DataSource = barcos.Almacenes.items;
                        cmbAlmacen.DataTextField = "almacen";
                        cmbAlmacen.DataValueField = "idAlmacen";
                        cmbAlmacen.DataBind();
                        cmbAlmacen.Items.Insert(0, new ListItem() { Text = "-- Seleccione --", Value = "0" });
                    }
                }
            }
            catch (JsonException ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, ex.Message);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Error, ex.Message);
            }

        }

        private void ObtenerEmpresaIntegracion()
        {
            decimal idEmpresaSIP = Convert.ToDecimal(Session["EmpresaID"]);

            var empresaIntegracion = new GPO_EMPRESAS_INTEGRACIONES().ObtenerEmpresasIntegraciones(null, idEmpresaSIP, null, null, null, null, null).FirstOrDefault();

            if (empresaIntegracion != null && empresaIntegracion.Id_empresa_eb12 != null)
                hdIdEmpresaEBS12.Value = empresaIntegracion.Id_empresa_eb12.ToString();
            else
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "La empresa seleccionada no cuenta con integracion en EBS12");
        }

        private void ObtenerPedidosVenta()
        {
            List<EBS12_PEDIDOS_VENTA.PedidoVenta> listaPedidosVenta = new List<EBS12_PEDIDOS_VENTA.PedidoVenta>();
            List<EBS12_PEDIDOS_VENTA.PedidoVentaGrid> listaPedidosVentaGrid = new List<EBS12_PEDIDOS_VENTA.PedidoVentaGrid>();
            EBS12_PEDIDOS_VENTA pedidoVenta = new EBS12_PEDIDOS_VENTA();
            decimal index = 0;

            try
            {
                decimal idEmpresaEBS12 = hdIdEmpresaEBS12.Value == string.Empty ? 0 : Convert.ToDecimal(hdIdEmpresaEBS12.Value);
                string fechaInicio = dtpFechaInicio.Date.ToString("yyyy-MM-dd");
                string fechaFin = dtpFechaFin.Date.ToString("yyyy-MM-dd");
                string almacen = cmbAlmacen.SelectedValue == "0" ? string.Empty : cmbAlmacen.SelectedItem.Text;
                string cliente = txtCliente.Text;
                string ordenCompra = txtOrdenCompra.Text;

                listaPedidosVenta = pedidoVenta.ObtenerPedidosVenta(idEmpresaEBS12, fechaInicio, fechaFin, almacen, cliente, ordenCompra);

                if (listaPedidosVenta.Count > 0)
                {
                    listaPedidosVentaGrid = (from pv in listaPedidosVenta
                                             select new EBS12_PEDIDOS_VENTA.PedidoVentaGrid
                                             {
                                                 IdIndex = index++,
                                                 OrdenCompra = pv.orden_compra,
                                                 NoPedidoOracle = pv.no_pedido_oracle,
                                                 Almacen = pv.almacen,
                                                 ClienteFacturacion = pv.cliente_facturacion,
                                                 ClienteEntrega = pv.cliente_entrega,
                                                 Cliente = pv.cliente,
                                                 Formato = pv.formato,
                                                 FechaOC = pv.fecha_oc,
                                                 FechaCancelacion = pv.fecha_cancelacion,
                                                 Linea = pv.linea,
                                                 SKU = pv.sku,
                                                 Descripcion = pv.descripcion,
                                                 UDM = pv.udm,
                                                 Cantidad = pv.cantidad,
                                                 PrecioLista = pv.precio_lista,
                                                 PrecioUnitario = pv.precio_unitario,
                                                 TotalLinea = pv.total_linea,
                                                 Moneda = pv.moneda,
                                                 CodigoBarras = pv.codigo_barras,
                                                 Estatus = pv.estatus,
                                                 FechaCreacion = pv.fecha_creacion,
                                                 FechaUltimaActualizacion = pv.fecha_ultima_actualizacion,
                                                 HeaderId = pv.header_id,
                                                 LineId = pv.line_id
                                             }).ToList();
                }
            }
            catch (JsonException ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, ex.Message);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Error, ex.Message);
            }

            Session["ListaPedidosVenta"] = listaPedidosVentaGrid;
        }

        private void CancelarPedidosVenta()
        {
            List<object> fieldValues = gridPedidosVenta.GetSelectedFieldValues(new string[] { "IdIndex", "HeaderId" });
            List<decimal> listaFilasEditadas = new List<decimal>();

            foreach (object[] item in fieldValues)
            {
                listaFilasEditadas.Add(Convert.ToDecimal(item[0].ToString()));
            }

            EBS12_PEDIDOS_VENTA pedidoVenta = new EBS12_PEDIDOS_VENTA();
            List<EBS12_PEDIDOS_VENTA.PedidoVentaGrid> listaPedidosVentaGrid = Session["ListaPedidosVenta"] as List<EBS12_PEDIDOS_VENTA.PedidoVentaGrid>;
            List<EBS12_PEDIDOS_VENTA.PedidoVentaGrid> listaPedidosVentaModificadas = new List<EBS12_PEDIDOS_VENTA.PedidoVentaGrid>();

            try
            {
                decimal idEmpresaEBS12 = hdIdEmpresaEBS12.Value == string.Empty ? 0 : Convert.ToDecimal(hdIdEmpresaEBS12.Value);
                string codigo = hdCodigoMotivo.Value;

                if (codigo != string.Empty)
                {
                    EBS12_CODIGOS_MOTIVOS.CodigoMotivo codigoMotivo = new EBS12_CODIGOS_MOTIVOS.CodigoMotivo();
                    codigoMotivo.codigo = codigo;

                    if (listaPedidosVentaGrid.Count > 0)
                    {
                        listaPedidosVentaModificadas = listaPedidosVentaGrid.Where(x => listaFilasEditadas.Contains(x.IdIndex)).ToList();
                        List<EBS12_PEDIDOS_VENTA.PedidoVenta> listaPedidos = (from pv in listaPedidosVentaModificadas
                                                                              select new EBS12_PEDIDOS_VENTA.PedidoVenta
                                                                              {
                                                                                  no_pedido_oracle = pv.NoPedidoOracle,
                                                                                  header_id = pv.HeaderId,
                                                                                  line_id = pv.LineId
                                                                              }).ToList();

                        listaPedidos = pedidoVenta.CancelarPedidosVenta(idEmpresaEBS12, listaPedidos, codigoMotivo);
                        if (listaPedidos.Count > 0)
                        {
                            foreach (var pv in listaPedidos)
                            {
                                EBS12_PEDIDOS_VENTA.PedidoVentaGrid pedido = listaPedidosVentaGrid.Where(x => x.NoPedidoOracle == pv.no_pedido_oracle && x.HeaderId == pv.header_id && x.LineId == pv.line_id).FirstOrDefault();
                                pedido.Resultado = pv.resultado;
                                pedido.Mensaje = pv.mensaje;
                            }
                            gridPedidosVenta.Selection.UnselectAll();
                            MostrarMensaje(ControladorMensajes.TipoMensaje.Exito, "Se grabó correctamente la información");
                        }
                    }
                }
                else
                {
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "No se ha seleccionado el codigo motivo.");
                }
            }
            catch (JsonException ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, ex.Message);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Error, ex.Message);
            }

            Session["ListaPedidosVenta"] = listaPedidosVentaGrid;
        }

        private void CargarGridPedidosVenta()
        {
            List<EBS12_PEDIDOS_VENTA.PedidoVentaGrid> listaPedidosVentaGrid = Session["ListaPedidosVenta"] as List<EBS12_PEDIDOS_VENTA.PedidoVentaGrid>;
            gridPedidosVenta.DataSource = listaPedidosVentaGrid;
            gridPedidosVenta.KeyFieldName = "IdIndex";
            gridPedidosVenta.DataBind();
        }

        private void MostrarMensaje(ControladorMensajes.TipoMensaje tipo, dynamic sms, int? tiempoMostrar = 7000)
        {
            ControladorMensajes.MostrarMensaje(UpdatePanel1, string.Format("CAPRECMP_{0}{1}", tipo, new Random().Next(0, 99)), tipo, sms, tiempoMostrar);
        }

        #endregion

    }
}
