using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Tribunet.Atv.Services
{
    [DebuggerDisplay("CodigoPais={CodigoPais} FechaCreacion={FechaCreacion} CedulaEmisor={CedulaEmisor} NumeracionConsecutivo={NumeracionConsecutivo} Situacion={Situacion} CodigoSeguridad={CodigoSeguridad}")]
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

        public static implicit operator string(ClaveNumerica instance) => instance == null ? string.Empty : instance.ToString();

        public static implicit operator ClaveNumerica(string text)
        {
            if (string.IsNullOrEmpty(text))
                return default;

            if (text.Length != 50)
                throw new InvalidCastException("string must be 50 length");

            if (text.Any(c => !char.IsDigit(c)))
                throw new InvalidCastException("string must contain only digits");

            int.TryParse(text[0..3], out var codigoPais);
            DateTime.TryParseExact(text[3..9], "ddMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fechaCreacion);
            string cedulaEmisor = text[9..21];
            NumeroConsecutivo numeracionConsecutivo = text[21..41];
            SituacionDelComprobante situacion =  (SituacionDelComprobante)int.Parse(text[41..42]);
            int.TryParse(text[42..50], out var codigoSeguridad);


            var claveNumerica = new ClaveNumerica(codigoPais,fechaCreacion,cedulaEmisor,numeracionConsecutivo,situacion,codigoSeguridad);
            return claveNumerica;

        }

    }
}
