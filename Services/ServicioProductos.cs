using ProductosExternosMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductosExternosMVC.Services
{
    public interface IServicioProductos
    {
        Task CrearProducto(CrearProductoDto crearProductoDto);
        Task BuscarMostrar(string id);
        Task<List<ProductoDto>> BuscarPorNombre(string nombre);
        Task<ProductoDto> Buscar(string id);
        Task Borrar(string id);
        Task Modificar(ModificarProductoDto productoDto);
        Task<List<ProductoDto>> Todos();
    }

    // Creamos un Dto para mapear los productos, es necesario para serializar y deserializar
    // La respuesta JSON
    public class ProductoDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("precio")]
        public decimal? Precio { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class ServicioProductos : IServicioProductos
    {
        public readonly string _apiUrl;
        public readonly HttpClient _httpClient;

        public ServicioProductos()
        {
            _apiUrl = "https://localhost:7221/api/Producto";
            _httpClient = new HttpClient();
        }

        public async Task<ProductoDto> Buscar(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiUrl}/id?id={id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                ProductoDto? producto = JsonSerializer.Deserialize<ProductoDto>(json);
                return producto!;
            }
            else
            {
                Console.WriteLine($"Error al buscar el producto: {response.StatusCode}");
                return null!;
            }
        }

        public async Task<List<ProductoDto>> BuscarPorNombre(string name)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiUrl}/name?name={name}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<ProductoDto> productos = JsonSerializer.Deserialize<List<ProductoDto>>(json);
                return productos;
            } else
            {
                return new List<ProductoDto>();
            }
        }

        public async Task Modificar(ModificarProductoDto productoDto)
        {
            string json = JsonSerializer.Serialize(productoDto);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{_apiUrl}/{productoDto.id}", content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Producto con ID {productoDto.id} modificado correctamente.");
            }
            else
            {
                Console.WriteLine($"Error al modificar el producto: {response.StatusCode}");
            }
        }

        public async Task Borrar(string id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Producto con ID {id} eliminado correctamente.");
            }
            else
            {
                Console.WriteLine($"Error al eliminar el producto: {response.StatusCode}");
            }
        }

        public async Task BuscarMostrar(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                ProductoDto? producto = JsonSerializer.Deserialize<ProductoDto>(json);
                Console.WriteLine($"Producto encontrado:");
                Console.WriteLine($"ID: {producto!.Id}");
                Console.WriteLine($"Nombre: {producto.Nombre}");
                Console.WriteLine($"Precio: {producto.Precio}");
                Console.WriteLine($"Fecha de Creación: {producto.CreatedAt}");
            }
            else
            {
                Console.WriteLine($"Error al buscar el producto: {response.StatusCode}");
            }
        }
        public async Task CrearProducto(CrearProductoDto crearProductoDto)
        {
            string json = JsonSerializer.Serialize(crearProductoDto);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Producto creado correctamente.");
            }
            else
            {
                Console.WriteLine($"Error al crear el producto: {response.StatusCode}");
            }
        }
        public async Task<List<ProductoDto>> Todos()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<ProductoDto> productos = JsonSerializer.Deserialize<List<ProductoDto>>(json);
                return productos;
            }
            else
            {
                return new List<ProductoDto>();
            }
        }
    }
}
