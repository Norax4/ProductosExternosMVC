using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductosExternosMVC.Models;
using ProductosExternosMVC.Services;

namespace ProductosExternosMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ServicioProductos servicioProductos = new ServicioProductos();

            ViewBag.Productos = await servicioProductos.Todos();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CrearProducto(CrearProductoDto _crearProductoDto)
        {
            // Validaciones varias
            // ....

            ServicioProductos servicioProductos = new ServicioProductos();
            ProductoDto? producto = await servicioProductos.CrearProducto(_crearProductoDto.nombre, _crearProductoDto.precio);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> BorrarProducto(string id)
        {
            ServicioProductos servicioProductos = new ServicioProductos();
            await servicioProductos.Borrar(id);
            return RedirectToAction("Index"); //Funciona correctamente
        }

        public async Task<IActionResult> Modificar(string id)
        {
            ServicioProductos servicioProductos = new ServicioProductos();
            ProductoDto productoDto = await servicioProductos.Buscar(id);

            if (productoDto == null)
            {
                return View("Index");
            }

            return View(productoDto);
        }

        [HttpPost]
        public async Task<IActionResult> ModificarProducto(ModificarProductoDto productoModif)
        {
            ServicioProductos servicioProductos = new ServicioProductos();
            ProductoDto producto = await servicioProductos.Buscar(productoModif.id);

            producto.Nombre = productoModif.nombre;
            producto.Precio = productoModif.precio;
            producto.UpdatedAt = DateTime.Now.ToString("o");

            await servicioProductos.Modificar(producto);
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
