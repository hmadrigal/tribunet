/*
 * Atv Api
 *
 * API de comprobantes electrónicos para la administración tributaria virtual
 *
 * The version of the OpenAPI document: 1.0.0
 * Contact: fe@hacienda.go.cr
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = Tribunet.Atv.ApiClient.Client.OpenAPIDateConverter;

namespace Tribunet.Atv.ApiClient.Model
{
    /// <summary>
    /// ComprobanteNotasCredito
    /// </summary>
    [DataContract(Name = "Comprobante_notasCredito")]
    public partial class ComprobanteNotasCredito : IEquatable<ComprobanteNotasCredito>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComprobanteNotasCredito" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected ComprobanteNotasCredito() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ComprobanteNotasCredito" /> class.
        /// </summary>
        /// <param name="clave">clave (required).</param>
        /// <param name="fecha">Fecha de la factura en formato [yyyy-MM-dd&#39;T&#39;HH:mm:ssZ] como se define en [http://tools.ietf.org/html/rfc3339#section-5.6] (date-time). (required).</param>
        public ComprobanteNotasCredito(string clave = default(string), string fecha = default(string))
        {
            this.Clave = clave;
            this.Fecha = fecha;
        }

        /// <summary>
        /// Gets or Sets Clave
        /// </summary>
        [DataMember(Name = "clave", IsRequired = true, EmitDefaultValue = false)]
        public string Clave { get; set; }

        /// <summary>
        /// Fecha de la factura en formato [yyyy-MM-dd&#39;T&#39;HH:mm:ssZ] como se define en [http://tools.ietf.org/html/rfc3339#section-5.6] (date-time).
        /// </summary>
        /// <value>Fecha de la factura en formato [yyyy-MM-dd&#39;T&#39;HH:mm:ssZ] como se define en [http://tools.ietf.org/html/rfc3339#section-5.6] (date-time).</value>
        [DataMember(Name = "fecha", IsRequired = true, EmitDefaultValue = false)]
        public string Fecha { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ComprobanteNotasCredito {\n");
            sb.Append("  Clave: ").Append(Clave).Append("\n");
            sb.Append("  Fecha: ").Append(Fecha).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as ComprobanteNotasCredito);
        }

        /// <summary>
        /// Returns true if ComprobanteNotasCredito instances are equal
        /// </summary>
        /// <param name="input">Instance of ComprobanteNotasCredito to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ComprobanteNotasCredito input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Clave == input.Clave ||
                    (this.Clave != null &&
                    this.Clave.Equals(input.Clave))
                ) && 
                (
                    this.Fecha == input.Fecha ||
                    (this.Fecha != null &&
                    this.Fecha.Equals(input.Fecha))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Clave != null)
                    hashCode = hashCode * 59 + this.Clave.GetHashCode();
                if (this.Fecha != null)
                    hashCode = hashCode * 59 + this.Fecha.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
        {
            // Clave (string) maxLength
            if(this.Clave != null && this.Clave.Length > 50)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Clave, length must be less than 50.", new [] { "Clave" });
            }

            yield break;
        }
    }

}
