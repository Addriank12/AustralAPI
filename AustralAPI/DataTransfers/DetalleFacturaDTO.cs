namespace AustralAPI.DataTransfers
{
    public class DetalleFacturaDTO
    {
        public long IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
