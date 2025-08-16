namespace WebApiTIenda.Domian.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() { }

        public NotFoundException(string message)
            : base(message) { }

        public NotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}

//var producto = await _context.Productos.FindAsync(id);
//if (producto == null)
//    throw new NotFoundException($"Producto con Id {id} no encontrado.");

