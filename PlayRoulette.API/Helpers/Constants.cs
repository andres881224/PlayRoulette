namespace PlayRoulette.API.Helpers
{
    public static class Constants
    {
        public static string ConnectionName = "DefaultConnection";
        public static string TokensKey = "Tokens:Key";
        public static string TokensIssuer = "Tokens:Issuer";
        public static string TokensAudience = "Tokens:Audience";
        public static int DayNumberThree = 3;
        public static string MessageLogCreateRoulette = "CreateRoulette";
        public static string MessageLogOpenRoulette = "OpenRoulette";
        public static string MessageLogCloseRoulette = "CloseRoulette";
        public static string MessageLogGetStatus = "GetStatus";
        public static string MessageSuccessful = "Exitosa";
        public static string MessageDenied = "Denegada";
        public static string MessageBetCreate = "Apuesta Creada";
        public static string MessageBetUser = "El usuario no existe.";
        public static string MessageBetValue = "El valor de la apuesta supera el saldo del usuario.";
        public static string MessageBetOpen = "La ruleta no se encuentra abierta.";



    }
}
