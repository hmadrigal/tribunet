namespace Tribunet.Atv.Services
{
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
}
