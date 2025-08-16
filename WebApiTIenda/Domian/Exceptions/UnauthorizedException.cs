namespace WebApiTIenda.Domian.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() { }

        public UnauthorizedException(string message)
            : base(message) { }

        public UnauthorizedException(string message, Exception inner)
            : base(message, inner) { }
    }
}


//if (!usuarioEsAdmin)
//    throw new UnauthorizedException("El usuario no tiene permisos para realizar esta acción.");