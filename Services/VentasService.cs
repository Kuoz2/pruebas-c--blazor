using Microsoft.EntityFrameworkCore;
using puntodeventa.Data;
using puntodeventa.Models;
using Microsoft.AspNetCore.Mvc;


namespace puntodeventa.Services
{
    [Route("api/ventas/")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly VentasService _ventasService;
        public VentasController(VentasService ventasService)
        {
            _ventasService = ventasService;
        }
        [HttpPost("createventa")]
        public async Task<IActionResult> CreateVenta( Ventas ventas)
        {
            try
            {
                if (ventas == null)
                {
                    return BadRequest("No se recibieron los datos correctamente");
                }

                await _ventasService.CreateVentasAsync(ventas);
                return Ok(new { success = "Venta guardada" });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new
                {
                    error = "Error al procesar la venta",
                    details = dbEx.Message,
                    innerException = dbEx.InnerException?.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Error inesperado al procesar la venta",
                    details = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }


        }
    }

    public class VentasService
    {
        private readonly ApplicationDbContext _context;

        public VentasService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Ventas>> GetVentasAsync()
        {
            return await _context.polls_ventas.Include(v => v.Productos).ToListAsync();
        }
        public async Task<Ventas> GetVentaByIdAsync(int id)
        {
            return await _context.polls_ventas.Include(v => v.Productos)
                .FirstOrDefaultAsync(v => v.id == id);
        }
        public async Task<Ventas> CreateVentasAsync(Ventas ventas)
        {
            Console.WriteLine(ventas);
            try
            {
                var exisisteProducto = await _context.polls_productos.FindAsync(ventas.Productoid);
                if (exisisteProducto == null)
                {
                    throw new Exception("El producto relacionado no existe");
                }
                ventas.Productos = exisisteProducto;
                _context.polls_ventas.Add(ventas);
                await _context.SaveChangesAsync();
                return ventas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Excepción interna: {ex.InnerException.Message}");
                }
                throw;
            }
        }
        public async Task UpdateVentasAsync(Ventas ventas)
        {
            _context.Entry(ventas).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DelteVentasAsync(int id)
        {
            var venta = await _context.polls_ventas.FindAsync(id);
            if (venta != null)
            {
                _context.polls_ventas.Remove(venta);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddVenta(Ventas ventas)
        {
            if( ventas  == null)
            {
                throw new ArgumentNullException(nameof(ventas));
            }
            _context.polls_ventas.Add(ventas);
            await _context.SaveChangesAsync();
        }
        
        public async Task<List<Ventas>> GetVentasDiarias()
        {
            DateTime fechaActual = DateTime.Now;
            string fechaFormateada = fechaActual.ToString("dd/M/yyyy");
            return await _context.polls_ventas.Include(v => v.Productos).Where(v => v.Fechav == fechaFormateada).ToListAsync();

        }
        public async Task<int> GetTotalVentaNow()
        {
            DateTime fechaActual = DateTime.Now;
            string fechaFormateada = fechaActual.ToString("dd/M/yyyy");
            var sumaTotal = await _context.polls_ventas.Where(v => v.Fechav == fechaFormateada).SumAsync(v => v.Preciov);
            return sumaTotal;

        }
    }
}
