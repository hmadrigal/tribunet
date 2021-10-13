using System;
using System.Globalization;

namespace Tribunet.Atv.Services
{
    public class ModelDataProvider
    {


    }

    public static class ModelDataProviderExtensions
    {
        public static string ToRfc3339String(this DateTime dateTime)
            => dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);
    }

    public class NumeroConsecutivo
    {
        public TipoComprobante TipoComprobante { get; }
        public int OficinaId { get; }
        public int PuntoDeVentaId { get; }
        public int ConsecutivoComprobante { get; }

        public const int OficinaPrincipal = 1;

        public const int PuntoDeVentaUnico = 1;

        public NumeroConsecutivo(
            TipoComprobante tipoComprobante,
            int oficinaId = OficinaPrincipal,
            int puntoDeVentaId = PuntoDeVentaUnico,
            int consecutivoComprobante = 0)
        {
            if (oficinaId <= 0 || oficinaId > 999)
                throw new ArgumentOutOfRangeException(nameof(oficinaId),
                    $"{nameof(oficinaId)} should be between 1 and 999 inclusive");

            if (puntoDeVentaId < 0 || puntoDeVentaId > 99999)
                throw new ArgumentOutOfRangeException(nameof(puntoDeVentaId),
                    $"{nameof(puntoDeVentaId)} should be between 0 and 99999 inclusive");

            if (puntoDeVentaId < 0)
                throw new ArgumentOutOfRangeException(nameof(puntoDeVentaId),
                    $"{nameof(puntoDeVentaId)} should be greater than 0");

            if (consecutivoComprobante < 0)
                throw new ArgumentOutOfRangeException(nameof(consecutivoComprobante),
                    $"{nameof(consecutivoComprobante)} should be greater than 0");
            TipoComprobante = tipoComprobante;
            OficinaId = oficinaId;
            PuntoDeVentaId = puntoDeVentaId;
            ConsecutivoComprobante = consecutivoComprobante;
        }

        public override string ToString()
            => string.Concat(
                OficinaId.ToString().PadLeft(3, '0'),
                PuntoDeVentaId.ToString().PadLeft(5, '0'),
                Convert.ToInt32(TipoComprobante).ToString().PadLeft(2, '0'),
                ConsecutivoComprobante.ToString().PadLeft(10, '0')
            );

        public static implicit operator string(NumeroConsecutivo d) => d == null ? string.Empty : d.ToString();

    }

    public class ClaveNumerica
    {
        public int CodigoPais { get; }
        public DateTime FechaCreacion { get; }
        public string CedulaEmisor { get; }
        public NumeroConsecutivo NumeracionConsecutivo { get; }
        public SituacionDelComprobante Situacion { get; }
        public int CodigoSeguridad { get; }

        public ClaveNumerica(
            int codigoPais,
            DateTime fechaCreacion,
            string cedulaEmisor,
            NumeroConsecutivo numeracionConsecutivo,
            SituacionDelComprobante situacion,
            int codigoSeguridad
        )
        {
            if (codigoPais < 0 && codigoPais > 999)
                throw new ArgumentOutOfRangeException(nameof(codigoPais), $"{nameof(codigoPais)} should be between 0 and 999.");

            if (string.IsNullOrWhiteSpace(cedulaEmisor))
                throw new ArgumentNullException(nameof(cedulaEmisor), $"{nameof(cedulaEmisor)} should have a non empty value");

            if (cedulaEmisor.Length > 12)
                throw new ArgumentException(nameof(cedulaEmisor), $"{nameof(cedulaEmisor)} should have less than 12 characters.");

            if (codigoSeguridad < 0 || codigoSeguridad > 99999999)
                throw new ArgumentOutOfRangeException(nameof(codigoSeguridad), $"{nameof(codigoSeguridad)} should be between 0 and 99999999.");
            CodigoPais = codigoPais;
            FechaCreacion = fechaCreacion;
            CedulaEmisor = cedulaEmisor;
            NumeracionConsecutivo = numeracionConsecutivo;
            Situacion = situacion;
            CodigoSeguridad = codigoSeguridad;
        }

        public override string ToString()
            => string.Concat(
                CodigoPais.ToString().PadLeft(3, '0'),
                FechaCreacion.Day.ToString().PadLeft(2, '0'),
                FechaCreacion.Month.ToString().PadLeft(2, '0'),
                new string(FechaCreacion.Year.ToString()[^2..]).PadLeft(2, '0'),
                CedulaEmisor.PadLeft(12, '0'),
                NumeracionConsecutivo,
                Convert.ToInt32(Situacion),
                CodigoSeguridad.ToString().PadLeft(8, '0')
            );

        public static implicit operator string(ClaveNumerica d) => d == null ? string.Empty : d.ToString();

    }

    public enum TipoComprobante : byte
    {
        FacturaElectronica = 1,
        NotaDeDebitoElectronica = 2,
        NotaDeCreditoElectronica = 3,
        TiqueteElectronico = 4,
        ConfirmacionDeAceptacionDelComprobanteElectrónico = 5,
        ConfirmacionDeAceptacionParcialDelComprobanteElectronico = 6,
        ConfirmacionDeRechazoDelComprobanteElectronico = 7,
    }

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
