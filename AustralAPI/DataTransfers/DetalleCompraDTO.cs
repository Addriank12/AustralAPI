namespace AustralAPI.DataTransfers
{
    public class DetalleCompraDTO
    {
        public long IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Precio { get; set; }
    }
}
