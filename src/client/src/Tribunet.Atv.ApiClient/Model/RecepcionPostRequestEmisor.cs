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
    /// RecepcionPostRequestEmisor
    /// </summary>
    [DataContract(Name = "RecepcionPostRequest_emisor")]
    public partial class RecepcionPostRequestEmisor : IEquatable<RecepcionPostRequestEmisor>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecepcionPostRequestEmisor" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected RecepcionPostRequestEmisor() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RecepcionPostRequestEmisor" /> class.
        /// </summary>
        /// <param name="tipoIdentificacion">tipoIdentificacion (required).</param>
        /// <param name="numeroIdentificacion">numeroIdentificacion (required).</param>
        public RecepcionPostRequestEmisor(string tipoIdentificacion = default(string), string numeroIdentificacion = default(string))
        {
            // to ensure "tipoIdentificacion" is required (not null)
            if (tipoIdentificacion == null) {
                throw new ArgumentNullException("tipoIdentificacion is a required property for RecepcionPostRequestEmisor and cannot be null");
            }
            this.TipoIdentificacion = tipoIdentificacion;
            // to ensure "numeroIdentificacion" is required (not null)
            if (numeroIdentificacion == null) {
                throw new ArgumentNullException("numeroIdentificacion is a required property for RecepcionPostRequestEmisor and cannot be null");
            }
            this.NumeroIdentificacion = numeroIdentificacion;
        }

        /// <summary>
        /// Gets or Sets TipoIdentificacion
        /// </summary>
        [DataMember(Name = "tipoIdentificacion", IsRequired = true, EmitDefaultValue = false)]
        public string TipoIdentificacion { get; set; }

        /// <summary>
        /// Gets or Sets NumeroIdentificacion
        /// </summary>
        [DataMember(Name = "numeroIdentificacion", IsRequired = true, EmitDefaultValue = false)]
        public string NumeroIdentificacion { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RecepcionPostRequestEmisor {\n");
            sb.Append("  TipoIdentificacion: ").Append(TipoIdentificacion).Append("\n");
            sb.Append("  NumeroIdentificacion: ").Append(NumeroIdentificacion).Append("\n");
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
            return this.Equals(input as RecepcionPostRequestEmisor);
        }

        /// <summary>
        /// Returns true if RecepcionPostRequestEmisor instances are equal
        /// </summary>
        /// <param name="input">Instance of RecepcionPostRequestEmisor to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RecepcionPostRequestEmisor input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.TipoIdentificacion == input.TipoIdentificacion ||
                    (this.TipoIdentificacion != null &&
                    this.TipoIdentificacion.Equals(input.TipoIdentificacion))
                ) && 
                (
                    this.NumeroIdentificacion == input.NumeroIdentificacion ||
                    (this.NumeroIdentificacion != null &&
                    this.NumeroIdentificacion.Equals(input.NumeroIdentificacion))
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
                if (this.TipoIdentificacion != null)
                    hashCode = hashCode * 59 + this.TipoIdentificacion.GetHashCode();
                if (this.NumeroIdentificacion != null)
                    hashCode = hashCode * 59 + this.NumeroIdentificacion.GetHashCode();
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
            // TipoIdentificacion (string) maxLength
            if(this.TipoIdentificacion != null && this.TipoIdentificacion.Length > 2)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for TipoIdentificacion, length must be less than 2.", new [] { "TipoIdentificacion" });
            }

            // NumeroIdentificacion (string) maxLength
            if(this.NumeroIdentificacion != null && this.NumeroIdentificacion.Length > 12)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for NumeroIdentificacion, length must be less than 12.", new [] { "NumeroIdentificacion" });
            }

            yield break;
        }
    }

}
