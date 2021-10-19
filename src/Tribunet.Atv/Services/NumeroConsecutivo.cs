using System;
using System.Diagnostics;
using System.Linq;

namespace Tribunet.Atv.Services
{
    [DebuggerDisplay("TipoComprobante={TipoComprobante} Oficina={OficinaId} PuntoDeVenta={PuntoDeVentaId} Consecutivo={ConsecutivoComprobante}")]
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

        public static implicit operator NumeroConsecutivo(string text)
        {
            if (string.IsNullOrEmpty(text))
                return default;

            if (text.Length != 20)
                throw new InvalidCastException("string must be 20 length");

            if (text.Any(c => !char.IsDigit(c)))
                throw new InvalidCastException("string must contain only digits");


            int.TryParse(text[0..3], out var oficinaId);
            int.TryParse(text[3..8], out var puntoDeVentaId);
            TipoComprobante tipoComprobante = (TipoComprobante)int.Parse(text[8..10]);
            int.TryParse(text[10..20], out var consecutivoComprobante);

            var numeroConsecutivo = new NumeroConsecutivo(tipoComprobante, oficinaId, puntoDeVentaId, consecutivoComprobante);
            return numeroConsecutivo;

        }

    }
}
