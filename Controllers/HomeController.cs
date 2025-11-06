using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductosExternosMVC.Models;
using ProductosExternosMVC.Services;

namespace ProductosExternosMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServicioProductos _servicioProductos;

        public HomeController(ILogger<HomeController> logger, IServicioProductos servicioProds)
        {
            _logger = logger;
            _servicioProductos = servicioProds;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Productos = await _servicioProductos.Todos();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResultadoBusqueda(string nombre)
        {
            ViewBag.ProductosBuscados = await _servicioProductos.BuscarPorNombre(nombre);
            Console.WriteLine(ViewBag.ProductosBuscados.Count);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearProducto(CrearProductoDto _crearProductoDto)
        {
            await _servicioProductos.CrearProducto(_crearProductoDto);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> BorrarProducto(string id)
        {
            await _servicioProductos.Borrar(id);
            return RedirectToAction("Index"); //Funciona correctamente
        }

        public async Task<IActionResult> Modificar(string id)
        {
            ProductoDto productoDto = await _servicioProductos.Buscar(id);

            if (productoDto == null)
            {
                return View("Index");
            }

            return View(productoDto);
        }

        [HttpPost]
        public async Task<IActionResult> ModificarProducto(ModificarProductoDto productoModif)
        {
            Console.WriteLine(productoModif.precio);
            await _servicioProductos.Modificar(productoModif);
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
