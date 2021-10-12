using System;

namespace Tribunet.Atv.Services
{
    public class ModelDataProvider
    {
        public class Clave
        {
            public tipoComprobante TipoComprobante { get; }
            public int OficinaId { get; }
            public int PuntoDeVentaId { get; }
            public int ConsecutivoComprobante { get; }

            public const int OficinaPrincipal = 1;

            public const int PuntoDeVentaUnico = 1;

            public enum tipoComprobante : byte
            {
                FacturaElectronica = 1,
                NotaDeDebitoElectronica = 2,
                NotaDeCreditoElectronica = 3,
                TiqueteElectronico = 4,
                ConfirmacionDeAceptacionDelComprobanteElectrónico = 5,
                ConfirmacionDeAceptacionParcialDelComprobanteElectronico = 6,
                ConfirmacionDeRechazoDelComprobanteElectronico = 7,
            }

            public Clave(
                tipoComprobante tipoComprobante,
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

            public static implicit operator string(Clave d) => d == null ? string.Empty :
                string.Concat(
                    d.OficinaId.ToString().PadLeft(3, '0'),
                    d.PuntoDeVentaId.ToString().PadLeft(5, '0'),
                    Convert.ToInt32(d.TipoComprobante).ToString().PadLeft(2, '0'),
                    d.ConsecutivoComprobante.ToString().PadLeft(10, '0')
                );
        }
    }
}
