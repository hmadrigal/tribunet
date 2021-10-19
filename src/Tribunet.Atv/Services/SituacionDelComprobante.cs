namespace Tribunet.Atv.Services
{
    public enum SituacionDelComprobante : int
    {
        /// <summary>
        /// Corresponde aquellos comprobantes electrónicos que
        /// son generados y transmitidos en el mismo acto de
        /// compraventa y prestación del servicio al sistema de
        /// validación de comprobantes electrónicos de la
        /// Dirección General de Tributación, conforme con lo
        /// establecido en la presente resolución.
        /// </summary>
        Normal,
        /// <summary>
        /// Corresponde aquellos comprobantes electrónicos que
        /// sustituyen al comprobante físico emitido por
        /// contingencia, conforme lo estipulado en el artículo 15
        /// de la presente resolución. 
        /// </summary>
        Contingencia,
        /// <summary>
        /// Corresponde aquellos comprobantes que han sido
        /// generados y expresados en formato electrónico
        /// conforme lo establecido en la presente resolución,
        /// pero no se cuenta con el respectivo acceso a internet
        /// para el envío inmediato de los mismos a la Dirección
        /// General de Tributación, esto conforme lo indicado en
        /// el artículo 9 párrafo segundo de la presente
        /// resolución.
        /// </summary>
        SinInternet
    }
}
