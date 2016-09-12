using System;

namespace MetaData
{
    /// <summary>
	/// Een attribuut waarmee een methode gemarkeerd kan worden om te laten testen door de AssemblyLezer.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class TestAttribute : Attribute
	{
		/// <summary>
		/// Het resultaat dat de methode terug moet geven.
		/// </summary>
		/// <value>
		/// The verwacht resultaat.
		/// </value>
		public object ExpectedResult { get; set; }

		/// <summary>
		/// Parameters om mee te geven aan de te testen methode.
		/// </summary>
		/// <value>
		/// The parameters.
		/// </value>
		public object[] Parameters { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TestAttribute"/> class.
		/// </summary>
		/// <param name="expectedResult">Het resultaat dat de te testen methode terug zou moeten geven.</param>
		/// <param name="parameters">De parameters die mee moeten worden gegeven aan de te testen methode.</param>
		public TestAttribute(object expectedResult, params object[] parameters)
		{
			this.ExpectedResult = expectedResult;
			this.Parameters = parameters;
		}
	}
}
