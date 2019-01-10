using Frontend.Core.Model.Preferences.Interfaces;

namespace Frontend.Core.Model.Preferences
{
	public class NumericPreference : Preference<double>, INumericPreference
	{
		protected override string OnValidateProperty(string propertyName)
		{
			if (MinValue >= double.MinValue && MaxValue >= double.MinValue)
			{
				return ValidateNumericValues();
			}

			return null;
		}

		/// <summary>
		///     Validating numeric values
		/// </summary>
		/// <param name="propertyName">The name of the property to validate</param>
		/// <returns>A string describing any problems with the current value</returns>
		private string ValidateNumericValues()
		{
			//TODO: This currently accepts both "," and "." as delimeters for some culture settings. Which can lead to confusing number parsing

			if (Value < MinValue || Value > MaxValue)
			{
				return "The value must be within the limits specified by the minimum and maximum value, currently " +
						 MinValue + " and " + MaxValue + " respectively.";
			}

			return null;
		}
	}
}