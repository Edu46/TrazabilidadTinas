using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace LogisticaERP.Clases
{
    public class EBS12_PEDIDOS_VENTA
    {
        public List<PedidoVenta> pedidos_venta { get; set; }
        public string resultado { get; set; }
        public string mensaje { get; set; }
        public string error { get; set; }

        [Serializable]
        public class PedidoVenta
        {
            public string orden_compra { get; set; }
            public decimal no_pedido_oracle { get; set; }
            public string almacen { get; set; }
            public string cliente_facturacion { get; set; }
            public string cliente_entrega { get; set; }
            public string cliente { get; set; }
            public string formato { get; set; }
            public DateTime fecha_oc { get; set; }
            public DateTime fecha_cancelacion { get; set; }
            public decimal linea { get; set; }
            public string sku { get; set; }
            public string descripcion { get; set; }
            public string udm { get; set; }
            public decimal cantidad { get; set; }
            public decimal precio_lista { get; set; }
            public decimal precio_unitario { get; set; }
            public decimal total_linea { get; set; }
            public string moneda { get; set; }
            public string codigo_barras { get; set; }
            public string estatus { get; set; }
            public DateTime fecha_creacion { get; set; }
            public DateTime fecha_ultima_actualizacion { get; set; }
            public decimal header_id { get; set; }
            public decimal line_id { get; set; }
            public string resultado { get; set; }
            public string mensaje { get; set; }

        }

        [Serializable]
        public class PedidoVentaGrid
        {
            public decimal IdIndex { get; set; }
            public string OrdenCompra { get; set; }
            public decimal NoPedidoOracle { get; set; }
            public string Almacen { get; set; }
            public string ClienteFacturacion { get; set; }
            public string ClienteEntrega { get; set; }
            public string Cliente { get; set; }
            public string Formato { get; set; }
            public DateTime FechaOC { get; set; }
            public DateTime FechaCancelacion { get; set; }
            public decimal Linea { get; set; }
            public string SKU { get; set; }
            public string Descripcion { get; set; }
            public string UDM { get; set; }
            public decimal Cantidad { get; set; }
            public decimal PrecioLista { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal TotalLinea { get; set; }
            public string Moneda { get; set; }
            public string CodigoBarras { get; set; }
            public string Estatus { get; set; }
            public DateTime FechaCreacion { get; set; }
            public DateTime FechaUltimaActualizacion { get; set; }
            public decimal HeaderId { get; set; }
            public decimal LineId { get; set; }
            public string Resultado { get; set; }
            public string Mensaje { get; set; }
        }

        public List<PedidoVenta> ObtenerPedidosVenta(decimal idEmpresaEBS12, string fechaInicio, string fechaFin, string almacen, string clientePV, string ordenCompra)
        {
            EBS12_PEDIDOS_VENTA pedidoVenta = new EBS12_PEDIDOS_VENTA();
            string json;

            try
            {
                ClaseHttpCliente cliente = new ClaseHttpCliente();
                var response = ClaseHttpCliente.cliente.GetAsync("/cancelacion/pv/ebs12/1.0/empresa/" + idEmpresaEBS12 + "/pedidos-venta?fecha_inicio=" + fechaInicio + "&fecha_fin=" + fechaFin + "&almacen=" + almacen + "&cliente=" + clientePV + "&orden_compra=" + ordenCompra).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    pedidoVenta = Newtonsoft.Json.JsonConvert.DeserializeObject<EBS12_PEDIDOS_VENTA>(json);
                }

                if (pedidoVenta.resultado == null || pedidoVenta.resultado == "NO")
                {
                    if (pedidoVenta.error == "ERROR_VALIDACION")
                        throw new JsonException(pedidoVenta.mensaje);
                    else
                        throw new Exception(pedidoVenta.mensaje);
                }
                return pedidoVenta.pedidos_venta;

            }
            catch (JsonException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PedidoVenta> CancelarPedidosVenta(decimal idEmpresaEBS12, List<PedidoVenta> listaPedidosVenta, EBS12_CODIGOS_MOTIVOS.CodigoMotivo codigoMotivo)
        {
            EBS12_PEDIDOS_VENTA pedidoVenta = new EBS12_PEDIDOS_VENTA();
            string json;

            try
            {
                ClaseHttpCliente cliente = new ClaseHttpCliente();
                string jsonViajes = JsonConvert.SerializeObject(new { pedidos_venta = listaPedidosVenta, codigo_motivo = codigoMotivo });
                HttpContent inputContent = new StringContent(jsonViajes, Encoding.UTF8, "application/json");
                HttpResponseMessage response = ClaseHttpCliente.cliente.PostAsync("/cancelacion/pv/ebs12/1.0/empresa/" + idEmpresaEBS12.ToString() + "/pedidos-venta/cancelacion", inputContent).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    pedidoVenta = JsonConvert.DeserializeObject<EBS12_PEDIDOS_VENTA>(json);
                }

                if (pedidoVenta.resultado == null || pedidoVenta.resultado == "NO")
                {
                    if (pedidoVenta.error == "ERROR_VALIDACION")
                        throw new JsonException(pedidoVenta.mensaje);
                    else
                        throw new Exception(pedidoVenta.mensaje);
                }
                return pedidoVenta.pedidos_venta;
            }
            catch (JsonException ex)
            {
                throw new JsonException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}